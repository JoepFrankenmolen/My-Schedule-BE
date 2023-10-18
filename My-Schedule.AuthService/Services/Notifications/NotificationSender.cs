using My_Schedule.Shared.Interfaces.AppSettings;
using System.Net;
using System.Net.Mail;

namespace My_Schedule.AuthService.Services.Notifications
{
    public class NotificationSender
    {
        private readonly IEmailSettings _appSettings;

        public NotificationSender(IEmailSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public void SendNotification(string recipientEmail, string subject, string body)
        {
            // Create the email message
            MailMessage mail = new MailMessage(_appSettings.SenderEmail, recipientEmail);
            mail.Subject = subject;
            mail.Body = body;

            // Create the SMTP client
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential(_appSettings.SenderEmail, _appSettings.SenderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail); // does not work due to the fact this app does not use proper security I think
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}