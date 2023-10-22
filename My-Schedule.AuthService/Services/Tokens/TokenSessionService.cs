using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.Tokens;
using My_Schedule.Shared.Helpers;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.Shared.Services.Tokens.Interfaces;

namespace My_Schedule.AuthService.Services.Auth.Tokens
{
    public class TokenSessionService : ITokenSessionValidator
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientDetailService _clientDetailService;
        private readonly TokenProducer _producer;

        public TokenSessionService(
            AuthServiceContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            ClientDetailService clientDetailService,
            TokenProducer tokenProducer)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _clientDetailService = clientDetailService ?? throw new ArgumentNullException(nameof(clientDetailService));
            _producer = tokenProducer ?? throw new ArgumentNullException(nameof(tokenProducer));
        }

        public async Task<Guid> GenerateSession()
        {
            var httpContextDTO = HttpContextHelper.GetContextDetails(_httpContextAccessor.HttpContext);

            var ipAddress = httpContextDTO.IPAddress;
            var userAgent = httpContextDTO.UserAgent;

            var clientDetailsId = await _clientDetailService.AddOrCreateClientDetails(ipAddress, userAgent);

            var tokenSession = new TokenSession
            {
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                SessionId = Guid.NewGuid(),
                ClientDetailsId = clientDetailsId,
                IsBlocked = false,
                BlockedTimestamp = null
            };

            await _dbContext.AddAsync(tokenSession);
            await _dbContext.SaveChangesAsync();

            return tokenSession.SessionId;
        }

        // return true if valid
        public async Task<bool> IsValidSession(Guid sessionId)
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

                await _producer.SendTokenStatusCreatedMessage(session);

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