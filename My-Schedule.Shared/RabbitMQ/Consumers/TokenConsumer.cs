using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Tokens;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class TokenConsumer<T> : IHostedService where T : DbContext, ITokenStatusContext
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly TokenStatusService _tokenStatusService;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<TokenConsumer<T>> _logger;

        public TokenConsumer(
            IMessageConsumer messageConsumer,
            TokenStatusService tokenStatusService,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<TokenConsumer<T>> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _tokenStatusService = tokenStatusService ?? throw new ArgumentNullException(nameof(tokenStatusService));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<TokenStatusCreateMessage>(ProcessTokenStatusCreateMessage, QueueNames.Tokens.TokenStatusCreated);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessTokenStatusCreateMessage(TokenStatusCreateMessage message)
        {
            var tokenStatus = new TokenStatus
            {
                Id = message.TokenId,
                SessionId = message.SessionId,
                IsBlocked = message.IsBlocked,
                BlockedTimestamp = message.BlockedTimestamp
            };

            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _tokenStatusService.CreateTokenStatus(tokenStatus, context);
                }

                _logger.LogInformation($"TokenStatusCreate Message processed for SessionId: {message.SessionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing TokenStatusCreate Message");
            }
        }
    }
}