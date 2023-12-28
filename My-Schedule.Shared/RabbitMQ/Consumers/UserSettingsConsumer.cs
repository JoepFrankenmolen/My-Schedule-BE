using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class UserSettingsConsumer<T> : IHostedService where T : DbContext, IUserContext
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<UserSettingsConsumer<T>> _logger;

        public UserSettingsConsumer(
            IMessageConsumer messageConsumer,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<UserSettingsConsumer<T>> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<TwoFactorEnabledMessage>(ProcessTwoFactorEnabledMessage, QueueNames.UserSettings.TwoFactorEnabledUpdate, true);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessTwoFactorEnabledMessage(TwoFactorEnabledMessage message)
        {
            try
            {
                /*using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userAuthDetailHelper.t(tokenStatus, context);
                }*/

                //_logger.LogInformation($"TokenStatusCreate Message processed for SessionId: {message.SessionId}");
                _logger.LogInformation($"Message not implemented yet.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ProcessTwoFactorEnabledMessage");
            }
        }
    }
}