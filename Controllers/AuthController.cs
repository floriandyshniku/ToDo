using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.AppData;
using Todo.Model;
using Todo.Request;
using Todo.Service;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IAuthService _authService, UserManager<IdentityUser> _userManager)
        {
            this._authService = _authService;
            this._userManager = _userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid) { 
                return BadRequest();
            }
            if (await _authService.Login(request) )
            {
                var token = _authService.GenerateTokenString(request);

                return Ok(token);
            }
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            IdentityUser identityUser =  new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                CreatedDate = DateTime.UtcNow
            };

            var (Succeeded, ErrorMessage) = await _authService.Register(identityUser, request.Password);

            if (Succeeded)
            {
                return Ok("Registration successful.");
            }
            else
            {
                return BadRequest(ErrorMessage);
            }
        }
    }
}
