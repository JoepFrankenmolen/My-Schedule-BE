using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users
{
    public class UserHelper : IUserHelper
    {
        private readonly IDefaultContextBuilder _defaultContextBuilder;

        public UserHelper(IDefaultContextBuilder dbContext)
        {
            _defaultContextBuilder = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // CREATE
        public async Task CreateUser(User user, IUserContext context)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // room for sending messages
        }

        // UPDATE

        // CHECK
        public async Task<bool> CheckIfEmailExists(string email, IUserContext contex)
        {
            return await contex.Users.AnyAsync(n => n.Email == email);
        }

        // GET
        public async Task<List<User>> GetAll(IUserContext contex)
        {
            return await contex.Users
                .Include(u => u.Roles).ToListAsync();
        }

        public async Task<User> GetUserById(Guid id, IUserContext contex)
        {
            return await contex.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<User> GetUserByEmail(string email, IUserContext contex)
        {
            return await contex.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.Email == email);
        }

        public async Task<User> GetUserByUserName(string userName, IUserContext contex)
        {
            return await contex.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.UserName == userName);
        }
    }
}