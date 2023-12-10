namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserSettings
    {
        bool TwoFactorEnabled { get; set; }
    }
}