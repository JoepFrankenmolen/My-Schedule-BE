﻿namespace My_Schedule.AuthService.DTO.Tokens
{
    public class TokenDTO : AccessTokenDTO
    {
        public long AccessTokenExpirationTimestamp { get; set; }
        public string AccessToken { get; set; }
    }
}