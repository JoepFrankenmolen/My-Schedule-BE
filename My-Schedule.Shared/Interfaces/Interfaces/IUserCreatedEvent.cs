using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Interfaces.Interfaces
{
    public interface IUserCreatedEvent
    {
        Task UserCreatedEvent(User user, IUserContext context);
    }
}