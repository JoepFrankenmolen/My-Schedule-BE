using My_Schedule.Shared.Models.Users.UserInterfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.Users
{
    public class UserSettings : IUserSettings
    {
        [Key]
        [ForeignKey("User")]
        [Required]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        // IUserSettings
        [Required]
        public bool TwoFactorEnabled { get; set; }

        /// potential settings:
        /// country
        /// language
        /// timezone
        /// time format
        /// security level (high low medium) currently only high possible
    }
}