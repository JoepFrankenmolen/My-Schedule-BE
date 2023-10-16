namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserActivity
    {
        long LastLoginTimestamp { get; set; }
        int LoginCount { get; set; }
    }
}