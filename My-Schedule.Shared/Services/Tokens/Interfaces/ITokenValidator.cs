using My_Schedule.Shared.DTO.Tokens;

namespace My_Schedule.Shared.Services.Tokens.Interfaces
{
    public interface ITokenValidator
    {
        Task<ValidatedTokenUserDTO> ValidateToken(string token, TokenType type);
    }
}