using My_Schedule.AuthService.Services.Notifications;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserCreatedEventAuth : IUserCreatedEvent
    {
        public Task UserCreatedEvent(User user, IUserContext context)
        {
            Console.WriteLine("not implemented yet on the auth but should create the auth obect and send the user an invite to create an account (privde the password)");

            return Task.CompletedTask;
        }
    }
}
