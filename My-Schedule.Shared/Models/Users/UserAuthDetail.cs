using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.Users
{
    /// <summary>
    /// This object resembles fields the authentication system needs.
    /// This should never be used if not needed.
    /// </summary>
    public class UserAuthDetail : IUserAuthDetail
    {
        // IUserDetails
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        // IUserSecurity
        [Required]
        public bool TwoFactorEnabled { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public int FailedLoginAttempts { get; set; }

        // IUserActivity
        [Required]
        public long LastLoginTimestamp { get; set; }

        [Required]
        public int LoginCount { get; set; }
    }
}