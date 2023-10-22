using Microsoft.AspNetCore.Connections;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;
using System.Text;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class UserProducer
    {
        private readonly IMessageProducer _messageProducer;

        public UserProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendUserBannedMessage(Guid userId, bool state)
        {
            var message = new UserBannedMessage
            {
                UserId = userId,
                IsBanned = state,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserBanned);
        }

        public async Task SendEmailConfirmedMessage(Guid userId, bool state)
        {
            var message = new UserBannedMessage
            {
                UserId = userId,
                IsBanned = state,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserBanned);
        }

        public async Task SendUserBlockedMessage(Guid userId, bool state)
        {
            var message = new UserBLockedMessage
            {
                UserId = userId,
                IsBlocked = state,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserBlocked);
        }
    }
}
