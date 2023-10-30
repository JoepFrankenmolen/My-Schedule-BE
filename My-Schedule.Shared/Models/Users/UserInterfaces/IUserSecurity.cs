namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserSecurity
    {
        bool TwoFactorEnabled { get; set; } // should be moved to IUserSettings
        string PasswordHash { get; set; }
        string Salt { get; set; }
    }
}