using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Models;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.AuthService.Models.PasswordReset;
using My_Schedule.AuthService.Models.Tokens;

namespace My_Schedule.AuthService.Core
{
    public class AuthServiceContext : DbContext
    {
        public AuthServiceContext(DbContextOptions<AuthServiceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<ConfirmationLog> ConfirmationLogs { get; set; }
        public DbSet<TokenSession> TokenSessions { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<PasswordReset> passwordResets { get; set; }
    }
}
