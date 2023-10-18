using Microsoft.AspNetCore.Mvc;
using My_Schedule.AuthService.DTO.Tokens;
using My_Schedule.AuthService.Services.Auth.Tokens;

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