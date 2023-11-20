using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.Shared.Helpers;
using My_Schedule.Shared.Services;

namespace My_Schedule.AuthService.Services.Logs
{
    public class LoginLogService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientDetailService _clientDetailService;

        public LoginLogService(AuthServiceContext dbContext, IHttpContextAccessor httpContextAccessor, ClientDetailService clientDetailService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _clientDetailService = clientDetailService ?? throw new ArgumentNullException(nameof(clientDetailService));
        }

        public async Task CreateLoginLog(string email, bool attemptFailed)
        {
            var httpContextDTO = HttpContextHelper.GetContextDetails(_httpContextAccessor.HttpContext);

            var ipAddress = httpContextDTO.IPAddress;
            var userAgent = httpContextDTO.UserAgent;

            var clientDetailsId = await _clientDetailService.AddOrCreateClientDetails(ipAddress, userAgent);

            var loginLog = new LoginLog
            {
                Email = email,
                ClientDetailsId = clientDetailsId,
                AttemptFailed = attemptFailed,
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _dbContext.LoginLogs.AddAsync(loginLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}