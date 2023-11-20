using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using System.ComponentModel.DataAnnotations;

namespace My_Schedule.Shared.DTO.Users
{
    public class UserDTO : IUserPublic
    {
        public Guid Id { get; set; }

        // IUserDetails
        public long CreationTimestamp { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // IUserRoles
        public List<UserRole> Roles { get; set; }

        public static UserDTO MapUserToDTO(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                CreationTimestamp = user.CreationTimestamp,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles
            };
        }
    }
}