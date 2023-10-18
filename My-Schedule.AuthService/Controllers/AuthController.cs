using Microsoft.AspNetCore.Mvc;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.Services.Auth.Authentication;
using My_Schedule.AuthService.Services.Authentication;
using My_Schedule.AuthService.Services.Confirmations;

namespace My_Schedule.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly PasswordResetService _passwordResetService;
        private readonly RegisterService _registerService;
        private readonly LogoutService _logoutService;

        public AuthController(PasswordResetService passwordResetService, LoginService loginService, RegisterService registerService, LogoutService logoutService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _passwordResetService = passwordResetService ?? throw new Exception(nameof(passwordResetService));
            _logoutService = logoutService ?? throw new ArgumentNullException(nameof(logoutService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDTO credentialsDTO)
        {
            _ = credentialsDTO ?? throw new ArgumentNullException(nameof(credentialsDTO));

            try
            {
                var result = await _loginService.Login(credentialsDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (await _logoutService.Logout())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            _ = registerDTO ?? throw new ArgumentNullException(nameof(registerDTO));

            try
            {
                var result = await _registerService.register(registerDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] CredentialsDTO credentialsDTO)
        {
            _ = credentialsDTO ?? throw new ArgumentNullException(nameof(credentialsDTO));

            try
            {
                /*var result =*/
                await _passwordResetService.CreatePasswordReset(credentialsDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}