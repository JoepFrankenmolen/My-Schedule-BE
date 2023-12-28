using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.Users
{
    public class UserActivityService : IUserActivityService
    {
        private readonly UserActivityProducer _userActivityProducer;
        private readonly IUserUpdateService _userUpdateService;

        public UserActivityService(UserActivityProducer userActivityProducer, IUserUpdateService userUpdateService)
        {
            _userActivityProducer = userActivityProducer ?? throw new ArgumentNullException(nameof(userActivityProducer));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
        }

        public async Task<User> UpdateOnLoginSuccess(
            Guid userId,
            User? user,
            IUserContext context,
            bool sendMessage = true)
        {
            if (user == null)
            {
                user = await UserFetcherService.GetUserById(userId, context);
            }

            // reset AccesFailedCount because successfull login attempt
            if (user.FailedLoginAttempts != 0)
            {
                user.FailedLoginAttempts = 0;
            }

            user.LastLoginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            user.LoginCount++;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userActivityProducer.SendSuccessfullLoginMessage(
                    user.Id,
                    user.LastLoginTimestamp,
                    user.LoginCount,
                    user.FailedLoginAttempts);
            }

            return user;
        }

        public async Task<User> UpdateOnLoginFail(
            Guid userId,
            User? user,
            bool isUserBlocked,
            IUserContext context,
            bool sendMessage = true)
        {
            if (user == null)
            {
                user = await UserFetcherService.GetUserById(userId, context);
            }

            // Check if the user is blocked and a message must be send.
            // This is to prevent a double update due to the BlockUser being a seperate message bus.
            if (isUserBlocked && sendMessage)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _userUpdateService.BlockUser(user.Id, true, timestamp, context, sendMessage);
            }

            user.FailedLoginAttempts++;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userActivityProducer.SendFailedLoginAttemptMessage(user.Id, user.FailedLoginAttempts, isUserBlocked);
            }

            return user;
        }
    }
}