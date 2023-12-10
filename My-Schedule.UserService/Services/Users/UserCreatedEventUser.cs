using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.UserService.Services.Users
{
    public class UserCreatedEventUser : IUserCreatedEvent
    {
        private readonly IUserSecurityCreateService _userSecurityCreateService;

        public UserCreatedEventUser(IUserSecurityCreateService userSecurityCreateService)
        {
            _userSecurityCreateService = userSecurityCreateService ?? throw new ArgumentNullException(nameof(userSecurityCreateService));
        }

        public Task UserCreatedEvent(User user, IUserContext context)
        {
            Console.WriteLine("not implemented yet on the user but should create the auth obect");

            return Task.CompletedTask;
        }
    }
}