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
     //   public DbSet<UserBasic> UserBasics { get; set; }
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

            // Configure both User and UserBasic to use the same table
   /*         modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserBasic>().ToTable("User");*/
        }
    }
}