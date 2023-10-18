namespace My_Schedule.Shared.Models.Users.UserInterfaces
{
    public interface IUserStatus
    {
        bool IsBlocked { get; } // Indicates whether the user is blocked by the system.
        bool IsBanned { get; } // Indicates whether the user is banned by another user.
        bool IsEmailConfirmed { get; }
        long TokenRevocationTimestamp { get; set; } // Timestamp from when on a token has been revoked.
    }
}