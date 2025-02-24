﻿using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Claims;
using BmsKhameleon.Core.Domain.IdentityEntities;
using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BmsKhameleon.UI.Controllers
{
    [Authorize]
    public class HomeController(IAccountsService accountsService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IAccountsService _accountsService = accountsService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index(string? bankFilter, string? searchString, string? sortBy, SortOrderOptions? sortOrder)
        {
            var banks = await _accountsService.GetAllAccountBanks();
            banks.Insert(0, "Any Bank");

            ViewBag.Banks = banks;
            ViewBag.BankFilter = bankFilter ?? "Any Bank";
            ViewBag.SortBy = sortBy ?? string.Empty;
            ViewBag.SortOrder = sortOrder ?? SortOrderOptions.Ascending;

            List<AccountResponse> accounts = await _accountsService.GetAllAccounts();

            //use search if searchString is not null
            if (searchString != null)
            {
                if (bankFilter == "Any Bank")
                {
                    bankFilter = null;
                }

                accounts = await _accountsService.GetFilteredAccounts(searchString, bankFilter);
            }

            //filter banks if bankFilter is not null
            if (bankFilter != null && bankFilter != "Any Bank")
            {
                accounts = accounts.Where(account => account.BankName == bankFilter).ToList();
                banks.Remove(bankFilter);
                banks.Insert(0, bankFilter);
            }

            // sort
            if (sortBy != null && sortOrder != null)
            {
                accounts = await _accountsService.SortAccounts(accounts, sortBy, (SortOrderOptions)sortOrder);
            }

            //get the current logged in user
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest("No user logged in");
            }

            accounts = _accountsService.SortAccountsByUser(accounts, user.Id);

            AccountViewModel accountViewModel = new AccountViewModel
            {
                AccountResponses = accounts
            };

            //get the local machine ipv4 address
            ViewBag.IpAddress = GetLocalIpAddresses();

            //this action method is a fat bitch. Refactor and use a viewmodel.
            return View(accountViewModel);
        }

        //this method should NOT BE IN THIS CONTROLLER. Maybe move to a viewmodel or something.
        private static List<(string IpAddress, string ConnectionType)> GetLocalIpAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            //list of tuple to store the ip and connection type
            var ipv4List = new List<(string IpAddress, string ConnectionType)>();

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string connectionType = NetworkInterface.GetAllNetworkInterfaces()
                        .Where(ni => ni.GetIPProperties().UnicastAddresses
                        .Any(ua => ua.Address.Equals(ip)))
                        .Select(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ? "WiFi" : "Ethernet")
                        .FirstOrDefault() ?? "Unknown";

                    ipv4List.Add((ip.ToString(), connectionType));
                }
            }

            if (ipv4List.Count <= 0)
            {
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }

            return ipv4List;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(AccountCreateRequest? accountCreateRequest)
        {
            if (accountCreateRequest == null)
            {
                return BadRequest("Invalid account request.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("No user logged id");
            }
            accountCreateRequest.ApplicationUserId = user.Id;

            bool result = await _accountsService.CreateAccount(accountCreateRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the account. Please try again.");
            }

            return RedirectToAction("Index");
        }

        [Route("[action]/{accountId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteBankAccount(Guid accountId)
        {
            var existingAccount = await _accountsService.GetAccountById(accountId);
            if (existingAccount == null)
            {
                return BadRequest("Account already deleted or not found");
            }

            bool result = await _accountsService.DeleteAccount(accountId);

            if (result == false)
            {
                return BadRequest("Unable to delete the account. Please try again.");
            }

            return RedirectToAction("Index");
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBankAccount(AccountUpdateRequest? accountUpdateRequest)
        {
            if (accountUpdateRequest == null)
            {
                return BadRequest("Invalid account request.");
            }

            bool result = await _accountsService.UpdateAccount(accountUpdateRequest);

            if (result == false)
            {
                return BadRequest("Unable to update the account. Please contact site manager.");
            }

            return RedirectToAction("Index");
        }

        [Route("[action]/{accountId}")]
        [HttpGet]
        public async Task<IActionResult> UpdateBankAccountPartial(Guid accountId)
        {

            AccountResponse? account = await _accountsService.GetAccountById(accountId);

            if (account == null)
            {
                return BadRequest("Account not found.");
            }

            if (account.DateEnrolled == null)
            {
                throw new Exception("Date enrolled is null.");
            }

            var dateEnrolled = account.DateEnrolled.Value;
            var dateEnrolledFormatted = dateEnrolled.ToString("MMMM d, yyyy");

            ViewBag.DateEnrolled = dateEnrolledFormatted;

            AccountUpdateRequest accountUpdateRequest = new AccountUpdateRequest
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                BankBranch = account.BankBranch,
                InitialBalance = account.InitialBalance,
                Visibility = account.Visibility
            };

            return PartialView("~/Views/Shared/AccountForms/_UpdateAccountPartialView.cshtml", accountUpdateRequest);
        }

    }
}
