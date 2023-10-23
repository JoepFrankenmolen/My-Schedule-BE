using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Models.Users
{
    public interface IUserAuthDetail : IUserSecurity, IUserActivity, IEntityWithGuidKey
    {
        User User { get; set; }
        Guid UserId { get; set; }
    }
}