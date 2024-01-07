using My_Schedule.NotificationService.Core;
using My_Schedule.NotificationService.Models.Enum;
using My_Schedule.NotificationService.Services.Notifications;
using My_Schedule.NotificationService.Services.Users;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Services.Users.Users;

namespace My_Schedule.NotificationService.Services
{
    public class NotificationDispatcher
    {
        private readonly IDefaultContextBuilder _dbContextBuilder;
        private readonly NotificationSender _sender;

        public NotificationDispatcher(IDefaultContextBuilder dbContextBuilder, NotificationSender sender)
        {
            _dbContextBuilder = dbContextBuilder ?? throw new ArgumentNullException(nameof(dbContextBuilder));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public async Task SendEmailNotification(NotificationType notificationType, Guid userId, string emailSubject, string emailBody)
        {
            using (var context = _dbContextBuilder.CreateContext<NotificationServiceContext>())
            {
                var notification = await NotificationFetcherService.GetNotificationByType(notificationType, context);
                var user = await UserFetcherService.GetUserById(userId, context);

                if (await NotificationUserPreferencesChecker.ShouldUserReceiveNotification(notification, user, context))
                {
                    _sender.SendNotification(user.Email, emailSubject, emailBody);
                }
            }
        }
    }
}