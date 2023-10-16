namespace My_Schedule.Shared.Interfaces.AppSettings
{
    public interface IAuthenticationSettings
    {
        string JWTIssuer { get; }
        string JWTAudience { get; }
        string JWTSigningKey { get; }
        int AccessTokenExpirationTime { get; }
        int RefreshTokenExpirationTime { get; }
    }
}