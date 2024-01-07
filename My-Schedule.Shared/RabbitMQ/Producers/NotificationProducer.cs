using My_Schedule.NotificationService.Models.Enum;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class NotificationProducer
    {
        private readonly IMessageProducer _messageProducer;

        public NotificationProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendNotificationTriggeredMessage(NotificationType type, Guid userId, string emailSubject, string emailBody)
        {
            var message = new NotificationTriggeredMessage
            {
                Type = type,
                UserId = userId,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            await _messageProducer.SendMessage(message, QueueNames.Notifications.Notificationtriggered);
        }
    }
}
