using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserRequestDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("Username already exists!");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(result);
        }
    }
}
