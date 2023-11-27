using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Users
{
    /// <summary>
    /// Fetches all the data from the DB
    /// </summary>
    public class UserFetcherService
    {
        public static async Task<List<User>> GetAll(IUserContext context)
        {
            var users = await context.Users
                .Include(u => u.Roles).ToListAsync();

            return users ?? throw new ArgumentNullException(nameof(users));
        }

        public static async Task<User> GetUserById(Guid id, IUserContext context)
        {
            var user = await context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.Id == id);

            return user ?? throw new ArgumentNullException(nameof(user));
        }

        public static async Task<User> GetUserByEmail(string email, IUserContext context)
        {
            var user = await context.Users
                .Include(u => u.Roles)
            .FirstOrDefaultAsync(n => n.Email == email);
            return user ?? throw new ArgumentNullException(nameof(user));
        }

        public static async Task<User> GetUserByUserName(string userName, IUserContext context)
        {
            var user = await context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(n => n.UserName == userName);
            return user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}