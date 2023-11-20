using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.DTO.Tokens
{
    public class ValidatedTokenUserDTO
    {
        public User user { get; set; }

        public Guid SessionId { get; set; }
    }
}