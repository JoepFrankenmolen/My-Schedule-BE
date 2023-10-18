using Microsoft.AspNetCore.Mvc;
using SecureLogin.Data.DTO.Auth.Tokens;
using SecureLogin.Services.Services.Auth.Tokens;

namespace My_Schedule.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("refresh")]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefreshAccessToken([FromBody] AccessTokenDTO accessTokenDTO)
        {
            _ = accessTokenDTO ?? throw new ArgumentNullException(nameof(accessTokenDTO));

            try
            {
                var result = await _tokenService.RefreshAccessToken(accessTokenDTO.RefreshToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
