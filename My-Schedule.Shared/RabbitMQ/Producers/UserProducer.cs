using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class UserProducer
    {
        private readonly IMessageProducer _messageProducer;

        public UserProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendUserBannedMessage(Guid userId, bool state, long timestamp)
        {
            var message = new UserBannedMessage
            {
                UserId = userId,
                IsBanned = state,
                TokenRevocationTimestamp = timestamp,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserBanned);
        }

        public async Task SendUserBlockedMessage(Guid userId, bool state, long timestamp)
        {
            var message = new UserBlockedMessage
            {
                UserId = userId,
                IsBlocked = state,
                TokenRevocationTimestamp = timestamp,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserBlocked);
        }

        public async Task SendTokenRevovationMessage(Guid userId, long timestamp)
        {
            var message = new UserTokenRevokedMessage
            {
                UserId = userId,
                TokenRevocationTimestamp = timestamp,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserTokenRevocation);
        }

        public async Task SendEmailConfirmationMessage(Guid userId, bool state, long timestamp)
        {
            var message = new UserEmailConfirmationMessage
            {
                UserId = userId,
                IsEmailConfirmed = state,
                TokenRevocationTimestamp = timestamp
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserEmailConfirmation);
        }

        public async Task SendIdentityUpdateMessage(Guid userId, UserIdentityDTO userIdentity)
        {
            var message = new UserIdentityMessage
            {
                UserId = userId,
                UserName = userIdentity.UserName,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserIdentityUpdate);
        }

        public async Task SendRoleUpdateMessage(Guid userId, UserRole role)
        {
            var message = new UserRoleUpdateMessage
            {
                UserId = userId,
                Role = role,
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserRoleUpdate);
        }

        public async Task SendUserCreatedMessage(User user)
        {
            var message = new UserCreatedMessage
            {
                UserId = user.Id,
                CreationTimestamp = user.CreationTimestamp,
                UserName = user.UserName,
                Email = user.Email,
                IsBlocked = user.IsBlocked,
                IsBanned = user.IsBanned,
                IsEmailConfirmed = user.IsEmailConfirmed,
                TokenRevocationTimestamp = user.TokenRevocationTimestamp,
                Roles = user.Roles
            };

            await _messageProducer.SendMessage(message, QueueNames.Users.UserCreated);
        }
    }
}