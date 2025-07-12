using Microsoft.AspNetCore.Mvc;
using Pet.Core.Application.DTOs;
using Pet.Core.Application.Infrastructure;

namespace Pet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.Authenticate(dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var result = await _authService.ChangePassword(userId, oldPassword, newPassword);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(int userId, string newPassword)
        {
            var result = await _authService.ResetPassword(userId, newPassword);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }
    }
}
