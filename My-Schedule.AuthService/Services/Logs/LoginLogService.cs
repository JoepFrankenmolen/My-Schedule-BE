using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.Shared.Helpers;

namespace My_Schedule.AuthService.Services.Logs
{
    public class LoginLogService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginLogService(AuthServiceContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task CreateLoginLog(string email, bool attemptFailed)
        {
            var httpContextDTO = HttpContextHelper.GetContextDetails(_httpContextAccessor.HttpContext);

            var loginLog = new LoginLog
            {
                Email = email,
                IPAddress = httpContextDTO.IPAddress ?? "Undefined",
                UserAgent = httpContextDTO.UserAgent,
                AttemptFailed = attemptFailed,
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _dbContext.LoginLogs.AddAsync(loginLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
