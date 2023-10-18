﻿using Microsoft.EntityFrameworkCore;
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
    }
}