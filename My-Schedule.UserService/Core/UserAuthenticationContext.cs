using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.UserService.Core
{
    public class UserAuthenticationContext : IUserAuthenticationContext
    {
        public void Install(HttpContext context, IUserBasic user)
        {
            UserId = user.Id;
            UserIp = context.Connection.RemoteIpAddress?.ToString();
            UserAgent = context.Request.Headers["User-Agent"].ToString();
            Roles = user.Roles;
            IsAdmin = CheckIfUserHasRole(user, UserRoleType.Admin);
            IsMasterAdmin = CheckIfUserHasRole(user, UserRoleType.MasterAdmin);
        }

        private bool CheckIfUserHasRole(IUserRoles user, UserRoleType type)
        {
            if (user.Roles.Any(r => r.Role == type))
            {
                return true;
            }
            return false;
        }

        public Guid UserId { get; internal set; }
        public string UserIp { get; internal set; }
        public string UserAgent { get; internal set; }
        public List<UserRole> Roles { get; internal set; }
        public bool IsAdmin { get; internal set; }
        public bool IsMasterAdmin { get; internal set; }
    }
}