using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    public class CalendarController(ITransactionsService transactionsService) : Controller
    {
        private readonly ITransactionsService _transactionsService = transactionsService;

        [HttpGet]
        [Route("[action]/{accountId}/{date?}")]
        public async Task<IActionResult> Calendar(Guid accountId, DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = await _transactionsService.GetMonthlyTransactionsAggregate(date.Value, accountId);

            return View();
        }
    }
}
