using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Context;
using My_Schedule.AuthService.Models.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenSessionService: ITokenSessionValidator
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenSessionService(AuthServiceContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Guid> GenerateSession()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();

            var tokenSession = new TokenSession
            {
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                SessionId = Guid.NewGuid(),
                IPAddress = ipAddress,
                UserAgent = userAgent,
                IsBlocked = false,
                BlockedTimestamp = null
            };

            await _dbContext.AddAsync(tokenSession);
            await _dbContext.SaveChangesAsync();

            return tokenSession.SessionId;
        }

        // return true if valid
        public async Task<bool> isValidSession(Guid sessionId)
        {
            return await _dbContext.TokenSessions
                .AnyAsync(ts => ts.SessionId == sessionId && !ts.IsBlocked);
        }

        public async Task<bool> BlockSession(Guid sessionId)
        {
            var session = await GetTokenSessionBySessionId(sessionId);

            if (session != null)
            {
                session.IsBlocked = true;
                session.BlockedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<TokenSession> GetTokenSessionBySessionId(Guid sessionId)
        {
            return await _dbContext.TokenSessions
                .FirstOrDefaultAsync(ts => ts.SessionId == sessionId);
        }
    }
}
