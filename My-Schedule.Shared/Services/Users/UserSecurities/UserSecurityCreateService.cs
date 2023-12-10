using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.UserSecurities
{
    public class UserSecurityCreateService : IUserSecurityCreateService
    {
        private readonly IUserCreateService _userCreateService;

        public UserSecurityCreateService(IUserCreateService userCreateService)
        {
            _userCreateService = userCreateService ?? throw new ArgumentNullException(nameof(userCreateService));
        }

        public async Task<UserSecurity> CreateUserSecurity(
            UserSecurity userSecurity,
            IUserSecurityContext context,
            bool sendMessage = true)
        {
            // Create user and send message if enabled.
            await _userCreateService.CreateUser(userSecurity.User, context, sendMessage);

            context.UserSecurities.Add(userSecurity);
            await context.SaveChangesAsync();

            return userSecurity;
        }
    }
}