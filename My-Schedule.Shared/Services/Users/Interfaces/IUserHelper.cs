using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserHelper
    {
        Task<bool> CheckIfEmailExists(string email, IUserContext contex);
        Task CreateUser(User user, IUserContext context);
        Task<List<User>> GetAll(IUserContext contex);
        Task<User> GetUserByEmail(string email, IUserContext contex);
        Task<User> GetUserById(Guid id, IUserContext contex);
        Task<User> GetUserByUserName(string userName, IUserContext contex);
    }
}