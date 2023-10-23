using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.Users
{
    public class User : IUser
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

        // IUserStatus
        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        public long TokenRevocationTimestamp { get; set; }

        // IUserRoles
        [Required]
        public List<UserRole> Roles { get; set; }
    }
}