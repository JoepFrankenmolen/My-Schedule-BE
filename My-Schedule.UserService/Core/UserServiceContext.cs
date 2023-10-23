using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Interfaces.Context;
using My_Schedule.Shared.Models.ClientDetails;
using My_Schedule.Shared.Models.Tokens;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.UserService.Core
{
    public class UserServiceContext : DbContext, ITokenStatusContext
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TokenStatus> TokenStatus { get; set; }
        public DbSet<ClientDetails> ClientDetails { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

/*            // Kinda unstable but 
            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("Users");
                u.Property(e => e.Email).HasColumnName("Email");
                u.Property(e => e.UserName).HasColumnName("UserName");
                u.Property(e => e.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
                u.Property(e => e.PasswordHash).HasColumnName("PasswordHash");
                u.Property(e => e.Salt).HasColumnName("Salt");
                u.Property(e => e.FailedLoginAttempts).HasColumnName("FailedLoginAttempts");
                u.Property(e => e.IsBlocked).HasColumnName("IsBlocked");
                u.Property(e => e.IsBanned).HasColumnName("IsBanned");
                u.Property(e => e.IsEmailConfirmed).HasColumnName("IsEmailConfirmed");
                u.Property(e => e.TokenRevocationTimestamp).HasColumnName("TokenRevocationTimestamp");
                u.Property(e => e.LastLoginTimestamp).HasColumnName("LastLoginTimestamp");
                u.Property(e => e.LoginCount).HasColumnName("LoginCount");
            });

            modelBuilder.Entity<UserBasic>(u =>
            {
                u.ToTable("Users");
                u.Property(e => e.Email).HasColumnName("Email");
                u.Property(e => e.UserName).HasColumnName("UserName");
                u.Property(e => e.IsBlocked).HasColumnName("IsBlocked");
                u.Property(e => e.IsBanned).HasColumnName("IsBanned");
                u.Property(e => e.IsEmailConfirmed).HasColumnName("IsEmailConfirmed");
                u.Property(e => e.TokenRevocationTimestamp).HasColumnName("TokenRevocationTimestamp");
                u.HasOne<User>().WithOne().HasForeignKey<UserBasic>(e => e.Id);
            });*/
        }
    }
}