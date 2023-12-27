using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserSecurityCreateService
    {
        Task<UserSecurity> CreateUserSecurity(UserSecurity userSecurity, IUserSecurityContext context, bool sendMessage = true);
    }
}