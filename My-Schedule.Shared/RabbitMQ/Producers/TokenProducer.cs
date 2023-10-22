using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class TokenProducer
    {
        private readonly IMessageProducer _messageProducer;

        public TokenProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendTokenStatusCreatedMessage(Guid tokenId, Guid sessionId, bool isBlocked, long blockedTimestamp)
        {
            var message = new TokenStatusCreateMessage
            {
                TokenId = tokenId,
                SessionId = sessionId,
                IsBlocked = isBlocked,
                BlockedTimestamp = blockedTimestamp,
            };

            await _messageProducer.SendMessage(message, QueueNames.Tokens.TokenStatusCreated);
        }
    }
}
