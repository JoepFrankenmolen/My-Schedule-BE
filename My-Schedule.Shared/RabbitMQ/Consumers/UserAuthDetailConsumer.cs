using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class UserAuthDetailConsumer<T> : IHostedService where T : DbContext, IUserAuthDetailContext
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IUserAuthDetailCreateService _userAuthDetailCreateService;
        private readonly IUserAuthDetailUpdateService _userAuthDetailUpdateService;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<UserAuthDetailConsumer<T>> _logger;

        public UserAuthDetailConsumer(
            IMessageConsumer messageConsumer,
            IUserAuthDetailCreateService userAuthDetailCreateService,
            IUserAuthDetailUpdateService userAuthDetailUpdateService,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<UserAuthDetailConsumer<T>> logger)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _userAuthDetailCreateService = userAuthDetailCreateService ?? throw new ArgumentNullException(nameof(userAuthDetailCreateService));
            _userAuthDetailUpdateService = userAuthDetailUpdateService ?? throw new ArgumentNullException(nameof(userAuthDetailUpdateService));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<TwoFactorEnabledMessage>(ProcessTwoFactorEnabledMessage, QueueNames.UserAuthDetails.TwoFactorEnabledUpdate, true);
            _messageConsumer.StartConsuming<SuccessfullLoginMessage>(ProcessSuccessfullLoginMessage, QueueNames.UserAuthDetails.SuccessfullLogin, true);
            _messageConsumer.StartConsuming<FailedLoginAttemptMessage>(ProcessFailedLoginAttemptMessage, QueueNames.UserAuthDetails.FailedLoginAttempt, true);
            _messageConsumer.StartConsuming<UserAuthDetailCreatedMessage>(ProcessUserAuthDetailCreatedMessage, QueueNames.UserAuthDetails.UserAuthDetailCreated, true);

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

        private async Task ProcessSuccessfullLoginMessage(SuccessfullLoginMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userAuthDetailUpdateService.UpdateOnLoginSuccess(message.UserId, null, context, false);
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
                    await _userAuthDetailUpdateService.UpdateOnLoginFail(message.UserId, null, message.IsUserBlocked, context, false);
                }

                _logger.LogInformation($"ProcessFailedLoginAttemptMessage processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ProcessFailedLoginAttemptMessage");
            }
        }

        private async Task ProcessUserAuthDetailCreatedMessage(UserAuthDetailCreatedMessage message)
        {
            var userAuthDetail = new UserAuthDetail
            {
                UserId = message.UserId,
                User = message.User,
                TwoFactorEnabled = message.TwoFactorEnabled,
                PasswordHash = string.Empty,
                Salt = string.Empty,
                LastLoginTimestamp = message.LastLoginTimestamp,
                FailedLoginAttempts = message.FailedLoginAttempts,
                LoginCount = message.LoginCount
            };

            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userAuthDetailCreateService.CreateUserAuthDetail(userAuthDetail, context, false);
                }

                _logger.LogInformation($"ProcessUserAuthDetailCreatedMessage processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ProcessUserAuthDetailCreatedMessage");
            }
        }
    }
}