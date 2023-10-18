using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.Users
{
    public class User : IUserDetails, IUserActivity, IUserSecurity, IUserRoles
    {
        // IUserDetails
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public long CreationTimestamp { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // IUserSecurity
        [Required]
        public bool TwoFactorEnabled { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public int FailedLoginAttempts { get; set; }

        // IUserStatus
        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        public long TokenRevocationTimestamp { get; set; }

        // IUserActivity
        [Required]
        public long LastLoginTimestamp { get; set; }

        [Required]
        public int LoginCount { get; set; }

        // IUserRoles
        [Required]
        public List<UserRole> Roles { get; set; }
    }
}