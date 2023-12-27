using My_Schedule.Shared.Models.Users.UserInterfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.Users
{
    /// <summary>
    /// This object resembles fields the authentication system needs.
    /// This should never be used if not needed.
    /// </summary>
    public class UserSecurity : IUserSecurity
    {
        [Key]
        [ForeignKey("User")]
        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        // IUserSecurity
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}