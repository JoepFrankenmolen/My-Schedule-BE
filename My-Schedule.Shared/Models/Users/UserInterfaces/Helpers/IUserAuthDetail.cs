using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Models.Users
{
    public interface IUserAuthDetail : IUserSecurity, IUserActivity
    {
        User User { get; set; }
        Guid UserId { get; set; }
    }
}