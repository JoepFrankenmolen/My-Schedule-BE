using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserAuthDetailUpdateService
    {
        Task<UserAuthDetail> UpdateCredentials(Guid userId, UserAuthDetail? userAuthDetail, string passwordHash, string salt, IUserAuthDetailContext context, bool sendMessage = true);

        Task<UserAuthDetail> UpdateOnLoginFail(Guid userId, UserAuthDetail? userAuthDetail, bool isUserBlocked, IUserAuthDetailContext context, bool sendMessage = true);

        Task<UserAuthDetail> UpdateOnLoginSuccess(Guid userId, UserAuthDetail? userAuthDetail, IUserAuthDetailContext context, bool sendMessage = true);
    }
}