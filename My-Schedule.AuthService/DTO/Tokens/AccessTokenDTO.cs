namespace My_Schedule.AuthService.DTO.Tokens
{
    public class AccessTokenDTO
    {
        // rename
        public long AccessTokenExpirationTimestamp { get; set; }
        public string AccessToken { get; set; }
    }
}