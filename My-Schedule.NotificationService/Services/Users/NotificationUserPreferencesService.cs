using Microsoft.EntityFrameworkCore;
using My_Schedule.NotificationService.Core;
using My_Schedule.NotificationService.Models;
using My_Schedule.NotificationService.Models.DTO;
using My_Schedule.NotificationService.Services.Notifications;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Services.Users.Users;

namespace My_Schedule.NotificationService.Services.Users
{
    public class NotificationUserPreferencesService
    {
        private readonly NotificationServiceContext _dbContext;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public NotificationUserPreferencesService(
            NotificationServiceContext dbContext,
            IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(userAuthenticationContext));
        }

        /// <summary>
        /// Generates a list of notifications with their EmailPreferences replaced by UserPreferences.
        /// </summary>
        /// <returns>Returns a list of notifications with their EmailPreferences replaced by the UserPreferences.</returns>
        public async Task<List<NotificationUserPreferenceDTO>> GetNotificationsUserPreferencesOrderedByGroups()
        {
            var user = await UserFetcherService.GetUserById(_userAuthenticationContext.UserId, _dbContext);

            // get all notifications that are not enforced.
            var notifications = await NotificationFetcherService.GetNotificationsOrderedByGroupsEnforcedOff(_dbContext);

            // Get user preferences.
            var userPreferences = await GetNotificationUserPreferences(notifications, user.Id);

            // Inject the user preference in the notification using a select loop and return the correct DTO.
            var notificationUserPreferences = notifications.Select(n =>
            {
                var notificationUserPreferencesById = userPreferences.FirstOrDefault(s => s.NotificationId == n.Id);

                if (notificationUserPreferencesById != null)
                {
                    n.EmailPreference = notificationUserPreferencesById.EmailPreference;
                }

                return NotificationUserPreferenceDTO.ConvertFromNotification(n);
            }).ToList();

            return notificationUserPreferences;
        }

        /// <summary>
        /// Update the UserPreference for a specific notification.
        /// </summary>
        /// <param name="notificationId">The Id of the notification where you want to update the UserPreference.</param>
        /// <param name="emailPreference">True or False depending on what you want the new UserPreference to be.</param>
        public async Task UpdateNotificationUserPreference(Guid notificationId, bool emailPreference)
        {
            // fetching the notifications
            var notification = await NotificationFetcherService.GetNotificationById(notificationId, _dbContext);
            var notifications = new List<Notification> { notification };

            await UpdateListOfNotifications(notifications, emailPreference);
        }

        /// <summary>
        /// Update all the UserPreferences for all notifications to the given value.
        /// </summary>
        /// <param name="emailPreference">Update all to True or False.</param>
        /// <returns>The list of all notifications with UserPreferences.</returns>
        /// <exception cref="NoTenantException">Thrown when no Tenant is found.</exception>
        /// <exception cref="UserNotFoundException">Thrown when user not found.</exception>
        public async Task UpdateAllNotificationUserPreferences(bool emailPreference)
        {
            // get all notifications that are not enforced.
            var notifications = await NotificationFetcherService.GetNotificationsOrderedByGroupsEnforcedOff(_dbContext);

            await UpdateListOfNotifications(notifications, emailPreference);
        }

        // Updates the preference on a list of notifications.
        private async Task UpdateListOfNotifications(List<Notification> notifications, bool emailPreference)
        {
            var user = await UserFetcherService.GetUserById(_userAuthenticationContext.UserId, _dbContext);

            var notificationUserPreferences = await GetNotificationUserPreferences(notifications, user.Id);

            foreach (var notification in notifications)
            {
                if (!notification.IsEnforced)
                {
                    var notificationUserPreference = notificationUserPreferences.FirstOrDefault(n => n.NotificationId == notification.Id);
                    UpdateUserPreference(notification, notificationUserPreference, emailPreference, user.Id);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        // Creates or updates a preference depending on what is given.
        private void UpdateUserPreference(Notification notification, NotificationUserPreference notificationUserPreference, bool emailPreference, Guid userId)
        {
            // this check is here for safety.
            if (!notification.IsEnforced)
            {
                // checks if a preference already exists if not create a new one
                if (notificationUserPreference == null)
                {
                    notificationUserPreference = new NotificationUserPreference
                    {
                        NotificationId = notification.Id,
                        EmailPreference = emailPreference,
                        UserId = userId,
                    };

                    _dbContext.NotificationUserPreferences.Add(notificationUserPreference);
                }
                else
                {
                    notificationUserPreference.EmailPreference = emailPreference;

                    _dbContext.NotificationUserPreferences.Update(notificationUserPreference);
                }
            }
        }

        // Gets all valid UserPreferences from a list of notifications.
        private async Task<List<NotificationUserPreference>> GetNotificationUserPreferences(List<Notification> notifications, Guid userId)
        {
            var notificationIds = notifications.Select(n => n.Id).ToList();

            return await _dbContext.NotificationUserPreferences
                .Where(n => n.UserId == userId && notificationIds.Contains(n.NotificationId)).ToListAsync();
        }
    }
}