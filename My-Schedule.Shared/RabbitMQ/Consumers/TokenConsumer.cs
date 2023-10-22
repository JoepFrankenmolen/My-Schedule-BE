using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Tokens;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class TokenConsumer<T> : IHostedService where T : DbContext, ITokenStatusContext
    {
        private readonly MessageConsumer _messageConsumer;
        private readonly TokenStatusService _tokenStatusService;

        public TokenConsumer(MessageConsumer messageConsumer, TokenStatusService tokenStatusService)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _tokenStatusService = tokenStatusService ?? throw new ArgumentNullException(nameof(tokenStatusService));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<TokenStatusCreateMessage>(ProcessMessage, QueueNames.Tokens.TokenStatusCreated);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessMessage(TokenStatusCreateMessage message)
        {
            var tokenStatus = new TokenStatus
            {
                Id = message.TokenId,
                SessionId = message.SessionId,
                IsBlocked = message.IsBlocked,
                BlockedTimestamp = message.BlockedTimestamp
            };

            await _tokenStatusService.CreateTokenStatus<T>(tokenStatus);
            // Specific message processing logic for TokenConsumer
            Console.WriteLine("TokenConsumer received: {0}", message);
            // Add your custom message processing logic here
        }
    }
}
