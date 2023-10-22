namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class TokenMessages
    {
        public Guid TokenId;
    }

    public class TokenStatusCreateMessage : TokenMessages
    {
        public Guid SessionId;
        public bool IsBlocked;
        public long BlockedTimestamp;
    }
}
