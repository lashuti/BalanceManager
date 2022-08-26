using BalanceManager.Domain.Enums;
using BalanceManager.Persistence.Abstractions;
using Balances;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v/{version:apiVersion}/[controller]")]
    public class WithdrawController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public WithdrawController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> Withdraw(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Withdraw);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> WithdrawV2(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Withdraw);
        }
    }
}
