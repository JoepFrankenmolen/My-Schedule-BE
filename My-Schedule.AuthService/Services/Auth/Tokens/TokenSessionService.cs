using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SecureLogin.Data.Context;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenSessionService
    {
        private readonly SecureLoginContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenSessionService(SecureLoginContext dbContext, IHttpContextAccessor httpContextAccessor)
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
                CreationTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                SessionId = Guid.NewGuid(),
                IPAddress = ipAddress,
                UserAgent = userAgent,
                IsBlocked = false,
                BlockedTimeStamp = null
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
                session.BlockedTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

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
