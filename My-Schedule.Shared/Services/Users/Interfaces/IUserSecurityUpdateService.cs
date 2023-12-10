using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserSecurityUpdateService
    {
        Task<UserSecurity> UpdateCredentials(Guid userId, UserSecurity? userSecurity, string passwordHash, string salt, IUserSecurityContext context, bool sendMessage = true);
    }
}