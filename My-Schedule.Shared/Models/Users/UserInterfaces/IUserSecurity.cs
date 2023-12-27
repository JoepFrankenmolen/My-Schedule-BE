namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserSecurity
    {
        string PasswordHash { get; set; }
        string Salt { get; set; }
    }
}