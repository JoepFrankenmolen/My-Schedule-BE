using My_Schedule.NotificationService.Services;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.NotificationService.RabbitMQ
{
    public class NotificationConsumer : IHostedService
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly NotificationDispatcher _notificationDispatcher;
        private readonly ILogger<NotificationConsumer> _logger;

        public NotificationConsumer(
            IMessageConsumer messageConsumer,
            NotificationDispatcher notificationDispatcher,
            ILogger<NotificationConsumer> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _notificationDispatcher = notificationDispatcher ?? throw new ArgumentNullException(nameof(notificationDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<NotificationTriggeredMessage>(ProcessNotificationTriggered, QueueNames.Notifications.Notificationtriggered);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessNotificationTriggered(NotificationTriggeredMessage message)
        {
            try
            {
                await _notificationDispatcher.SendEmailNotification(message.Type, message.UserId, message.EmailSubject, message.EmailBody);

                _logger.LogInformation($"Notification Triggered Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Notification Triggered Message");
            }
        }
    }
}