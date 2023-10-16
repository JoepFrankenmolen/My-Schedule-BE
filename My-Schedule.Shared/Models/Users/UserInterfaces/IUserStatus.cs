namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserStatus
    {
        bool IsBlocked { get; } // Indicates whether the user is blocked by the system.
        bool IsBanned { get; } // Indicates whether the user is banned by another user.
        bool IsEmailConfirmed { get; }
        long RevocationTimestamp { get; set; } // Timestamp when a user has been revoked (banned or blocked).
    }
}