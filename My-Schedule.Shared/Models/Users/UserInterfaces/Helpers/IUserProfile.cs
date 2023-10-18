namespace My_Schedule.Shared.Models.Users.UserInterfaces.Helpers
{
    public interface IUserProfile : IEntityWithGuidKey, IUserDetails, IUserStatus, IUserRoles
    {
    }
}