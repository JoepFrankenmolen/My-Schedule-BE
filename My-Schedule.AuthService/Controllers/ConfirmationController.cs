using Microsoft.AspNetCore.Mvc;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.Services.Confirmations;

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

        /// <summary>
        /// EmailConfirmation = 0,
        /// LoginVerification = 1,
        /// PasswordReset = 2,
        /// UnBlock = 3,
        /// </summary>
        /// <param name="confirmDTO"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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