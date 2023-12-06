using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.UserService.Services.Users
{
    public class UserCreatedEventUser : IUserCreatedEvent
    {
        private readonly IUserAuthDetailCreateService _userAuthDetailCreateService;

        public UserCreatedEventUser(IUserAuthDetailCreateService userAuthDetailCreateService) 
        {
            _userAuthDetailCreateService = userAuthDetailCreateService ?? throw new ArgumentNullException(nameof(userAuthDetailCreateService));
        }

        public Task UserCreatedEvent(User user, IUserContext context)
        {
            Console.WriteLine("not implemented yet on the user but should create the auth obect");

            return Task.CompletedTask;
        }
    }
}
