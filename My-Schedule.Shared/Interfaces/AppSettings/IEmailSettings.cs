namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IEmailSettings
    {
        string SenderEmail { get; }
        string SenderPassword { get; }
    }
}