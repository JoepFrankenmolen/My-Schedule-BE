using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Users;
using My_Schedule.AuthService.DTO;
using My_Schedule.AuthService.Models.Users;
using My_Schedule.AuthService.Models;
using My_Schedule.Shared.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.AuthService.Core;
using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Models.Users.UserInterfaces;

namespace My_Schedule.AuthService.Services.Users
{
    public class UserService
    {
        private readonly AuthServiceContext _dbContext;
        private readonly UserHelper _userHelper;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserService(AuthServiceContext dbContext, UserHelper userHelper, IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = await _dbContext.Users.Include(u => u.Roles).ToListAsync(); // Retrieve all users from the database

            // Create a list of UserDTO objects to store the mapped user data
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    CreationTimestamp = user.CreationTimestamp,
                    UserName = user.UserName,
                    Email = user.Email,
                    LastLoggedInTimeStamp = user.LastLoginTimestamp,
                    Roles = user.Roles
                };

                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        public async Task<UserDTO> GetCurrentLoggedInUser()
        {
            try
            {
                var user = await _userHelper.GetUserById(_userAuthenticationContext.UserId);

                return new UserDTO
                {
                    Id = user.Id,
                    CreationTimestamp = user.CreationTimestamp,
                    UserName = user.UserName,
                    Email = user.Email,
                    LastLoggedInTimeStamp = user.LastLoginTimestamp,
                    Roles = user.Roles
                };
            }
            catch (Exception ex)
            {
                return null;
            }
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
