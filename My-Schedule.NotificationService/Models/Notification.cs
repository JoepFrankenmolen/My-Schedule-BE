using My_Schedule.NotificationService.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.NotificationService.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid GroupId { get; set; }

        public NotificationGroup Group { get; set; }

        // The default setting of this notification.
        [Required]
        public bool EmailPreference { get; set; }

        // If enforced then users can't change the user preferences or see it in the user preference menu.
        [Required]
        public bool IsEnforced { get; set; }
    }
}