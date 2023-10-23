using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserAuthDetailHelper
    {
        Task<UserAuthDetail> CreateUserAuthDetail(UserAuthDetail userAuthDetail, IUserAuthDetailContext context);
        Task<UserAuthDetail> UpdateOnLoginSuccess(UserAuthDetail user, IUserAuthDetailContext context);
        Task<UserAuthDetail> UpdateOnLoginFail(UserAuthDetail user, int maxAttempts, IUserAuthDetailContext context);
        Task<UserAuthDetail> UpdateCredentials(UserAuthDetail user, string passwordHash, string salt, IUserAuthDetailContext context);
        Task<List<UserAuthDetail>> GetAll(IUserAuthDetailContext contex);
        Task<UserAuthDetail> GetUserByEmail(string email, IUserAuthDetailContext contex);
        Task<UserAuthDetail> GetUserById(Guid id, IUserAuthDetailContext contex);
        Task<UserAuthDetail> GetUserByUserName(string userName, IUserAuthDetailContext contex);
    }
}