namespace My_Schedule.AuthService.Services.Notifications
{
    public class NotificationTriggerService
    {
        private readonly NotificationSender _notificationSender;

        public NotificationTriggerService(NotificationSender notificationSender)
        {
            _notificationSender = notificationSender ?? throw new ArgumentNullException(nameof(notificationSender));
        }

        public void SendEmailConfirmation(string recipientEmail, string confirmationCode)
        {
            var subject = "Account Email Verification";
            var body = $"Please use the following verification code to verify your email address: {confirmationCode}";

            _notificationSender.SendNotification(recipientEmail, subject, body);
        }

        public void SendLoginVerification(string recipientEmail, string confirmationCode)
        {
            var subject = "Account Login Confirmation";
            var body = $"Please use the following confirmation code to verify your email address: {confirmationCode}";

            _notificationSender.SendNotification(recipientEmail, subject, body);
        }

        public void SendPasswordReset(string recipientEmail, Guid confirmationId, string confirmationCode)
        {
            var subject = "Account reset password Confirmation";
            var body = $"Please use the following code to verify your password reset: {confirmationCode} and id {confirmationId}";

            _notificationSender.SendNotification(recipientEmail, subject, body);
        }

        /*        public void SendUserLoginVerification(string recipientEmail, string confirmationCode)
                {
                    var subject = "Account Login Verification";
                    var body = $"Please use the following verification code to verify your email address: {confirmationCode}";

                    _notificationSender.SendNotification(recipientEmail, subject, body);
                }*/
    }
}
