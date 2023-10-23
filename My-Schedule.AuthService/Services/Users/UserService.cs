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
        private readonly IUserAuthDetailHelper _authDetailHelper;

        public UserService(AuthServiceContext dbContext, IUserAuthDetailHelper userAuthDetailHelper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _authDetailHelper = userAuthDetailHelper ?? throw new ArgumentNullException(nameof(userAuthDetailHelper));
        }

        public async Task<User> CreateUser(RegisterDTO registerDTO, HashDTO hashDTO)
        {
            var userAuth = new UserAuthDetail
            {
                Id = new Guid(),
                User = new User // check if this also gets saved
                {
                    Id = new Guid(),
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
            userAuth.User.Roles.Add(await CreateBasicRole(userAuth.Id, UserRoleType.User));

            userAuth = await _authDetailHelper.CreateUserAuthDetail(userAuth, _dbContext);
            return userAuth.User;
        }

        private async Task<UserRole> CreateBasicRole(Guid userId, UserRoleType type)
        {
            return new UserRole { Role = type, UserId = userId };
        }
    }
}