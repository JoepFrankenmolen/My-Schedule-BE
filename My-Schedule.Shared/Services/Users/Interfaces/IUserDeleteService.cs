using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserDeleteService
    {
        Task<User> DeleteUser(Guid userId, long timestamp, IUserContext context, bool sendMessage = true);
    }
}