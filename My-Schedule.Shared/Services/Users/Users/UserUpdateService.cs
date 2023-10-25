using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.Users
{
    public class UserUpdateService : IUserUpdateService
    {
        private readonly UserProducer _userProducer;

        public UserUpdateService(UserProducer userProducer)
        {
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
        }

        public async Task<User> BanUser(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.IsBanned = state;
            user.TokenRevocationTimestamp = timestamp;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendUserBannedMessage(user.Id, state, timestamp);
            }

            return user;
        }

        public async Task<User> BlockUser(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.IsBlocked = state;
            user.TokenRevocationTimestamp = timestamp;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendUserBlockedMessage(user.Id, state, timestamp);
            }

            return user;
        }

        public async Task<User> TokenRevocation(Guid userId, long timestamp, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.TokenRevocationTimestamp = timestamp;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendTokenRevovationMessage(user.Id, timestamp);
            }

            return user;
        }

        public async Task<User> EmailConfirmation(Guid userId, bool state, long timestamp, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.IsEmailConfirmed = state;
            user.TokenRevocationTimestamp = timestamp;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendEmailConfirmationMessage(user.Id, state, timestamp);
            }

            return user;
        }

        public async Task<User> IdentityUpdate(Guid userId, UserIdentityDTO userIdentity, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            user.UserName = userIdentity.UserName;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendIdentityUpdateMessage(user.Id, userIdentity);
            }

            return user;
        }

        public async Task<User> RoleUpdate(Guid userId, UserRole role, IUserContext context, bool sendMessage = true)
        {
            var user = await UserFetcherService.GetUserById(userId, context);

            if (user.Roles.Contains(role))
            {
                user.Roles.Remove(role);
            }
            else
            {
                user.Roles.Add(role);
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userProducer.SendRoleUpdateMessage(user.Id, role);
            }

            return user;
        }
    }
}