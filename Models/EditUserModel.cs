using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class EditUserModel 
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 xharecters.")]
        public string Username { get; set; }

        public bool Locked { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";

        public string NameSurname { get; set; }



    }
}
