namespace My_Schedule.AuthService.DTO.Tokens
{
    public class TokenDTO
    {
        public int AccessTokenExpirationTimestamp { get; set; }
        public string AccessToken { get; set; }

        public int RefreshTokenExpirationTimestamp { get; set; }
        public string RefreshToken { get; set; }
    }
}