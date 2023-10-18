using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.Shared.Helpers;

namespace My_Schedule.AuthService.Services.Logs
{
    public class ConfirmationLogService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfirmationLogService(AuthServiceContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task CreateConfirmationLog(Guid userId, Guid confirmationId, bool attemptFailed)
        {
            var httpContextDTO = HttpContextHelper.GetContextDetails(_httpContextAccessor.HttpContext);

            var confirmationLog = new ConfirmationLog
            {
                UserId = userId,
                confirmationId = confirmationId,
                IPAddress = httpContextDTO.IPAddress ?? "Undefined",
                UserAgent = httpContextDTO.UserAgent,
                AttemptFailed = attemptFailed,
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _dbContext.ConfirmationLogs.AddAsync(confirmationLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
