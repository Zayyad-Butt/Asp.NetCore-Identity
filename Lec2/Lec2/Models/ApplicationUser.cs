using Microsoft.AspNetCore.Identity;

namespace Lec2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Gender { get; set; }
    }
}
