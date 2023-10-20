using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.Core.Interfaces;
using My_Schedule.Shared.DTO.Users;
using My_Schedule.UserService.Core;
using My_Schedule.UserService.Services.Users.Helpers;

namespace My_Schedule.UserService.Services.Users
{
    public class UserFetchingService
    {
        private readonly UserServiceContext _dbContext;
        private readonly UserHelper _userHelper;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserFetchingService(UserServiceContext dbContext, UserHelper userHelper, IUserAuthenticationContext userAuthenticationContext)
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
    }
}