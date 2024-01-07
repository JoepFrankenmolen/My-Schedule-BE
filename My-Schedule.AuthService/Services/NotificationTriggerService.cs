using My_Schedule.NotificationService.Models.Enum;
using My_Schedule.Shared.RabbitMQ.Producers;

namespace My_Schedule.AuthService.Services
{
    public class NotificationTriggerService
    {
        private readonly NotificationProducer _notificationProducer;

        public NotificationTriggerService(NotificationProducer notificationProducer)
        {
            _notificationProducer = notificationProducer ?? throw new ArgumentNullException(nameof(notificationProducer));
        }

        public async void SendEmailConfirmation(Guid userId, string confirmationCode)
        {
            var subject = "Account Email Verification";
            var body = $"Please use the following verification code to verify your email address: {confirmationCode}";

            await _notificationProducer.SendNotificationTriggeredMessage(NotificationType.EmailConfirmation, userId, subject, body);
        }

        public async void SendLoginVerification(Guid userId, string confirmationCode)
        {
            var subject = "Account Login Confirmation";
            var body = $"Please use the following confirmation code to verify your email address: {confirmationCode}";

            await _notificationProducer.SendNotificationTriggeredMessage(NotificationType.LoginVerification, userId, subject, body);
        }

        public async void SendPasswordReset(Guid userId, Guid confirmationId, string confirmationCode)
        {
            var subject = "Account reset password Confirmation";
            var body = $"Please use the following code to verify your password reset: {confirmationCode} and id {confirmationId}";

            await _notificationProducer.SendNotificationTriggeredMessage(NotificationType.PasswordReset, userId, subject, body);
        }
    }
}