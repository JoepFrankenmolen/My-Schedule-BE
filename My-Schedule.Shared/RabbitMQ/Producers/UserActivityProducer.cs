using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class UserActivityProducer
    {
        private readonly IMessageProducer _messageProducer;

        public UserActivityProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendSuccessfullLoginMessage(Guid userId, long lastLoginTimestamp, int loginCount, int failedLoginAttempts)
        {
            var message = new SuccessfullLoginMessage
            {
                UserId = userId,
                LastLoginTimestamp = lastLoginTimestamp,
                LoginCount = loginCount,
                FailedLoginAttempts = failedLoginAttempts
            };

            await _messageProducer.SendFanMessage(message, QueueNames.UserActivity.SuccessfullLogin);
        }

        public async Task SendFailedLoginAttemptMessage(Guid userId, int failedLoginCount, bool isUserBlocked)
        {
            var message = new FailedLoginAttemptMessage
            {
                UserId = userId,
                FailedLoginAttempts = failedLoginCount,
                IsUserBlocked = isUserBlocked,
            };

            await _messageProducer.SendFanMessage(message, QueueNames.UserActivity.FailedLoginAttempt);
        }
    }
}