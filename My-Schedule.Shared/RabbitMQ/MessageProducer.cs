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

        public MessageProducer(IMessageQueueSettings appSettings)
        {
            _ = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

            var factory = new ConnectionFactory
            {
                HostName = appSettings.MessageQueueHostName, // RabbitMQ server host name
                Port = appSettings.MessageQueuePort,         // Port number (usually 5672)
                UserName = appSettings.MessageQueueUserName, // RabbitMQ username
                Password = appSettings.MessageQueuePassword, // RabbitMQ password
                VirtualHost = appSettings.MessageQueueVirtualHost, // RabbitMQ virtual host (if used)

                // Enable SSL/TLS for secure communication (optional)
                Ssl = new SslOption
                {
                    Enabled = appSettings.MessageQueueUseSsl, // Set to true if you want to use SSL/TLS
                    ServerName = appSettings.MessageQueueHostName // Server name for SSL certificate validation
                }
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task SendMessage<T>(T message, string queueName, CancellationToken cancellationToken = default)
        {
            // Declare a queue
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            try
            {
                var jsonMessage = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(jsonMessage);

                _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}