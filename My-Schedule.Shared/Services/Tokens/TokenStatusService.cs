using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.Services.Tokens.Interfaces;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.Shared.Services.Tokens
{
    public class TokenStatusService : ITokenStatusService
    {

        public async Task CreateTokenStatus(ITokenStatus tokenStatus, ITokenStatusContext context)
        {
            _ = tokenStatus ?? throw new ArgumentNullException(nameof(tokenStatus));

            var doesTokenStatusExist = context.TokenStatus.Any(t => t.Id == tokenStatus.Id);

            if (!doesTokenStatusExist)
            {
                context.TokenStatus.Add(tokenStatus as TokenStatus);
                await context.SaveChangesAsync();
            }

            
        }
    }
}
