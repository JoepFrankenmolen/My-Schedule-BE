namespace My_Schedule.AuthService.DTO.Tokens
{
    public class TokenDTO
    {
        public long AccessTokenExpirationTimestamp { get; set; }
        public string AccessToken { get; set; }

        public long RefreshTokenExpirationTimestamp { get; set; }
        public string RefreshToken { get; set; }
    }
}