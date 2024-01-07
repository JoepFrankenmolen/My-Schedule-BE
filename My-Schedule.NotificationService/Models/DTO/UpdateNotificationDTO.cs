namespace My_Schedule.NotificationService.Models.DTO
{
    public class UpdateNotificationDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool EmailPreference { get; set; }

        public bool IsEnforced { get; set; }
    }
}