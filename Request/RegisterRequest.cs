using System.Text.Json;

namespace Todo.Request
{
    public class RegisterRequest
    {
        public string FirstName {  get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        JsonProperty("Password")
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
