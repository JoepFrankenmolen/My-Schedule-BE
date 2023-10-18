using My_Schedule.Shared.Models.Users.UserInterfaces.Helpers;

namespace My_Schedule.Shared.DTO.Tokens
{
    public class ValidatedTokenUserDTO
    {
        public IUserBasic user { get; set; }

        public Guid SessionId { get; set; }
    }
}