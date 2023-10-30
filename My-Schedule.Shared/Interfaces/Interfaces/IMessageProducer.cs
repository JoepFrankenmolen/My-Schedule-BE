namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessage<T>(T message, string queueName);

        Task SendMassMessage<T>(T message, string exchangeName);
    }
}