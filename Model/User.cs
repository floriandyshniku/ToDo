using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Todo.Model
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters.")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must contain only letters.")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public List<ToDo> ToDoItems { get; set; } = [];

    }
}
