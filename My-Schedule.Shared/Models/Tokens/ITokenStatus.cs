namespace SecureLogin.Data.Models.Tokens
{
    public interface ITokenStatus
    {
        Guid Id { get; set; }
        Guid SessionId { get; set; }
        long CreationTimestamp { get; set; }
        bool IsBlocked { get; set; }
        long? BlockedTimestamp { get; set; }
    }
}