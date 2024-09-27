using System.ComponentModel.DataAnnotations;
using BmsKhameleon.Core.Enums;

namespace BmsKhameleon.UI.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required(ErrorMessage = "User Role is required.")]
        public UserRoleOptions UserRole { get; set; }
    }
}
