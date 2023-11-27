namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserDetails
    {
        long CreationTimestamp { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }
}