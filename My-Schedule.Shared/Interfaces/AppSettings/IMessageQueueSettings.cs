namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IMessageQueueSettings
    {
        string MessageQueueHostName { get; }
        int MessageQueuePort { get; set; }
        string MessageQueueUserName { get; set; }
        string MessageQueuePassword { get; set; }
        string MessageQueueVirtualHost { get; set; }
        bool MessageQueueUseSsl { get; set; }
    }
}