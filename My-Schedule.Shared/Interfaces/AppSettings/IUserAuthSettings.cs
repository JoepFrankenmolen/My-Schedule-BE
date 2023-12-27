namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IUserAuthSettings
    {
        int MaxLoginAttempts { get; }
        string Pepper { get; }
    }
}