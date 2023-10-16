using My_Schedule.AuthService.Models;

namespace My_Schedule.AuthService.DTO.Auth.Tokens
{
    public class ValidatedTokenUserDTO
    {
        public User user { get; set; }

        public Guid SessionId { get; set; }
    }
}