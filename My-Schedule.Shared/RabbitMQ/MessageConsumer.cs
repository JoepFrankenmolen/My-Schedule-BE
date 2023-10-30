using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using IModel = RabbitMQ.Client.IModel;

namespace My_Schedule.Shared.RabbitMQ
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IMessageQueueSettings _appSettings;
        private readonly ILogger<MessageConsumer> _logger;
        private readonly string _serviceName;
        private readonly string _headerName;

        public MessageConsumer(IMessageQueueSettings appSettings, ILogger<MessageConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentException(nameof(appSettings));
            _connection = new ConnectionFactoryWrapper(appSettings, _logger).CreateConnection();
            _channel = _connection.CreateModel();
            _serviceName = _appSettings.MessageQueueServiceName;
            _headerName = _appSettings.MessageQueueHeaderName;
        }

        /// <summary>
        /// Use to start consuming on a queue.
        /// </summary>
        /// <typeparam name="T">The messageType</typeparam>
        /// <param name="messageHandler">The function in which the message will be processed.</param>
        /// <param name="queueName">The name of the queue.</param>
        /// <param name="isFanExchange">True if the queue you are listening to is a fan exchange.</param>
        public void StartConsuming<T>(Func<T, Task> messageHandler, string queueName, bool isFanExchange = false)
        {
            string exchangeName = isFanExchange ? queueName : string.Empty;

            if (isFanExchange)
            {
                queueName = $"{_serviceName}.{queueName}";

                // Declare exchange
                _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            }

            // Declare queue
            var queue = _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null).QueueName;

            if (isFanExchange)
            {
                // Bind queue to exchange
                _channel.QueueBind(queue, exchangeName, routingKey: "");
            }

            // Create the consumer and assign the messageHandler to the queue.
            var consumer = CreateConsumer<T>(messageHandler);

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        private T ConvertBodyToMessage<T>(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // Convert from JSON to the specified type T
                return JsonConvert.DeserializeObject<T>(message);
            }
            catch (Exception ex)
            {
                // Handle deserialization errors here, e.g., log the exception
                _logger.Equals($"Failed to deserialize message: {ex.Message}");
                throw new ArgumentException($"Failed to deserialize message: {ex.Message}");
            }
        }

        private bool HasSameServiceName(IBasicProperties properties)
        {
            if (properties.Headers != null && properties.Headers.ContainsKey(_headerName))
            {
                // rawHeaderValue is a byte array.
                var rawHeaderValue = properties.Headers[_headerName];
                var headerValue = Encoding.UTF8.GetString((byte[])rawHeaderValue);

                return string.Equals(_serviceName, headerValue, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private EventingBasicConsumer CreateConsumer<T>(Func<T, Task> messageHandler)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                // Convert body to message.
                T messageObject = ConvertBodyToMessage<T>(ea);

                // Check the "ServiceName" header in the message
                if (!HasSameServiceName(ea.BasicProperties))
                {
                    // Pass the deserialized messageObject to the handler.
                    await messageHandler(messageObject);
                }
                else
                {
                    // Log and throw an error when ServiceName doesn't match
                    _logger.LogWarning("Message has the same ServiceName.");
                }
            };

            return consumer;
        }
    }
}