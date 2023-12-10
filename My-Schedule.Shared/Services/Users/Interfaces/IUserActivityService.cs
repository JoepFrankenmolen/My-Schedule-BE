using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserActivityService
    {
        Task<User> UpdateOnLoginFail(Guid userId, User? user, bool isUserBlocked, IUserSecurityContext context, bool sendMessage = true);

        Task<User> UpdateOnLoginSuccess(Guid userId, User? user, IUserSecurityContext context, bool sendMessage = true);
    }
}