using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.Shared.Services.Tokens
{
    public class TokenSessionValidator : ITokenSessionValidator
    {
        private readonly ITokenStatusContext _dbContext;

        public TokenSessionValidator(ITokenStatusContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // return true if valid
        public async Task<bool> isValidSession(Guid sessionId)
        {
            // If session not found return true and if session found and blocked return false else true
            var session = await _dbContext.TokenStatus
                .FirstOrDefaultAsync(ts => ts.SessionId == sessionId);

            if (session == null)
            {
                // Session not found, return true
                return true;
            }

            // Session found, check if it's blocked, return true if not blocked
            return !session.IsBlocked;
        }
    }
}