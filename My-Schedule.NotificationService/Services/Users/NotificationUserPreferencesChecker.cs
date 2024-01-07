using Microsoft.EntityFrameworkCore;
using My_Schedule.NotificationService.Core;
using My_Schedule.NotificationService.Models;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.NotificationService.Services.Users
{
    public static class NotificationUserPreferencesChecker
    {
        /// <summary>
        /// Validate if a user is allowed to recieve the notification.
        /// </summary>
        /// <param name="notification">The notification that got triggered.</param>
        /// <param name="userRecipient">The user that is supposed to get the notification.</param>
        /// <param name="tenantId">The tennant in which the notification got triggered. Should preferable not be null.</param>
        /// <returns>A bool if the user is allowed to recieve the notification or not.</returns>
        public static async Task<bool> ShouldUserReceiveNotification(Notification notification, User userRecipient, NotificationServiceContext _dbContext)
        {
            // Phisicly can't sent notification without any of these two.
            if (userRecipient == null || notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification.IsEnforced)
            {
                return notification.EmailPreference;
            }

            // If this happens than the user should recieve the notification. This is because the user does not exist in the db.
            if (userRecipient.Id == Guid.Empty)
            {
                return true;
            }

            // Fetch the userSetting based on the tenant, user recipeint and the notificationId.
            var userSetting = await _dbContext.NotificationUserPreferences
                                    .Where(s => s.UserId == userRecipient.Id && s.NotificationId == notification.Id)
                                    .FirstOrDefaultAsync();

            // If userSetting exists than resturn the EmailPreference of the userSetting. If it does not return the default EmailPreference.
            return userSetting != null ? userSetting.EmailPreference : notification.EmailPreference;
        }
    }
}