using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Tokens;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface ITokenStatusContext : IContextBase
    {
        DbSet<TokenStatus> TokenStatus { get; set; }
    }
}