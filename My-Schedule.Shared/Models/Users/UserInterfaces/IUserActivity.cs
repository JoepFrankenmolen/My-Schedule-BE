namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserActivity
    {
        int FailedLoginAttempts { get; } // Number of failed login attempts. Resets after successfull login.
        long LastLoginTimestamp { get; set; }
        int LoginCount { get; set; }
    }
}