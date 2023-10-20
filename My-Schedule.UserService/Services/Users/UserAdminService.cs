using Microsoft.EntityFrameworkCore;
using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.RabbitMQ.Producers;
using My_Schedule.UserService.Core;
using My_Schedule.UserService.Services.Users.Helpers;

namespace My_Schedule.UserService.Services.Users
{
    public class UserAdminService
    {
        private readonly UserServiceContext _dbContext;
        private readonly UserHelper _userHelper;
        private readonly UserProducer _userProducer;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserAdminService(UserServiceContext dbContext, UserHelper userHelper, UserProducer userProducer, IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _userProducer = userProducer ?? throw new ArgumentNullException(nameof(userProducer));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task BanUser(string userId, bool sendMessage = true)
        {
            var user = await _userHelper.GetUserById(Guid.Parse(userId));

            if (user == null)
            {
                throw new ArgumentNullException("userId failed to fetch");
            }

            user.IsBanned = true;

            if (sendMessage)
            {
                await _userProducer.SendUserBannedMessage(user.Id, true);
            }

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
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
