using System.ComponentModel.DataAnnotations;

namespace Lec1.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Length should not exceed 20 characters")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match with confirmation password")]
        public string ConfirmPassword { get; set; }
    }
}
