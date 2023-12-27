using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly IUserSecurityCreateService _userSecurityCreateService;

        public UserService(AuthServiceContext dbContext, IUserSecurityCreateService userSecurityCreateService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userSecurityCreateService = userSecurityCreateService ?? throw new ArgumentNullException(nameof(userSecurityCreateService));
        }

        public async Task<User> CreateUser(RegisterDTO registerDTO, HashDTO hashDTO)
        {
            // setup user
            var user = new User
            {
                Id = Guid.NewGuid(),
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                IsEmailConfirmed = false,
                IsBlocked = false,
                TokenRevocationTimestamp = 0,
                LastLoginTimestamp = 0,
                FailedLoginAttempts = 0,
                LoginCount = 0,
                Roles = new List<UserRole>()
            };

            // create role
            user.Roles.Add(await CreateBasicRole(user.Id, UserRoleType.User));

            // create security object
            var userSecurity = new UserSecurity
            {
                UserId = user.Id,
                User = user,
                PasswordHash = hashDTO.PasswordHash,
                Salt = hashDTO.Salt,
            };

            userSecurity = await _userSecurityCreateService.CreateUserSecurity(userSecurity, _dbContext);
            return userSecurity.User;
        }

        private async Task<UserRole> CreateBasicRole(Guid userId, UserRoleType type)
        {
            return new UserRole { Role = type, UserId = userId };
        }
    }
}