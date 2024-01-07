using Microsoft.EntityFrameworkCore;
using My_Schedule.NotificationService.Core;
using My_Schedule.NotificationService.Models;
using My_Schedule.NotificationService.Models.DTO;
using My_Schedule.NotificationService.Models.Enum;

namespace My_Schedule.NotificationService.Services.Notifications
{
    public static class NotificationFetcherService
    {
        /// <summary>
        /// Get a Notification by Id.
        /// This does not require the context
        /// </summary>
        /// <param name="id">The Id of the notification you want to get.</param>
        /// <returns>A notification.</returns>
        /// <exception cref="ArgumentNullException">Thrown when Notification is not found.</exception>
        public static async Task<Notification> GetNotificationById(Guid? id, NotificationServiceContext context)
        {
            var notification = await context.Notifications
                .Include(n => n.Group)
                .FirstOrDefaultAsync(n => n.Id == id);

            return notification ?? throw new ArgumentNullException(nameof(notification));
        }

        /// <summary>
        /// Get notification by type.
        /// </summary>
        /// <param name="type">The type of the notification.</param>
        /// <returns><see cref="RNotification"/></returns>
        public static async Task<Notification> GetNotificationByType(NotificationType type, NotificationServiceContext context)
        {
            var notification = await context.Notifications
                .Include(n => n.Group)
                .FirstOrDefaultAsync(n => n.Type == type);

            return notification ?? throw new ArgumentNullException(nameof(notification));
        }

        /// <summary>
        /// Get all notifications by groupId
        /// </summary>
        /// <param name="groupId">The groupId of all the notifications.</param>
        /// <returns><see cref="Notification"/></returns>
        public static async Task<List<Notification>> GetNotificationsByGroupId(Guid groupId, NotificationServiceContext context)
        {
            var notifications = await context.Notifications
                .Include(n => n.Group)
                .Where(n => n.GroupId == groupId)
                .ToListAsync();

            return notifications ?? throw new ArgumentNullException(nameof(notifications));
        }

        /// <summary>
        /// Get all  notifications async and grouped on grouptype.
        /// </summary>
        /// <param name="start">The start where the list should start begin.</param>
        /// <param name="length">The lenght how long the list should be.</param>
        /// <returns>A list of notifications orderd by groups and the amount of notifications specified in the params. <see cref="FilteredTableNotificationResultDto"/></returns>
        public static async Task<FilteredTableNotificationDto> GetNotificationsFilteredOrderedByGroups(int start, int length, NotificationServiceContext context)
        {
            var notificationTotal = context.Notifications.Count();

            var notifications = await context.Notifications
                .Include(n => n.Group)
                .OrderBy(n => n.GroupId)
                .Skip(start)
                .Take(length)
                .ToListAsync();

            return new FilteredTableNotificationDto { Notifications = ConvertToTableNotificationResultDto(notifications), NotificationTotal = notificationTotal };
        }

        /// <summary>
        /// Get all  notifications async and grouped on grouptype.
        /// </summary>
        /// <param name="start">The start where the list should start begin.</param>
        /// <param name="length">The lenght how long the list should be.</param>
        /// <returns>A list of notifications orderd by groups and the amount of notifications specified in the params. <see cref="FilteredTableNotificationResultDto"/></returns>
        public static async Task<List<Notification>> GetNotificationsOrderedByGroupsEnforcedOff(NotificationServiceContext context)
        {
            var notificationTotal = context.Notifications.Count();

            var notifications = await context.Notifications
                .Include(n => n.Group)
                .OrderBy(n => n.GroupId)
                .Where(n => n.IsEnforced == false)
                .ToListAsync();

            return notifications;
        }

        // maybe change to mapping in the future
        public static List<TableNotificationDto> ConvertToTableNotificationResultDto(List<Notification> notifications)
        {
            return notifications.Select(n => new TableNotificationDto
            {
                Id = n.Id,
                Type = n.Type.ToString(),
                TypeGroupName = n.Group.Name,
                Title = n.Title,
                Description = n.Description,
                EmailPreference = n.EmailPreference,
                IsEnforced = n.IsEnforced,
            }).ToList();
        }
    }
}