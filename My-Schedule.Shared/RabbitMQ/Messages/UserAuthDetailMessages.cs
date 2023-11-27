using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserAuthDetailMessages
    {
        public Guid UserId;
    }

    public class TwoFactorEnabledMessage : UserMessages
    {
        public bool TwoFactorEnabled;
    }

    public class SuccessfullLoginMessage : UserMessages
    {
        public long LastLoginTimestamp;
        public int LoginCount;
        public int FailedLoginAttempts;
    }

    public class FailedLoginAttemptMessage : UserMessages
    {
        public int FailedLoginAttempts;
        public bool IsUserBlocked;
    }

    public class UserAuthDetailCreatedMessage : UserMessages
    {
        // IUserDetails
        public User User;

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