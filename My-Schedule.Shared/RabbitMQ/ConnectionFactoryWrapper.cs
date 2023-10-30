using Microsoft.Extensions.Logging;
using My_Schedule.Shared.Interfaces.AppSettings;
using RabbitMQ.Client;

namespace My_Schedule.Shared.RabbitMQ
{
    public class ConnectionFactoryWrapper
    {
        private readonly IMessageQueueSettings _appSettings;
        private readonly ILogger _logger;

        public ConnectionFactoryWrapper(IMessageQueueSettings appSettings, ILogger logger)
        {
            _appSettings = appSettings ??  throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _appSettings.MessageQueueHostName,
                Port = _appSettings.MessageQueuePort,
                UserName = _appSettings.MessageQueueUserName,
                Password = _appSettings.MessageQueuePassword,
                VirtualHost = _appSettings.MessageQueueVirtualHost,
                Ssl = new SslOption
                {
                    Enabled = _appSettings.MessageQueueUseSsl,
                    ServerName = _appSettings.MessageQueueHostName
                }
            };

            try
            {
                return factory.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error connecting to RabbitMQ.");
                throw new Exception("Error connecting to RabbitMQ.", ex);
            }
        }
    }
}
