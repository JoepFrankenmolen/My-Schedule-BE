namespace My_Schedule.Shared.RabbitMQ
{
    public interface IMessageProducer
    {
        Task SendMessage<T>(T message, string queueName, CancellationToken cancellationToken = default);
    }
}
