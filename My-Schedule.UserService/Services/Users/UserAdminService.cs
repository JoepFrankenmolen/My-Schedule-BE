using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users
{
    public class UserAdminService
    {
        private readonly UserServiceContext _dbContext;
        private readonly IUserAuthDetailHelper _userAuthDetailHelper;
        private readonly IUserUpdateService _userUpdateService;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserAdminService(
            UserServiceContext dbContext,
            IUserAuthDetailHelper userAuthDetailHelper,
            IUserUpdateService userUpdateService,
            IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userAuthDetailHelper = userAuthDetailHelper ?? throw new ArgumentNullException(nameof(userAuthDetailHelper));
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
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
    }
}