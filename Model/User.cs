using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Todo.Model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }
}
