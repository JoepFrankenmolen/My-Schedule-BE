using Microsoft.AspNetCore.Mvc;
using My_Schedule.UserService.Services.Users;

namespace My_Schedule.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAdminController : ControllerBase
    {
        private readonly UserAdminFetchingService _userFetchingService;
        private readonly UserAdminService _userAdminService;

        public UserAdminController(UserAdminFetchingService userFetchingService, UserAdminService userAdminService)
        {
            _userFetchingService = userFetchingService ?? throw new ArgumentNullException(nameof(userFetchingService));
            _userAdminService = userAdminService ?? throw new ArgumentNullException(nameof(userAdminService));
        }

        [HttpPut("Create")]
        public async Task<IActionResult> CreateUser(string userId, bool state)
        {
            try
            {
                await _userAdminService.BanUser(userId, state);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Ban")]
        public async Task<IActionResult> BanUser(string userId, bool state)
        {
            try
            {
                await _userAdminService.BanUser(userId, state);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Block")]
        public async Task<IActionResult> BLockUser(string userId, bool state)
        {
            try
            {
                await _userAdminService.BlockUser(userId, state);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("Get/All")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userFetchingService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await _userAdminService.DeleteUser(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}