using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Models.Users;

namespace My_Schedule.AuthService.Models.PasswordReset
{
    public class PasswordReset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid ConfirmationId { get; set; }
        public virtual Confirmation Confirmation { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
