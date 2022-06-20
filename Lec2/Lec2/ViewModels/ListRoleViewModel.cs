using System.ComponentModel.DataAnnotations;

namespace Lec2.ViewModels
{

    public class ListRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }


}
