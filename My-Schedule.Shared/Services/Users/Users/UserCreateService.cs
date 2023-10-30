using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.Users
{
    public class UserCreateService : IUserCreateService
    {
        private readonly UserProducer _userProducer;

        public UserCreateService(UserProducer userProducer)
        {
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
        }

        public async Task<User> CreateUser(User user, IUserContext context, bool sendMessage = true)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendUserCreatedMessage(user);
            }

            return user;
        }
    }
}