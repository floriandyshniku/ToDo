

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = Todo.Request.LoginRequest;

namespace Todo.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthService> _logger; 
        private readonly IConfiguration config; 

        public AuthService(UserManager<IdentityUser> userManager, ILogger<AuthService> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger; 
            config = configuration;
        }
        public async Task<bool> Login(LoginRequest request)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Username);
            if (identityUser is null)
            {
                _logger.LogWarning($"Login failed: User not found for email {request.Username}");
                return false;
            }
            var checkPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!checkPassword)
            {
                _logger.LogWarning($"Login failed: Incorrect password for user {identityUser.PasswordHash}, {request.Password}");
            }
            else
            {
                _logger.LogInformation($"Login successful for user: {request.Username}");
            }

            return checkPassword;
        }

        public async Task<(bool Succeeded, IEnumerable<IdentityError> ErrorMessage)> Register(IdentityUser user, string password)
        {
            var result  = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, result.Errors);
        }

        public string GenerateTokenString(LoginRequest request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Username),
                new Claim(ClaimTypes.Role, "user"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B623D7FB-DB87-44B4-B597-5F65C6EF791B18DAE64D-B3DD-49CF-BF9C-53F72A2F252B"));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddMinutes(60),
                issuer:config.GetSection("Jwt:Issuer").Value,
                audience: config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
