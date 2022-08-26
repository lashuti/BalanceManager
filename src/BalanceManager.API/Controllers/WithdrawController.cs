using BalanceManager.Domain.Enums;
using BalanceManager.Application.Abstractions;
using Balances;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class WithdrawController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public WithdrawController(IBalanceService balanceService)
        {
            _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        }

        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> Withdraw(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Withdraw);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> WithdrawV2(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalanceV2(amount, transactionId, OperationType.Withdraw);
        }
    }
}
