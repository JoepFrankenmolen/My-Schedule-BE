using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users
{
    public class UserCreationService
    {
        private readonly UserServiceContext _dbContext;
        private readonly UserRoleService _userRoleService;

        public UserCreationService(UserServiceContext dbContext, UserRoleService userRoleService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
        }

        /*public async Task<User> CreateUser(RegisterDTO registerDTO, HashDTO hashDTO)
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
        }*/
    }
}