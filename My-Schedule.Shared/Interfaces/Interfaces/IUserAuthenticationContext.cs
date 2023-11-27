using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IUserAuthenticationContext
    {
        public void Install(HttpContext context, IUser user);

        Guid UserId { get; }
        string UserIp { get; }
        string UserAgent { get; }
        List<UserRole> Roles { get; }
        bool IsAdmin { get; }
        bool IsMasterAdmin { get; }
    }
}