namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserDetails
    {
        long CreationTimeStamp { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}