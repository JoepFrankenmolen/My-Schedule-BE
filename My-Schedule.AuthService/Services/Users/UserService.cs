using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.Shared.Core.Interfaces;
using My_Schedule.Shared.Models.Users;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserService(AuthServiceContext dbContext, IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task<User> CreateUser(RegisterDTO registerDTO, HashDTO hashDTO)
        {
            var user = new User
            {
                Id = new Guid(),
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                IsEmailConfirmed = false,
                TwoFactorEnabled = true,
                PasswordHash = hashDTO.PasswordHash,
                Salt = hashDTO.Salt,
                IsBlocked = false,
                FailedLoginAttempts = 0,
                TokenRevocationTimestamp = 0,
                Roles = new List<UserRole>()
            };

            // create role
            user.Roles.Add(await CreateBasicRole(user.Id, UserRoleType.User));

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        private async Task<UserRole> CreateBasicRole(Guid userId, UserRoleType type)
        {
            return new UserRole { Role = type, UserId = userId };
        }
    }
}