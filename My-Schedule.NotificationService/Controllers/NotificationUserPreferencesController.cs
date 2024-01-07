using Microsoft.AspNetCore.Mvc;
using My_Schedule.NotificationService.Services.Users;
using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.User, UserRoleType.Admin)]
    public class NotificationUserPreferencesController : ControllerBase
    {
        private NotificationUserPreferencesService _notificationUserPreferenceService;

        public NotificationUserPreferencesController(NotificationUserPreferencesService notificationUserPreferenceService)
        {
            _notificationUserPreferenceService = notificationUserPreferenceService ?? throw new ArgumentNullException(nameof(notificationUserPreferenceService));
        }

        /// <summary>
        /// Get all notifications with the logged in user, their EmailPreferences, injected into the notification EmailPreferences.
        /// </summary>
        /// <returns> A list of notifications with injected EmailPreferences. </returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetNotificationUserPreferencesOrderedByGroups()
        {
            try
            {
                var data = await _notificationUserPreferenceService.GetNotificationsUserPreferencesOrderedByGroups();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Save a user EmailPreference of a notification.
        /// </summary>
        /// <param name="notificationId"> The notification Id. </param>
        /// <param name="emailPreference"> The new user EmailPreference. </param>
        /// <returns> nothing </returns>
        [HttpPost]
        [Route("{notificationId}/{emailPreference}")]
        public async Task<IActionResult> UpdateNotificationUserPreference(Guid notificationId, bool emailPreference)
        {
            try
            {
                await _notificationUserPreferenceService.UpdateNotificationUserPreference(notificationId, emailPreference);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Enable or disable all EmailPreferences on notifications.
        /// </summary>
        /// <param name="emailPreference"> If you want them all on or off. </param>
        /// <returns> A new list of notifications with EmailPreferences injected into the notifications. </returns>
        [HttpPost]
        [Route("all/{emailPreference}")]
        public async Task<IActionResult> UpdateAllNotificationUserPreferences(bool emailPreference)
        {
            try
            {
                await _notificationUserPreferenceService.UpdateAllNotificationUserPreferences(emailPreference);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}