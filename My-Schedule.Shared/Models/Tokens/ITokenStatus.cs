using My_Schedule.Shared.Models;

namespace SecureLogin.Data.Models.Tokens
{
    public interface ITokenStatus : IEntityWithGuidKey
    {
        Guid SessionId { get; set; }
        bool IsBlocked { get; set; }
        long? BlockedTimestamp { get; set; }
    }
}