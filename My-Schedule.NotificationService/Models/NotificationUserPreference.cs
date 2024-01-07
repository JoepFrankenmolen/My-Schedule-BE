using My_Schedule.Shared.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.NotificationService.Models
{
    public class NotificationUserPreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid NotificationId { get; set; }

        public virtual Notification Notification { get; set; }

        [Required]
        public bool EmailPreference { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}