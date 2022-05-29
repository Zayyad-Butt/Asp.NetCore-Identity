using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_IS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20,ErrorMessage ="Length should not exceed 20 characters")]
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
