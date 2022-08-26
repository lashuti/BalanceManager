using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Balances;
using System.Threading.Tasks;
using BalanceManager.Persistence.Abstractions;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            return _balanceService.TransferBalance(amount, transactionId);
        }
    }
}
