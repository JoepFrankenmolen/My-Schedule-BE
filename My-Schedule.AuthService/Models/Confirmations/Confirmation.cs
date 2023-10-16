using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Confirmations
{
    public class Confirmation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public long CreationTimestamp { get; set; }

        [Required]
        public long ExpirationTimestamp { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public ConfirmationType ConfirmationType { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public int Attempts { get; set; }
    }
}