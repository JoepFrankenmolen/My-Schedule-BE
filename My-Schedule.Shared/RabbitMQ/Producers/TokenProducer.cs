using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class TokenProducer
    {
        private readonly IMessageProducer _messageProducer;

        public TokenProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendTokenStatusCreatedMessage(ITokenStatus tokenStatus)
        {
            var message = new TokenStatusCreateMessage
            {
                TokenId = tokenStatus.Id,
                SessionId = tokenStatus.SessionId,
                IsBlocked = tokenStatus.IsBlocked,
                BlockedTimestamp = tokenStatus.BlockedTimestamp.Value,
            };

            await _messageProducer.SendMessage(message, QueueNames.Tokens.TokenStatusCreated);
        }
    }
}
