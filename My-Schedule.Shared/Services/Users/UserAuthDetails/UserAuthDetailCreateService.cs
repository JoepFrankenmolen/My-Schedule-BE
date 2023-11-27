using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.Shared.Services.Users.UserAuthDetails
{
    public class UserAuthDetailCreateService : IUserAuthDetailCreateService
    {
        private readonly UserAuthDetailProducer _userAuthDetailProducer;
        private readonly IUserCreateService _userCreateService;

        public UserAuthDetailCreateService(UserAuthDetailProducer userAuthDetailProducer, IUserCreateService userCreateService)
        {
            _userAuthDetailProducer = userAuthDetailProducer ?? throw new ArgumentNullException(nameof(userAuthDetailProducer));
            _userCreateService = userCreateService ?? throw new ArgumentNullException(nameof(userCreateService));
        }

        public async Task<UserAuthDetail> CreateUserAuthDetail(
            UserAuthDetail userAuthDetail,
            IUserAuthDetailContext context,
            bool sendMessage = true)
        {
            // Create user and send message if enabled.
            await _userCreateService.CreateUser(userAuthDetail.User, context, sendMessage);

            context.UserAuthDetails.Add(userAuthDetail);
            await context.SaveChangesAsync();

            if (sendMessage)
            {
                await _userAuthDetailProducer.SendUserAuthDetailCreatedMessage(userAuthDetail);
            }

            return userAuthDetail;
        }
    }
}