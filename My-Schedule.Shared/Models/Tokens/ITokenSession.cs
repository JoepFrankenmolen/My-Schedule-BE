using My_Schedule.Shared.Models;
using My_Schedule.Shared.Models.ClientDetails;

namespace SecureLogin.Data.Models.Tokens
{
    public interface ITokenSession : IEntityWithGuidKey
    {
        Guid SessionId { get; set; }
        long CreationTimestamp { get; set; }
        IClientDetails ClientDetails { get; set; }
        bool IsBlocked { get; set; }
        long? BlockedTimestamp { get; set; }
    }
}