using My_Schedule.Shared.Models.ClientDetails;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Logs
{
    public class ConfirmationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid confirmationId { get; set; }

        [Required]
        public Guid ClientDetailsId { get; set; }

        public virtual ClientDetails ClientDetails { get; set; }

        [Required]
        public bool AttemptFailed { get; set; }

        [Required]
        public long TimeStamp { get; set; }
    }
}