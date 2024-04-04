using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Todo.AppData;
using Todo.Request;

namespace Todo.Service
{
    public interface IAuthService
    {
        Task<bool> Login(LoginRequest request);
        public  Task<(bool Succeeded, IEnumerable<IdentityError> ErrorMessage)> Register(IdentityUser user, string password);
        public string GenerateTokenString(LoginRequest request);
    }
}