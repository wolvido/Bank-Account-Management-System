using BmsKhameleon.Core.Domain.IdentityEntities;

namespace BmsKhameleon.UI.ViewModels
{
    public class UserRoleViewModel
    {
        public required ApplicationUser User { get; set; }
        public required string UserRole { get; set; }
    }
}
