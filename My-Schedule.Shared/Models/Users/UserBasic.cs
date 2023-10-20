using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.Shared.Models.Users
{
    public class UserBasic : IUserBasic
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
