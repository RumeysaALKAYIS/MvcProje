using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class LoginViewModel
    {
      //  [Display(Name ="Kullanıcı Adı",Prompt ="placesholder")]
        [Required(ErrorMessage ="Username is required.")]
        [StringLength(30,ErrorMessage ="Username can be max 30 xharecters.")]
        public string Username { get; set; }

       // [DataType(DataType.Password)]//pasword .... yazıyo
        [Required(ErrorMessage ="Password is required")]
        [MinLength(6,ErrorMessage ="Password can be min 6 charecters.")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 charecters.")]
        public string Password { get; set; }

    }
}
