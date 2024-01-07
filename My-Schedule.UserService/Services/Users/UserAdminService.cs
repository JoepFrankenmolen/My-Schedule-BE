using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users
{
    public class UserAdminService
    {
        private readonly UserServiceContext _dbContext;
        private readonly IUserUpdateService _userUpdateService;
        private readonly IUserDeleteService _userDeleteService;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserAdminService(
            UserServiceContext dbContext,
            IUserUpdateService userUpdateService,
            IUserDeleteService userDeleteService,
            IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
            _userDeleteService = userDeleteService ?? throw new ArgumentNullException(nameof(userDeleteService));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task BanUser(string userId, bool state)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var userIdParsed = Guid.Parse(userId);
            await _userUpdateService.BanUser(userIdParsed, state, timestamp, _dbContext);
        }

        public async Task BlockUser(string userId, bool state)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var userIdParsed = Guid.Parse(userId);
            await _userUpdateService.BlockUser(userIdParsed, state, timestamp, _dbContext);
        }

        public async Task DeleteUser(string userId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var userIdParsed = Guid.Parse(userId);
            await _userDeleteService.DeleteUser(userIdParsed, timestamp, _dbContext);
        }
    }
}