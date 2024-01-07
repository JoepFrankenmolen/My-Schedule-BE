namespace My_Schedule.NotificationService.Models.DTO
{
    public class NotificationUserPreferenceDTO
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public NotificationGroup Group { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool EmailPreference { get; set; }

        public static NotificationUserPreferenceDTO ConvertFromNotification(Notification notification)
        {
            return new NotificationUserPreferenceDTO
            {
                Id = notification.Id,
                Type = notification.Type.ToString(),
                Group = notification.Group,
                Title = notification.Title,
                Description = notification.Description,
                EmailPreference = notification.EmailPreference
            };
        }
    }
}