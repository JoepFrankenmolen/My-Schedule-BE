using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Interfaces
{
    public interface IUserAuthenticationContext
    {
        public void Install(HttpContext context, IUserDetails user);

        Guid UserId { get; }
        string Email { get; }
        string UserIp { get; }
        string UserAgent { get; }
        List<IUserRole> Roles { get; }
        bool IsAdmin { get; }
        bool IsMasterAdmin { get; }
    }
}