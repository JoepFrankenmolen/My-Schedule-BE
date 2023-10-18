namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IUserSettings
    {
        int MaxLoginAttempts { get; }
        string Pepper { get; }
    }
}