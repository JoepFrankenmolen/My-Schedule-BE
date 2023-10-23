namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IMessageConsumer : IDisposable
    {
        void StartConsuming<T>(Func<T, Task> messageHandler, string queueName);
    }
}