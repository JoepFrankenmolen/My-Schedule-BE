namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IMessageQueueSettings
    {
        string MessageQueueHostName { get; }
        int MessageQueuePort { get; }
        string MessageQueueUserName { get; }
        string MessageQueuePassword { get; }
        string MessageQueueVirtualHost { get; }
        bool MessageQueueUseSsl { get; }
    }
}