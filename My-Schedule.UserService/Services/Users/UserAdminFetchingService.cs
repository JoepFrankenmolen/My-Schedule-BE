﻿using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Services.Users.Users;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Services.Users
{
    public class UserAdminFetchingService
    {
        private readonly UserServiceContext _dbContext;
        private readonly IUserAuthenticationContext _userAuthenticationContext;

        public UserAdminFetchingService(UserServiceContext dbContext, IUserAuthenticationContext userAuthenticationContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(_userAuthenticationContext));
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            // Retrieve all users from the database
            var usersAuthDetails = await UserFetcherService.GetAll(_dbContext);

            // Create a list of UserDTO objects to store the mapped user data
            var userDTOs = new List<UserDTO>();

            foreach (var userAuth in usersAuthDetails)
            {
                var userDTO = UserDTO.MapUserToDTO(userAuth);

                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        public async Task<UserDTO> GetCurrentLoggedInUser()
        {
            try
            {
                var user = await UserFetcherService.GetUserById(_userAuthenticationContext.UserId, _dbContext);

                return UserDTO.MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}