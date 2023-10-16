namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserSecurity : IUserStatus
    {
        bool TwoFactorEnabled { get; set; }
        string PasswordHash { get; set; }
        string Salt { get; set; }
        int FailedLoginAttempts { get; } // Number of failed login attempts. Resets after successfull login.
    }
}