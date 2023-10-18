namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IConfirmationSettings
    {
        int ConfirmationExpirationTime { get; }
        int MaxConfirmationAttempts { get; }
    }
}