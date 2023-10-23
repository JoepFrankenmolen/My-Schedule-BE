using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.Shared.Interfaces.Context
{
    public interface IUserContext : IContextBase
    {
        DbSet<User> Users{ get; set; }
    }
}