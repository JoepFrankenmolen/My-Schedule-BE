using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserAuthDetailCreateService
    {
        Task<UserAuthDetail> CreateUserAuthDetail(UserAuthDetail userAuthDetail, IUserAuthDetailContext context, bool sendMessage = true);
    }
}