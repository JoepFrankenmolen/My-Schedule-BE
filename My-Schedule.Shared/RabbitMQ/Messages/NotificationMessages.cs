using My_Schedule.NotificationService.Models.Enum;

namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public class NotificationTriggeredMessage
    {
        public NotificationType Type;
        public Guid UserId;
        public string EmailSubject;
        public string EmailBody;
    }
}
