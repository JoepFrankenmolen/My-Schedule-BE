using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.Users
{
    public class UserDeleteService : IUserDeleteService
    {
        private readonly UserProducer _userProducer;
        private const string DeletedVariable = "(Deleted)";

        public UserDeleteService(UserProducer userProducer)
        {
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
        }

        public async Task<User> DeleteUser(Guid userId, long timestamp, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.Email = DeletedVariable;
            user.UserName = DeletedVariable;
            user.TokenRevocationTimestamp = timestamp;

            // Preventing another login or access attempt by blocking and banning the user.
            user.IsBlocked = true;
            user.IsBanned = true;

            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendUserDeletedMessage(userId, timestamp);
            }

            return user;
        }
    }
}