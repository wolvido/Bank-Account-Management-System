using BmsKhameleon.Core.DTO.AccountDTOs;

namespace BmsKhameleon.UI.ViewModels
{
    public class AccountViewModel
    {
        public AccountCreateRequest? AccountCreateRequest { get; set; }
        public AccountUpdateRequest? AccountUpdateRequest { get; set; }
        public List<AccountResponse>? AccountResponses { get; set; }
    }
}
