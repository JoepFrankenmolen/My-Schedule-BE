using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserMessages
    {
        public Guid UserId;
    }

    public class UserBLockedMessage : UserMessages
    {
        public bool IsBlocked;
        public long TokenRevocationTimestamp;
    }

    public class UserBannedMessage : UserMessages
    {
        public bool IsBanned;
        public long TokenRevocationTimestamp;
    }

    public class UserEmailConfirmedMessage : UserMessages
    {
        public bool IsEmailConfirmed;
        public long TokenRevocationTimestamp;
    }

    public class UserTokenRevokedMessage : UserMessages
    {
        public long TokenRevocationTimestamp;
    }

    // should be thought out how the connection between user service and auth service goes with this
    public class UserBasicCreatedMessage : UserMessages
    {
        public long CreationTimestamp { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsBanned { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public long TokenRevocationTimestamp { get; set; }
        public List<UserRole> Roles { get; set; }
    }

    public class UserDetailsUpdateMessage : UserMessages
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class UserRolesUpdateMessage : UserMessages
    {
        public UserRole Roles { get; set; }
    }
}