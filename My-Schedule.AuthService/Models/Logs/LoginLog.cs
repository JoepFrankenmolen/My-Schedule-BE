using My_Schedule.AuthService.Models.ClientDetail;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Logs
{
    public class LoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid ClientDetailsId { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }

        [Required]
        public bool AttemptFailed { get; set; }

        [Required]
        public long TimeStamp { get; set; }
    }
}