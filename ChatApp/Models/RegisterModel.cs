using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Enter name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Name or password is incorrect")]

        public string ConfirmPassword { get; set; }
    }
}
