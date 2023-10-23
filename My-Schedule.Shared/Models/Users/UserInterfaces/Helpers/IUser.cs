using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.Shared.Models.Users
{
    public interface IUser : IUserDetails, IUserStatus, IUserRoles, IEntityWithGuidKey
    {
    }
}