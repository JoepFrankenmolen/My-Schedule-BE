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

        public MessageConsumer(IMessageQueueSettings appSettings, ILogger<MessageConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentException(nameof(appSettings));
            _connection = new ConnectionFactoryWrapper(appSettings, _logger).CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartConsuming<T>(Func<T, Task> messageHandler, string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                // Convert body to message.
                T messageObject = ConvertBodyToMessage<T>(ea);

                // Check the "ServiceName" header in the message
                if (!HasSameServiceName(ea.BasicProperties))
                {
                    // Pass the deserialized messageObject to the handler.
                    messageHandler(messageObject);
                }
                else
                {
                    // Log and throw an error when ServiceName doesn't match
                    _logger.LogError("Message has the same ServiceName.");
                }

                // Pass the deserialized messageObject to the handler.
                messageHandler(messageObject); 
            };

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
            var message = Encoding.UTF8.GetString(body); // Use UTF-8 encoding for decoding

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
            var headerName = _appSettings.MessageQueueHeaderName;
            var serviceName = _appSettings.MessageQueueServiceName;

            if (properties.Headers != null && properties.Headers.ContainsKey(headerName))
            {
                var headerValue = properties.Headers[headerName].ToString();
                return string.Equals(serviceName, serviceName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}