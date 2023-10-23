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

        public MessageConsumer(IMessageQueueSettings appSettings)
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

        public void StartConsuming<T>(Func<T, Task> messageHandler, string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body); // Use UTF-8 encoding for decoding

                T messageObject = default(T); // Initialize messageObject with a default value

                try
                {
                    // Convert from JSON to the specified type T
                    messageObject = JsonConvert.DeserializeObject<T>(message);
                }
                catch (Exception ex)
                {
                    // Handle deserialization errors here, e.g., log the exception
                    Console.WriteLine($"Failed to deserialize message: {ex.Message}");
                }

                messageHandler(messageObject); // Pass the deserialized messageObject to the handler
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}