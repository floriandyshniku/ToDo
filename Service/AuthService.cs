

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo.AppData;
using LoginRequest = Todo.Request.LoginRequest;

namespace Todo.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthService> _logger; 
        private readonly IConfiguration config;
        private readonly IOptionsMonitor<JwtOption> options;
        private const string UserRole = "user";

        public AuthService(UserManager<IdentityUser> userManager, 
                                       ILogger<AuthService> logger, 
                                       IConfiguration configuration,
                                       IOptionsMonitor<JwtOption> options)

        {
            _userManager = userManager;
            _logger = logger; 
            config = configuration;
            this.options = options;
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
                new (ClaimTypes.Email, request.Username),
                new (ClaimTypes.Role, UserRole),
            };

            _logger.LogInformation($"Key {options.CurrentValue.Key}");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.CurrentValue.Key));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512Signature);

            _logger.LogInformation($"Key {}");

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddMinutes(60),
                issuer:config.GetSection(options.CurrentValue.Issuer).Value,
                audience: config.GetSection(options.CurrentValue.Audience).Value,
                signingCredentials: signingCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
