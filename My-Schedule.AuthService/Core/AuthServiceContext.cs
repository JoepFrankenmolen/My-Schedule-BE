using Microsoft.EntityFrameworkCore;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Models.Logs;
using My_Schedule.AuthService.Models.PasswordReset;
using My_Schedule.AuthService.Models.Tokens;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.ClientDetails;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.AuthService.Core
{
    public class AuthServiceContext : DbContext, IUserSecurityContext, IUserContext, IClientDetailsContext
    {
        public AuthServiceContext(DbContextOptions<AuthServiceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSecurity> UserSecurities { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<ConfirmationLog> ConfirmationLogs { get; set; }
        public DbSet<TokenSession> TokenSessions { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<ClientDetails> ClientDetails { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}