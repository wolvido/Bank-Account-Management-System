using System.Globalization;
using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BmsKhameleon.UI.Controllers
{
    public class TransactionsOverviewController(ITransactionsService transactionsService) : Controller
    {
        private readonly ITransactionsService _transactionsService = transactionsService;

        [Route("[action]/{accountId}/{date}")]
        public IActionResult TransactionsOverview(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return View();
        }


    }
}
