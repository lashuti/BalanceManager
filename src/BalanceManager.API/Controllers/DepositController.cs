using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Balances;
using System.Threading.Tasks;
using BalanceManager.Persistence.Abstractions;
using BalanceManager.Domain.Enums;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v/{version:apiVersion}/[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public DepositController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet("{transactionId}/{amount}")]
        public ActionResult<ErrorCode> Deposit(decimal amount, string transactionId)
        {
            return _balanceService.TransferBalance(amount, transactionId, OperationType.Deposit);
        }
    }
}
