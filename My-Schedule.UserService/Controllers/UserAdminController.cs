using Microsoft.AspNetCore.Mvc;
using My_Schedule.Shared.Attributes;
using My_Schedule.Shared.Models.Users;
using My_Schedule.UserService.Services.Users;

namespace My_Schedule.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizedRoles(UserRoleType.Admin, UserRoleType.MasterAdmin)]
    public class UserAdminController : ControllerBase
    {
        private readonly UserFetchingService _userFetchingService;

        public UserAdminController(UserFetchingService userFetchingService)
        {
            _userFetchingService = userFetchingService ?? throw new ArgumentNullException(nameof(userFetchingService));
        }

        [HttpGet]
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
    }
}