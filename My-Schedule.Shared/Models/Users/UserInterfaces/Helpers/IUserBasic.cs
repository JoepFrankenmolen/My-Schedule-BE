namespace My_Schedule.Shared.Models.Users.UserInterfaces.Helpers
{
    public interface IUserBasic : IUserStatus, IUserRoles
    {
        Guid Id { get; set; }
    }
}