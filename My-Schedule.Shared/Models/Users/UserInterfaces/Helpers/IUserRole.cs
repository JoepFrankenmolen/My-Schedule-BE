namespace My_Schedule.Shared.Models.Users.UserInterfaces.Helpers
{
    public interface IUserRole : IEntityWithGuidKey
    {
        UserRoleType Role { get; set; }
        Guid UserId { get; set; }
    }
}