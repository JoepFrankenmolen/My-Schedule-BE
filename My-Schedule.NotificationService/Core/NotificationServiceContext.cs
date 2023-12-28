using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.NotificationService.Core
{
    public class NotificationServiceContext : DbContext, IUserContext, ITokenStatusContext
    {
        public NotificationServiceContext(DbContextOptions<NotificationServiceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TokenStatus> TokenStatus { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}