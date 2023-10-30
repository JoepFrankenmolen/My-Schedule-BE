using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Messages;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.RabbitMQ.Consumers
{
    public class UserConsumer<T> : IHostedService where T : DbContext, IUserContext, IUserAuthDetailContext?
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IUserUpdateService _userUpdateService;
        private readonly IUserCreateService _userCreateService;
        private readonly IDefaultContextBuilder _defaultContextBuilder;
        private readonly ILogger<UserConsumer<T>> _logger;
        private readonly ConsumerConfiguration _consumerConfiguration;

        public UserConsumer(
            IMessageConsumer messageConsumer,
            IUserUpdateService userUpdateService,
            IUserCreateService userCreateService,
            IDefaultContextBuilder defaultContextBuilder,
            ILogger<UserConsumer<T>> logger,
            ConsumerConfiguration consumerConfiguration)
        {
            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
            _userCreateService = userCreateService ?? throw new ArgumentNullException(nameof(userCreateService));
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumerConfiguration = consumerConfiguration ?? throw new ArgumentNullException(nameof(consumerConfiguration));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the background processing logic directly
            _messageConsumer.StartConsuming<UserBannedMessage>(ProcessUserBannedMessage, QueueNames.Users.UserBanned);
            _messageConsumer.StartConsuming<UserBlockedMessage>(ProcessUserBlockedMessage, QueueNames.Users.UserBlocked);
            _messageConsumer.StartConsuming<UserTokenRevokedMessage>(ProcessUserTokenRevokedMessage, QueueNames.Users.UserTokenRevocation);
            _messageConsumer.StartConsuming<UserEmailConfirmationMessage>(ProcessUserEmailConfirmationMessage, QueueNames.Users.UserEmailConfirmation);
            _messageConsumer.StartConsuming<UserIdentityMessage>(ProcessUserIdentityMessage, QueueNames.Users.UserIdentityUpdate);
            _messageConsumer.StartConsuming<UserRoleUpdateMessage>(ProcessUserRoleUpdateMessage, QueueNames.Users.UserRoleUpdate);

            if (!_consumerConfiguration.DoesUserAuthExist)
            {
                _messageConsumer.StartConsuming<UserCreatedMessage>(ProcessUserCreateMessage, QueueNames.Users.UserCreated);
            }
            else
            {
                _logger.LogInformation($"Not processing user creation messages in this service!");
            }

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
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.BanUser(message.UserId, message.IsBanned, message.TokenRevocationTimestamp, context, false);
                }

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
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.BlockUser(message.UserId, message.IsBlocked, message.TokenRevocationTimestamp, context, false);
                }

                _logger.LogInformation($"User Blocked Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Blocked Message");
            }
        }

        private async Task ProcessUserEmailConfirmationMessage(UserEmailConfirmationMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.EmailConfirmation(message.UserId, message.IsEmailConfirmed, message.TokenRevocationTimestamp, context, false);
                }

                _logger.LogInformation($"User Email Confirmed Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Email Confirmed Message");
            }
        }

        private async Task ProcessUserTokenRevokedMessage(UserTokenRevokedMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.TokenRevocation(message.UserId, message.TokenRevocationTimestamp, context, false);
                }

                _logger.LogInformation($"User token revokation Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User token revokation Message");
            }
        }

        private async Task ProcessUserIdentityMessage(UserIdentityMessage message)
        {
            try
            {
                var userIdentity = new UserIdentityDTO { UserName = message.UserName, };

                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.IdentityUpdate(message.UserId, userIdentity, context, false);
                }

                _logger.LogInformation($"User Update Identity Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Update Identity Message");
            }
        }

        private async Task ProcessUserRoleUpdateMessage(UserRoleUpdateMessage message)
        {
            try
            {
                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userUpdateService.RoleUpdate(message.UserId, message.Role, context, false);
                }

                _logger.LogInformation($"User Role Update Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Role Update Message");
            }
        }

        /// <summary>
        /// Should never be used when Auth is present.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessUserCreateMessage(UserCreatedMessage message)
        {
            try
            {
                var user = new User
                {
                    Id = message.UserId,
                    UserName = message.UserName,
                    Email = message.Email,
                    CreationTimestamp = message.CreationTimestamp,
                    IsBanned = message.IsBanned,
                    IsBlocked = message.IsBlocked,
                    IsEmailConfirmed = message.IsEmailConfirmed,
                    Roles = message.Roles,
                    TokenRevocationTimestamp = message.TokenRevocationTimestamp,
                };

                using (var context = _defaultContextBuilder.CreateContext<T>())
                {
                    await _userCreateService.CreateUser(user, context, false);
                }

                _logger.LogInformation($"User Create Message processed for UserId: {message.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing User Create Message");
            }
        }
    }
}