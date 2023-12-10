using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.UserSecurities
{
    public class UserSecurityFetcherService
    {
        // GET
        public static async Task<List<UserSecurity>> GetAll(IUserSecurityContext contex)
        {
            return await contex.UserSecurities
                .Include(u => u.User)
                .Include(u => u.User.Roles).ToListAsync();
        }

        /// <summary>
        /// It is the UserId not the UserAuthDetailId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contex"></param>
        /// <returns></returns>
        public static async Task<UserSecurity> GetUserById(Guid id, IUserSecurityContext contex, bool canReturnNull = true)
        {
            var userSecurity = await contex.UserSecurities
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.UserId == id);

            if (canReturnNull)
            {
                return userSecurity;
            }

            return userSecurity ?? throw new ArgumentNullException(nameof(userSecurity));
        }

        public static async Task<UserSecurity> GetUserByEmail(string email, IUserSecurityContext contex, bool canReturnNull = true)
        {
            var userSecurity = await contex.UserSecurities
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.Email == email);

            if (canReturnNull)
            {
                return userSecurity;
            }

            return userSecurity ?? throw new ArgumentNullException(nameof(userSecurity));
        }

        public static async Task<UserSecurity> GetUserByUserName(string userName, IUserSecurityContext contex, bool canReturnNull = true)
        {
            var userSecurity = await contex.UserSecurities
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.UserName == userName);

            if (canReturnNull)
            {
                return userSecurity;
            }

            return userSecurity ?? throw new ArgumentNullException(nameof(userSecurity));
        }
    }
}