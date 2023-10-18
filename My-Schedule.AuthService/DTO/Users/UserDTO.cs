using My_Schedule.AuthService.Models.Users;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using System.ComponentModel.DataAnnotations;

namespace My_Schedule.AuthService.DTO.Users
{
    public class UserDTO : IUserPublic
    {
        // IUserDetails
        public Guid Id { get; set; }

        public long CreationTimestamp { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public long LastLoggedInTimeStamp { get; set; }

        public List<IUserRole> Roles { get; set; }
    }
}