using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users.Helpers
{
    public class UserHelper : IUserBasicHelper
    {
        private readonly UserServiceContext _dbContext;

        public UserHelper(UserServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(n => n.Email == email);
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            return await _dbContext.Users.AnyAsync(n => n.Email == email);
        }

        public async Task<IUserBasic> GetUserBasicById(Guid id)
        {
            return await GetUserById(id);
        }
    }
}