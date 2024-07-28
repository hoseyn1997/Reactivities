using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    /*for register user we will get these 4 information from users*/
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.\\d*)(?=.*[a-z])(?=.*[A-Z]).{4,8}$",
            ErrorMessage = "Password must be complex")]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}