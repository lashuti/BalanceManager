using BalanceManager.Domain.Enums;
using BalanceManager.Persistence.Abstractions;
using Balances;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WithdrawController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public WithdrawController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> Withdraw(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Withdraw);
        }
    }
}
