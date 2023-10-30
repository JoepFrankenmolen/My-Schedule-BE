using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IUserAuthDetailContext : IContextBase, IUserContext
    {
        DbSet<UserAuthDetail> UserAuthDetails { get; set; }
    }
}