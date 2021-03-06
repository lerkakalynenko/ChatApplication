using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
