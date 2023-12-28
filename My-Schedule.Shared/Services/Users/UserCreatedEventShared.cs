using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Services.Users
{
    public class UserCreatedEventShared : IUserCreatedEvent
    {
        public UserCreatedEventShared()
        {
        }

        public Task UserCreatedEvent(User user, IUserContext context)
        {
            Console.WriteLine("not implemented yet on the user but should create the settings obect");

            return Task.CompletedTask;
        }
    }
}