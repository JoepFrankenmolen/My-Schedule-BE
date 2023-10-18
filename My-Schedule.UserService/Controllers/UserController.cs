using Microsoft.AspNetCore.Mvc;
using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.Models.Users;
using My_Schedule.UserService.Services.Users;

namespace My_Schedule.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.User)]
    public class UserController : ControllerBase
    {
        private readonly UserFetchingService _userFetchingService;

        public UserController(UserFetchingService userFetchingService)
        {
            _userFetchingService = userFetchingService ?? throw new ArgumentNullException(nameof(userFetchingService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            try
            {
                var result = await _userFetchingService.GetCurrentLoggedInUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}