using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users
{
    public class UserAdminService
    {
        private readonly UserServiceContext _dbContext;
        private readonly IUserHelper _userHelper;
        private readonly IUserAuthDetailHelper _userAuthDetailHelper;
        private readonly UserProducer _userProducer;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserAdminService(
            UserServiceContext dbContext,
            IUserHelper userHelper,
            IUserAuthDetailHelper userAuthDetailHelper,
            UserProducer userProducer,
            IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _userAuthDetailHelper = userAuthDetailHelper ?? throw new ArgumentNullException(nameof(userAuthDetailHelper));
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task BanUser(string userId, bool state, bool sendMessage = true)
        {
            var user = await _userHelper.GetUserById(Guid.Parse(userId), _dbContext);

            if (user == null)
            {
                throw new ArgumentNullException("userId failed to fetch");
            }

            user.IsBanned = state;

            if (sendMessage)
            {
                await _userProducer.SendUserBannedMessage(user.Id, state);
            }

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task BlockUser(string userId, bool state, bool sendMessage = true)
        {
            var user = await _userHelper.GetUserById(Guid.Parse(userId), _dbContext);

            if (user == null)
            {
                throw new ArgumentNullException("userId failed to fetch");
            }

            user.IsBlocked = true;

            if (sendMessage)
            {
                await _userProducer.SendUserBlockedMessage(user.Id, state);
            }

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}