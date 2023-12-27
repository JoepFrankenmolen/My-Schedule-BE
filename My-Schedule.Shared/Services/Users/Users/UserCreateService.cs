using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.Users
{
    public class UserCreateService : IUserCreateService
    {
        private readonly UserProducer _userProducer;
        private readonly IUserCreatedEvent _userCreatedEvent;

        public UserCreateService(UserProducer userProducer, IUserCreatedEvent userCreatedEvent)
        {
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
            _userCreatedEvent = userCreatedEvent ?? throw new ArgumentNullException(nameof(userCreatedEvent));
        }

        public async Task<User> CreateUser(User user, IUserContext context, bool sendMessage = true)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Event Created event on microservices.
            await _userCreatedEvent.UserCreatedEvent(user, context);

            if (sendMessage)
            {
                await _userProducer.SendUserCreatedMessage(user);
            }

            return user;
        }
    }
}