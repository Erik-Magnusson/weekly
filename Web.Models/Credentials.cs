using System.ComponentModel.DataAnnotations;


namespace Web.Models.Models
{
    public class Credentials
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]*$", ErrorMessage = "Username contains invalid characters.")]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password needs to be at least 8 characters.")]
        public string Password { get; set; }
    }
}
