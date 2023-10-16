namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IUserSettings
    {
        int MaxLoginAttempts { get; }
        int MaxConfirmationAttempts { get; }
        string Pepper { get; }
    }
}