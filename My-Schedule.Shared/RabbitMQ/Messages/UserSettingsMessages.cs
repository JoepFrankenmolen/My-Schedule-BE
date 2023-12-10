namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserSettingsMessages
    {
        public Guid UserId;
    }

    public class TwoFactorEnabledMessage : UserMessages
    {
        public bool TwoFactorEnabled;
    }
}