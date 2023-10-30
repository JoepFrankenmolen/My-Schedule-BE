using My_Schedule.Shared.Interfaces.AppSettings;
using My_Schedule.Shared.Interfaces.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace My_Schedule.Shared.RabbitMQ
{
    public class MessageProducer : IMessageProducer, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<MessageConsumer> _logger;
        private readonly Dictionary<string, object> _headers;

        public MessageProducer(IMessageQueueSettings appSettings, ILogger<MessageConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connection = new ConnectionFactoryWrapper(appSettings, _logger).CreateConnection();
            _channel = _connection.CreateModel();

            // Add headers so that when picked up by a consumer it can check if it has already processed this message.
            _headers = CreateHeaders(appSettings);
        }

        /// <summary>
        /// Sends a single message to a single queue.
        /// </summary>
        /// <typeparam name="T">The Type of the message.</typeparam>
        /// <param name="message">The message itself.</param>
        /// <param name="queueName">The queue to sent to.</param>
        /// <returns></returns>
        public async Task SendMessage<T>(T message, string queueName)
        {
            // Declare queue
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            try
            {
                // Convert to JSON
                var body = ConvertMessageToBody(message);

                // Add headers
                var properties = CreateProperties();

                // Send queue message
                _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);

                _logger.LogInformation($"Message sent to {queueName}: {message.GetType().Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message to {queueName}: {message.GetType().Name}");
            }
        }

        /// <summary>
        /// Sends a fan out message. This means one message to multible queues.
        /// </summary>
        /// <typeparam name="T">The message Type.</typeparam>
        /// <param name="message">The message itself.</param>
        /// <param name="exchangeName">The queue name.</param>
        /// <returns></returns>
        public async Task SendFanMessage<T>(T message, string exchangeName)
        {
            try
            {
                // Convert to JSON
                var body = ConvertMessageToBody(message);

                // Add Headers
                var properties = CreateProperties();

                // Send Exchange message.
                _channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: properties, body: body);

                _logger.LogInformation($"Message sent to fanout exchange {exchangeName}: {message.GetType().Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message to fanout exchange {exchangeName}: {message.GetType().Name}");
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        private byte[] ConvertMessageToBody<T>(T message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(jsonMessage);
        }

        private IBasicProperties CreateProperties()
        {
            var properties = _channel.CreateBasicProperties();
            if (_headers != null)
            {
                properties.Headers = new Dictionary<string, object>(_headers);
            }
            return properties;
        }

        private Dictionary<string, object> CreateHeaders(IMessageQueueSettings appSettings)
        {
            return new Dictionary<string, object>
            {
                { appSettings.MessageQueueHeaderName, appSettings.MessageQueueServiceName },
            };
        }
    }
}