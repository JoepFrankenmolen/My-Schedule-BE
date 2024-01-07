using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.NotificationService.Models
{
    public class NotificationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NotificationType { get; set; }

        // The email of the user(due to the user not always existing the email is stored to make sure we still know who it is send to).
        [Required]
        public string Email { get; set; }

        // The HTTP response code(202 means succes).
        [Required]
        public int StatusCode { get; set; }

        [Required]
        public long Timestamp { get; set; }
    }
}