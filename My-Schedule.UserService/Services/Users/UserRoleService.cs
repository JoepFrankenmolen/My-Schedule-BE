using My_Schedule.Shared.Models.Users;
using My_Schedule.UserService.Core;
using My_Schedule.UserService.Services.Users.Helpers;

namespace My_Schedule.UserService.Services.Users
{
    public class UserRoleService
    {
        private readonly UserServiceContext _dbContext;

        public UserRoleService(UserServiceContext dbContext, UserHelper userHelper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UserRole> CreateBasicRole(Guid userId, UserRoleType type)
        {
            return new UserRole { Role = type, UserId = userId };
        }
    }
}