namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserDetails
    {
        Guid Id { get; set; }
        long CreationTimeStamp { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}