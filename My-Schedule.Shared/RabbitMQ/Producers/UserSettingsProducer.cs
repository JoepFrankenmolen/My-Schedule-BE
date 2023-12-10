using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Messages;

namespace My_Schedule.Shared.RabbitMQ.Producers
{
    public class UserSettingsProducer
    {
        private readonly IMessageProducer _messageProducer;

        public UserSettingsProducer(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task SendTwoFactorEnabledMessage(Guid userId, bool state)
        {
            var message = new TwoFactorEnabledMessage
            {
                UserId = userId,
                TwoFactorEnabled = state,
            };

            await _messageProducer.SendFanMessage(message, QueueNames.UserSettings.TwoFactorEnabledUpdate);
        }
    }
}