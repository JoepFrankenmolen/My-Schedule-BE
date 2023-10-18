namespace My_Schedule.Shared.Services.Tokens
{
    public interface ITokenSessionValidator
    {
        Task<bool> isValidSession(Guid sessionId);
    }
}