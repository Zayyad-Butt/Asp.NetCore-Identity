using System.ComponentModel.DataAnnotations;

namespace Lec2.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }

    }
}
