using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.Shared.Helpers;

namespace My_Schedule.AuthService.Services.Logs
{
    public class ConfirmationLogService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientDetailService _clientDetailService;

        public ConfirmationLogService(AuthServiceContext dbContext, IHttpContextAccessor httpContextAccessor, ClientDetailService clientDetailService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _clientDetailService = clientDetailService ?? throw new ArgumentNullException(nameof(clientDetailService));
        }

        public async Task CreateConfirmationLog(Guid userId, Guid confirmationId, bool attemptFailed)
        {
            var httpContextDTO = HttpContextHelper.GetContextDetails(_httpContextAccessor.HttpContext);

            var ipAddress = httpContextDTO.IPAddress;
            var userAgent = httpContextDTO.UserAgent;

            var clientDetailsId = await _clientDetailService.AddOrCreateClientDetails(ipAddress, userAgent);

            var confirmationLog = new ConfirmationLog
            {
                UserId = userId,
                confirmationId = confirmationId,
                ClientDetailsId = clientDetailsId,
                AttemptFailed = attemptFailed,
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _dbContext.ConfirmationLogs.AddAsync(confirmationLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
