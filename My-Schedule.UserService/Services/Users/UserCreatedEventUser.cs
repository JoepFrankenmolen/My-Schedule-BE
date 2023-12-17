using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.UserService.Services.Users
{
    public class UserCreatedEventUser : IUserCreatedEvent
    {

        public UserCreatedEventUser()
        {
        }

        public Task UserCreatedEvent(User user, IUserContext context)
        {
            Console.WriteLine("not implemented yet on the user but should create the settings obect");

            return Task.CompletedTask;
        }
    }
}