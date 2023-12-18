using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserAuthDetailMessages
    {
        public Guid UserId;
    }

    public class UserAuthDetailCreatedMessage : UserMessages
    {
        // IUserDetails
        public User User;
        HeaderDictionaryTypeExtensions moet gefixt worden
        // IUserSecurity
        public bool TwoFactorEnabled; // should be in user settings :)

        /*        public string PasswordHash;
                public string Salt;*/

        // IUserActivity
        public long LastLoginTimestamp;

        public int FailedLoginAttempts;
        public int LoginCount;
    }
}