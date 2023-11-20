using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users.Interfaces
{
    public interface IUserUpdateService
    {
        Task<User> BanUser(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true);

        Task<User> BlockUser(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true);

        Task<User> EmailConfirmation(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true);

        Task<User> IdentityUpdate(Guid userId, UserIdentityDTO userIdentity, IUserContext context, bool sendMessage = true);

        Task<User> RoleUpdate(Guid userId, UserRole role, IUserContext context, bool sendMessage = true);

        Task<User> TokenRevocation(Guid userId, long timestamp, IUserContext context, bool sendMessage = true);
    }
}