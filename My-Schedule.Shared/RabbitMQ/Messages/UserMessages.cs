﻿using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.RabbitMQ.Messages
{
    public abstract class UserMessages
    {
        public Guid UserId;
    }

    public class UserBlockedMessage : UserMessages
    {
        public bool IsBlocked;
        public long TokenRevocationTimestamp;
    }

    public class UserBannedMessage : UserMessages
    {
        public bool IsBanned;
        public long TokenRevocationTimestamp;
    }

    public class UserEmailConfirmationMessage : UserMessages
    {
        public bool IsEmailConfirmed;
        public long TokenRevocationTimestamp;
    }

    public class UserTokenRevokedMessage : UserMessages
    {
        public long TokenRevocationTimestamp;
    }

    public class UserCreatedMessage : UserMessages
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

    public class UserDeletedMessage : UserMessages
    {
        public long TokenRevocationTimestamp { get; set; }
    }

    public class UserIdentityMessage : UserMessages
    {
        public string UserName { get; set; }
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

    public class UserRoleUpdateMessage : UserMessages
    {
        public UserRole Role { get; set; }
    }
}