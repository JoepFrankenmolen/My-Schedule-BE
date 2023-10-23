using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserCreateService
    {
        Task CreateUser(User user, IUserContext context, bool sendMessage = true);
    }
}