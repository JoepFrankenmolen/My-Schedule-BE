using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using My_Schedule.Shared.Services.Users;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserHelper : IUserHelper
    {
        private readonly AuthServiceContext _dbContext;

        public UserHelper(AuthServiceContext dbContext)
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

        public async Task<User> GetUserForAuthorization(Guid id)
        {
            return await _dbContext.Users
                .Include(n => n.Roles)
                .Where(n => n.IsBlocked == false)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IUserBasic> GetUserBasicById(Guid id)
        {
            return (IUserBasic)await GetUserById(id);
        }
    }
}