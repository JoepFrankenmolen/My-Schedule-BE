using Microsoft.AspNetCore.Mvc;
using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.DTO.Users;
using My_Schedule.Shared.Interfaces.Interfaces;
using My_Schedule.Shared.Models.Users;
using My_Schedule.Shared.Services.Users.Interfaces;
using My_Schedule.Shared.Services.Users.Users;
using My_Schedule.UserService.Core;

namespace My_Schedule.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.User)]
    public class UserController : ControllerBase
    {
        private readonly IUserDeleteService _userDeleteService;
        private readonly IUserAuthenticationContext _userAuthenticationContext;
        private readonly IUserUpdateService _userUpdateService;
        private readonly UserServiceContext _dbContext;

        public UserController(
            IUserDeleteService userDeleteService,
            IUserAuthenticationContext userAuthenticationContext,
            IUserUpdateService userUpdateService,
            UserServiceContext dbContext)
        {
            _userUpdateService = userUpdateService ?? throw new ArgumentNullException(nameof(userUpdateService));
            _userDeleteService = userDeleteService ?? throw new ArgumentNullException(nameof(userDeleteService));
            _userAuthenticationContext = userAuthenticationContext ?? throw new ArgumentNullException(nameof(userAuthenticationContext));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            try
            {
                var userId = _userAuthenticationContext.UserId;
                var user = await UserFetcherService.GetUserById(userId, _dbContext);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCurrentLoggedInUser(UserIdentityDTO userIdentityDTO)
        {
            try
            {
                var userId = _userAuthenticationContext.UserId;
                var result = await _userUpdateService.IdentityUpdate(userId, userIdentityDTO, _dbContext);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCurrentLoggedInUser()
        {
            try
            {
                var userId = _userAuthenticationContext.UserId;
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                await _userDeleteService.DeleteUser(userId, timestamp, _dbContext);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}