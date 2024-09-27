using BmsKhameleon.Core.Domain.IdentityEntities;

namespace BmsKhameleon.UI.ViewModels
{
    public class AdminViewModel
    {
        public RegisterViewModel? RegisterDTO { get; set; }
        public List<ApplicationUser>? Users { get; set; }
    }
}
