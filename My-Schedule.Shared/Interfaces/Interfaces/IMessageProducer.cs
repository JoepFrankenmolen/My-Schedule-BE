namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessage<T>(T message, string queueName, CancellationToken cancellationToken = default);
    }
}
