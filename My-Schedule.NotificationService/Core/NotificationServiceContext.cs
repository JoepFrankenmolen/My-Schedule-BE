using Microsoft.EntityFrameworkCore;
using My_Schedule.NotificationService.Models;
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

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }
        public DbSet<NotificationUserPreference> NotificationUserPreferences { get; set; }

        // todo: logs

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}