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
        private readonly IUserAuthDetailCreateService _userAuthDetailCreateService;

        public UserService(AuthServiceContext dbContext, IUserAuthDetailCreateService userAuthDetailCreateService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userAuthDetailCreateService = userAuthDetailCreateService ?? throw new ArgumentNullException(nameof(userAuthDetailCreateService));
        }

        public async Task<User> CreateUser(RegisterDTO registerDTO, HashDTO hashDTO)
        {
            var userId = Guid.NewGuid();
            var userAuth = new UserAuthDetail
            {
                UserId = userId,
                User = new User
                {
                    Id = userId,
                    CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                    IsEmailConfirmed = false,
                    IsBlocked = false,
                    TokenRevocationTimestamp = 0,
                    Roles = new List<UserRole>()
                },

                TwoFactorEnabled = true,
                PasswordHash = hashDTO.PasswordHash,
                Salt = hashDTO.Salt,
                FailedLoginAttempts = 0,
            };

            // create role
            userAuth.User.Roles.Add(await CreateBasicRole(userAuth.UserId, UserRoleType.User));

            userAuth = await _userAuthDetailCreateService.CreateUserAuthDetail(userAuth, _dbContext);
            return userAuth.User;
        }

        private async Task<UserRole> CreateBasicRole(Guid userId, UserRoleType type)
        {
            return new UserRole { Role = type, UserId = userId };
        }
    }
}