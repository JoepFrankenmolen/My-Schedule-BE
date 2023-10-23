using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users
{
    /// <summary>
    /// Created for the UserAuthDetail object which resembles field the authentication system needs.
    /// </summary>
    public class UserAuthDetailHelper : IUserAuthDetailHelper
    {
        // CREATE
        public async Task<UserAuthDetail> CreateUserAuthDetail(UserAuthDetail userAuthDetail, IUserAuthDetailContext context)
        {
            context.UserAuthDetails.Add(userAuthDetail);
            await context.SaveChangesAsync();

            return userAuthDetail;

            // room for sending messages
        }

        // UPDATE
        public async Task<UserAuthDetail> UpdateOnLoginSuccess(UserAuthDetail user, IUserAuthDetailContext context)
        {
            // reset AccesFailedCount because successfull login attempt
            if (user.FailedLoginAttempts != 0)
            {
                user.FailedLoginAttempts = 0;
            }

            user.LastLoginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            user.LoginCount++;

            await context.SaveChangesAsync();

            return user;
        }

        public async Task<UserAuthDetail> UpdateOnLoginFail(UserAuthDetail user, int maxAttempts, IUserAuthDetailContext context)
        {
            // Block user if exeeding the limit
            if (user.FailedLoginAttempts >= maxAttempts && user.User.IsBlocked == false)
            {
                user.User.IsBlocked = true;
            }

            user.FailedLoginAttempts++;

            await context.SaveChangesAsync();

            return user;
        }

        public async Task<UserAuthDetail> UpdateCredentials(UserAuthDetail user, string passwordHash, string salt, IUserAuthDetailContext context)
        {
            user.PasswordHash = passwordHash;
            user.Salt = salt;
            user.User.TokenRevocationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            await context.SaveChangesAsync();

            return user;
        }

        // GET
        public async Task<List<UserAuthDetail>> GetAll(IUserAuthDetailContext contex)
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
        public async Task<UserAuthDetail> GetUserById(Guid id, IUserAuthDetailContext contex)
        {
            return await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.UserId == id);
        }

        public async Task<UserAuthDetail> GetUserByEmail(string email, IUserAuthDetailContext contex)
        {
            return await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.Email == email);
        }

        public async Task<UserAuthDetail> GetUserByUserName(string userName, IUserAuthDetailContext contex)
        {
            return await contex.UserAuthDetails
                .Include(u => u.User)
                .Include(u => u.User.Roles)
                .FirstOrDefaultAsync(n => n.User.UserName == userName);
        }
    }
}