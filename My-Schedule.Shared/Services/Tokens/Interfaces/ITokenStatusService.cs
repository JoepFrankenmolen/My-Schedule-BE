using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.Shared.Services.Tokens.Interfaces
{
    public interface ITokenStatusService
    {
        Task CreateTokenStatus(ITokenStatus tokenStatus, ITokenStatusContext tokenStatusContext);
    }
}