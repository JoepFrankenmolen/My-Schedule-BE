using My_Schedule.NotificationService.Core;
using My_Schedule.NotificationService.Models;
using My_Schedule.NotificationService.Models.DTO;

namespace My_Schedule.NotificationService.Services.Notifications
{
    public class NotificationUpdateService
    {
        private readonly NotificationServiceContext _dbContext;

        public NotificationUpdateService(NotificationServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Update all the settings of a notification at once.
        /// </summary>
        /// <param name="id">The notificationId.</param>
        /// <param name="notification">The object that must contains all the current/new values: Default, Enforced, Public, ThresholdValue.</param>
        /// <returns>The new notification.</returns>
        public async Task<Notification> UpdateNotification(Guid id, UpdateNotificationDTO notificationDTO)
        {
            // validation of the fields
            if (!ValidateUpdateParameters(notificationDTO))
            {
                throw new ArgumentException("Invalid parameters!");
            }

            var notification = await NotificationFetcherService.GetNotificationById(id, _dbContext);

            await _dbContext.SaveChangesAsync();

            return notification;
        }

        /// <summary>
        /// Update the email preference of the settings of a notification.
        /// </summary>
        /// <param name="id">The notificationId.</param>
        /// <param name="state">True or false.</param>
        /// <returns>The new notification.</returns>
        public async Task<Notification> UpdateNotificationEmailPreference(Guid id, bool state)
        {
            var notification = await NotificationFetcherService.GetNotificationById(id, _dbContext);

            notification.EmailPreference = state;

            await _dbContext.SaveChangesAsync();

            return notification;
        }

        /// <summary>
        /// Update the enforced of the settings of a notification.
        /// </summary>
        /// <param name="id">The notificationId.</param>
        /// <param name="state">True or false.</param>
        /// <returns>The new notification.</returns>
        public async Task<Notification> UpdateNotificationEnforced(Guid id, bool state)
        {
            var notification = await NotificationFetcherService.GetNotificationById(id, _dbContext);

            notification.IsEnforced = state;

            await _dbContext.SaveChangesAsync();

            return notification;
        }

        // maybe good to have it return a bool and handle it like that
        private bool ValidateUpdateParameters(UpdateNotificationDTO notification)
        {
            // validation of the fields
            if (notification.Title.Count() >= 255 || notification.Description.Count() >= 255)
            {
                return false;
            }

            return true;
        }
    }
}