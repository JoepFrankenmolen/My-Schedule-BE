using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.UserAuthDetails
{
    public class UserAuthDetailFetcherService
    {
        // GET
        public static async Task<List<UserAuthDetail>> GetAll(IUserAuthDetailContext contex)
        {
            return await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles).ToListAsync();
        }

        /// <summary>
        /// It is the UserId not the UserAuthDetailId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contex"></param>
        /// <returns></returns>
        public static async Task<UserAuthDetail> GetUserById(Guid id, IUserAuthDetailContext contex, bool canReturnNull = true)
        {
            var userAuthDetail = await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.UserId == id);

            if (canReturnNull)
            {
                return userAuthDetail;
            }

            return userAuthDetail ?? throw new ArgumentNullException(nameof(userAuthDetail));
        }

        public static async Task<UserAuthDetail> GetUserByEmail(string email, IUserAuthDetailContext contex, bool canReturnNull = true)
        {
            var userAuthDetail = await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.Email == email);

            if (canReturnNull)
            {
                return userAuthDetail;
            }

            return userAuthDetail ?? throw new ArgumentNullException(nameof(userAuthDetail));
        }

        public static async Task<UserAuthDetail> GetUserByUserName(string userName, IUserAuthDetailContext contex, bool canReturnNull = true)
        {
            var userAuthDetail = await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.UserName == userName);

            if (canReturnNull)
            {
                return userAuthDetail;
            }

            return userAuthDetail ?? throw new ArgumentNullException(nameof(userAuthDetail));
        }
    }
}