namespace My_Schedule.Shared.Models.Users.UserInterfaces.Helpers
{
    public interface IUserBasic : IEntityWithGuidKey, IUserDetails, IUserStatus, IUserRoles
    {
    }
}