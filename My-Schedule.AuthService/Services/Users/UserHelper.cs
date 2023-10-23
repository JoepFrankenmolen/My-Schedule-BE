using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Core;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserHelper : IUserBasicHelper
    {
        private readonly AuthServiceContext _dbContext;

        public UserHelper(AuthServiceContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetUserRealById(Guid id)
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

        public async Task<IUserBasic> GetUserById(Guid id)
        {
            return await GetUserRealById(id);
        }

        Task<IUserBasic> IUserBasicHelper.GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IUserBasic> GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}