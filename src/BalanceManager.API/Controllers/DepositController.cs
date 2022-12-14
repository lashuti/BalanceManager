using Microsoft.AspNetCore.Mvc;
using System;
using Balances;
using BalanceManager.Application.Abstractions;
using BalanceManager.Domain.Enums;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public DepositController(IBalanceService balanceService)
        {
            _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        }

        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> Deposit(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Deposit);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> DepositV2(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalanceV2(amount, transactionId, OperationType.Withdraw);
        }
    }
}
