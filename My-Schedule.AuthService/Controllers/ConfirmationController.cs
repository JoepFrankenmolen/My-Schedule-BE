using Microsoft.AspNetCore.Mvc;
using SecureLogin.Data.DTO.Auth.Authentication;
using SecureLogin.Services.Services.Auth.Confirmations;

namespace My_Schedule.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfirmationController : ControllerBase
    {
        private readonly ConfirmationDispatcher _confirmationDispatcher;

        public ConfirmationController(ConfirmationDispatcher confirmationDispatcher)
        {
            _confirmationDispatcher = confirmationDispatcher ?? throw new ArgumentNullException(nameof(confirmationDispatcher));
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirmation([FromBody] ConfirmDTO confirmDTO)
        {
            _ = confirmDTO ?? throw new ArgumentNullException(nameof(confirmDTO));

            try
            {
                var data = await _confirmationDispatcher.DispatchConfirmation(confirmDTO);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
