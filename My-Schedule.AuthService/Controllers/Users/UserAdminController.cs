using Microsoft.AspNetCore.Mvc;
using SecureLogin.Attributes;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Services.ApplicationUsers;

namespace My_Schedule.AuthService.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.Admin, UserRoleType.MasterAdmin)]
    public class UserAdminController : ControllerBase
    {
        private readonly UserService _userService;

        public UserAdminController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
