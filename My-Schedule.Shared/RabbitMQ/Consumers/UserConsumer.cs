using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class UserConsumer<T> : IHostedService where T : DbContext, IUserContext
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IUserUpdateService _userUpdateService;
        private readonly IUserCreateService _userCreateService;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<UserConsumer<T>> _logger;

        public UserConsumer(
            IMessageConsumer messageConsumer,
            IUserUpdateService userUpdateService,
            IUserCreateService userCreateService,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<UserConsumer<T>> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
            _userCreateService = userCreateService ?? throw new ArgumentNullException(nameof(userCreateService));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<UserBannedMessage>(ProcessUserBannedMessage, QueueNames.Users.UserBanned);
            _messageConsumer.StartConsuming<UserBlockedMessage>(ProcessUserBlockedMessage, QueueNames.Users.UserBlocked);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop any ongoing processing and perform cleanup if needed
            _messageConsumer.Dispose();
            // Return a completed task
            return Task.CompletedTask;
        }

        private async Task ProcessUserBannedMessage(UserBannedMessage message)
        {
            try
            {
                // Process the UserBannedMessage here
                // Example: Update the user's banned status in the database
                //await _userUpdateService.BanUser(message.UserId, message.TokenRevocationTimestamp);

                _logger.LogInformation($"User Banned Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Banned Message");
            }
        }

        private async Task ProcessUserBlockedMessage(UserBlockedMessage message)
        {
            try
            {
                // Process the UserBannedMessage here
                // Example: Update the user's banned status in the database
                //await _userUpdateService.BanUser(message.UserId, message.TokenRevocationTimestamp);

                _logger.LogInformation($"User Blocked Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Blocked Message");
            }
        }
    }
}