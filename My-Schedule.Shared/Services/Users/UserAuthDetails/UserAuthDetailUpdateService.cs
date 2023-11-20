using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.UserAuthDetails
{
    public class UserAuthDetailUpdateService : IUserAuthDetailUpdateService
    {
        private readonly UserAuthDetailProducer _userAuthDetailProducer;
        private readonly IUserUpdateService _userUpdateService;

        public UserAuthDetailUpdateService(UserAuthDetailProducer userAuthDetailProducer, IUserUpdateService userUpdateService)
        {
            _userAuthDetailProducer = userAuthDetailProducer ?? throw new ArgumentNullException(nameof(userAuthDetailProducer));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
        }

        public async Task<UserAuthDetail> UpdateOnLoginSuccess(
            Guid userId,
            UserAuthDetail? userAuthDetail,
            IUserAuthDetailContext context,
            bool sendMessage = true)
        {
            if (userAuthDetail == null)
            {
                userAuthDetail = await UserAuthDetailFetcherService.GetUserById(userId, context, false);
            }

            // reset AccesFailedCount because successfull login attempt
            if (userAuthDetail.FailedLoginAttempts != 0)
            {
                userAuthDetail.FailedLoginAttempts = 0;
            }

            userAuthDetail.LastLoginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            userAuthDetail.LoginCount++;

            context.UserAuthDetails.Update(userAuthDetail);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userAuthDetailProducer.SendSuccessfullLoginMessage(
                    userAuthDetail.UserId,
                    userAuthDetail.LastLoginTimestamp,
                    userAuthDetail.LoginCount,
                    userAuthDetail.FailedLoginAttempts);
            }

            return userAuthDetail;
        }

        public async Task<UserAuthDetail> UpdateOnLoginFail(
            Guid userId,
            UserAuthDetail? userAuthDetail,
            bool isUserBlocked,
            IUserAuthDetailContext context,
            bool sendMessage = true)
        {
            if (userAuthDetail == null)
            {
                userAuthDetail = await UserAuthDetailFetcherService.GetUserById(userId, context, false);
            }

            // Check if the user is blocked and a message must be send.
            // This is to prevent a double update due to the BlockUser being a seperate message bus.
            if (isUserBlocked && sendMessage)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _userUpdateService.BlockUser(userAuthDetail.UserId, true, timestamp, context, sendMessage);
            }

            userAuthDetail.FailedLoginAttempts++;

            context.UserAuthDetails.Update(userAuthDetail);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userAuthDetailProducer.SendFailedLoginAttemptMessage(userAuthDetail.UserId, userAuthDetail.FailedLoginAttempts, isUserBlocked);
            }

            return userAuthDetail;
        }

        public async Task<UserAuthDetail> UpdateCredentials(
            Guid userId,
            UserAuthDetail? userAuthDetail,
            string passwordHash,
            string salt,
            IUserAuthDetailContext context,
            bool sendMessage = true)
        {
            if (userAuthDetail == null)
            {
                userAuthDetail = await UserAuthDetailFetcherService.GetUserById(userId, context, false);
            }

            userAuthDetail.PasswordHash = passwordHash;
            userAuthDetail.Salt = salt;

            await context.SaveChangesAsync();

            var revocationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await _userUpdateService.TokenRevocation(userAuthDetail.UserId, revocationTimestamp, context, sendMessage);

            return userAuthDetail;
        }
    }
}