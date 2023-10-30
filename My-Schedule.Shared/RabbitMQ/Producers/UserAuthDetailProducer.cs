using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class UserAuthDetailProducer
    {
        private readonly IMessageProducer _messageProducer;

        public UserAuthDetailProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendTwoFactorEnabledMessage(Guid userId, bool state)
        {
            var message = new TwoFactorEnabledMessage
            {
                UserId = userId,
                TwoFactorEnabled = state,
            };

            await _messageProducer.SendMassMessage(message, QueueNames.UserAuthDetails.TwoFactorEnabledUpdate);
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

            await _messageProducer.SendMassMessage(message, QueueNames.UserAuthDetails.SuccessfullLogin);
        }

        public async Task SendFailedLoginAttemptMessage(Guid userId, int failedLoginCount, bool isUserBlocked)
        {
            var message = new FailedLoginAttemptMessage
            {
                UserId = userId,
                FailedLoginAttempts = failedLoginCount,
                IsUserBlocked = isUserBlocked,
            };

            await _messageProducer.SendMassMessage(message, QueueNames.UserAuthDetails.FailedLoginAttempt);
        }

        public async Task SendUserAuthDetailCreatedMessage(UserAuthDetail userAuthDetail)
        {
            var message = new UserAuthDetailCreatedMessage
            {
                UserId = userAuthDetail.UserId,
                User = userAuthDetail.User,
                TwoFactorEnabled = userAuthDetail.TwoFactorEnabled,
                /*                PasswordHash = userAuthDetail.PasswordHash,
                                Salt = userAuthDetail.Salt,*/
                LastLoginTimestamp = userAuthDetail.LastLoginTimestamp,
                FailedLoginAttempts = userAuthDetail.FailedLoginAttempts,
                LoginCount = userAuthDetail.LoginCount,
            };

            await _messageProducer.SendMassMessage(message, QueueNames.UserAuthDetails.UserAuthDetailCreated);
        }
    }
}