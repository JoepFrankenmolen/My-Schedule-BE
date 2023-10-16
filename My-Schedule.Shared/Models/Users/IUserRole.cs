namespace My_Schedule.Shared.Models.Users
{
    public interface IUserRole
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        UserRoleType Role { get; set; }
    }
}
