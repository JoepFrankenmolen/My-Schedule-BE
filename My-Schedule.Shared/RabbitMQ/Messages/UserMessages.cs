namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserMessages
    {
        public Guid UserId;
    }

    public class UserBLockedMessage : UserMessages
    {
        public bool IsBlocked;
    }

    public class UserBannedMessage : UserMessages
    {
        public bool IsBanned;
    }
}
