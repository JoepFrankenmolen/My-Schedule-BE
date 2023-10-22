using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.Shared.Services.Tokens
{
    public class TokenStatusService : ITokenStatusService
    {
        private readonly IDefaultContextBuilder _defaultContextBuilder;

        public TokenStatusService(IDefaultContextBuilder defaultContextBuilder)
        {
            _defaultContextBuilder = defaultContextBuilder ?? throw new ArgumentNullException(nameof(defaultContextBuilder));
        }

        public async Task CreateTokenStatus<T>(ITokenStatus tokenStatus) where T : DbContext, ITokenStatusContext
        {
            _ = tokenStatus ?? throw new ArgumentNullException(nameof(tokenStatus));

            using (var context = _defaultContextBuilder.CreateContext<T>())
            {
                var doesTokenStatusExist = context.TokenStatus.Any(t => t.Id == tokenStatus.Id);

                if (!doesTokenStatusExist)
                {
                    context.TokenStatus.Add(tokenStatus as TokenStatus);
                    await context.SaveChangesAsync();
                }
            }
            
        }
    }
}
