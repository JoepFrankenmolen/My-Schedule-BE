namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessage<T>(T message, string queueName);

        Task SendFanMessage<T>(T message, string exchangeName);
    }
}