using angular_vega.Core;
using Microsoft.AspNetCore.Mvc;

namespace angular_vega.Controllers
{   
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService,
                              ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request.Email,
                                                          request.FullName,
                                                          request.Password);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request.Email,
                                                       request.Password);

            if (!result.Success)
                return Unauthorized(result);

            var user = await _authService.GetUserByEmailAsync(request.Email);
            var token = _tokenService.GenerateToken(user);

            return Ok(new AuthResult
            {
                Success = true,
                Message = "Login realizado com sucesso!",
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
