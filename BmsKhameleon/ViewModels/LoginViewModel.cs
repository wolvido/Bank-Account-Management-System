using System.ComponentModel.DataAnnotations;

namespace BmsKhameleon.UI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
