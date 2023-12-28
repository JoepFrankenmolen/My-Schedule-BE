using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class UserActivityConsumer<T> : IHostedService where T : DbContext, IUserContext
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IUserActivityService _userActivityService;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<UserActivityConsumer<T>> _logger;

        public UserActivityConsumer(
            IMessageConsumer messageConsumer,
            IUserActivityService userActivityService,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<UserActivityConsumer<T>> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _userActivityService = userActivityService ?? throw new ArgumentNullException(nameof(userActivityService));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<SuccessfullLoginMessage>(ProcessSuccessfullLoginMessage, QueueNames.UserActivity.SuccessfullLogin, true);
            _messageConsumer.StartConsuming<FailedLoginAttemptMessage>(ProcessFailedLoginAttemptMessage, QueueNames.UserActivity.FailedLoginAttempt, true);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessSuccessfullLoginMessage(SuccessfullLoginMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userActivityService.UpdateOnLoginSuccess(message.UserId, null, context, false);
                }

                _logger.LogInformation($"ProcessSuccessfullLoginMessage processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ProcessSuccessfullLoginMessage");
            }
        }

        private async Task ProcessFailedLoginAttemptMessage(FailedLoginAttemptMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userActivityService.UpdateOnLoginFail(message.UserId, null, message.IsUserBlocked, context, false);
                }

                _logger.LogInformation($"ProcessFailedLoginAttemptMessage processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ProcessFailedLoginAttemptMessage");
            }
        }
    }
}