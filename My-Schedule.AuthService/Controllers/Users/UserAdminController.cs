using Microsoft.AspNetCore.Mvc;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.Models.Users;

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