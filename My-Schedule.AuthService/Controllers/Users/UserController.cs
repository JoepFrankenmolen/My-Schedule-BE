using Microsoft.AspNetCore.Mvc;
using SecureLogin.Attributes;
using SecureLogin.Data.Models.ApplicationUser;
using SecureLogin.Services.Services.ApplicationUsers;

namespace My_Schedule.AuthService.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.User)]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            try
            {
                var result = await _userService.GetCurrentLoggedInUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
