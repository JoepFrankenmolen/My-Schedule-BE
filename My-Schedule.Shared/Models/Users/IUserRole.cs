namespace My_Schedule.Shared.Models.Users
{
    public interface IUserRole: IEntityWithGuidKey
    {
        Guid UserId { get; set; }
        UserRoleType Role { get; set; }
    }
}
