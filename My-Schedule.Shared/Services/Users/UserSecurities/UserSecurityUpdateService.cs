using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.UserSecurities
{
    public class UserSecurityUpdateService : IUserSecurityUpdateService
    {
        private readonly IUserUpdateService _userUpdateService;

        public UserSecurityUpdateService(IUserUpdateService userUpdateService)
        {
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
        }

        public async Task<UserSecurity> UpdateCredentials(
            Guid userId,
            UserSecurity? userSecurity,
            string passwordHash,
            string salt,
            IUserSecurityContext context,
            bool sendMessage = true)
        {
            if (userSecurity == null)
            {
                userSecurity = await UserSecurityFetcherService.GetUserById(userId, context, false);
            }

            userSecurity.PasswordHash = passwordHash;
            userSecurity.Salt = salt;

            await context.SaveChangesAsync();

            var revocationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            await _userUpdateService.TokenRevocation(userSecurity.UserId, revocationTimestamp, context, sendMessage);

            return userSecurity;
        }
    }
}