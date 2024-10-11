using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
