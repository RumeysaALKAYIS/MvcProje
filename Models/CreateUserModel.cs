using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class CreateUserModel
    {

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 xharecters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 charecters.")]
        public string Password { get; set; }

        public bool Locked { get; set; } 

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";

        public string NameSurname { get; set; }
        

        [Required(ErrorMessage = "Repassword is required")]
        [MinLength(6, ErrorMessage = "Repassword can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "Repassword can be max 16 charecters.")]
        [Compare(nameof(Password))] //Karşılaştır]
        public string RePassword { get; set; }

    }
}
