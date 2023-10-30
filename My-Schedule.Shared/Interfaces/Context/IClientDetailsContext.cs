using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.ClientDetails;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IClientDetailsContext : IContextBase
    {
        DbSet<ClientDetails> ClientDetails { get; set; }
    }
}