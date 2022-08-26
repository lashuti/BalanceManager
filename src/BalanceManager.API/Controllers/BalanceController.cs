using BalanceManager.Persistence.Abstractions;
using Balances;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet]
        public ActionResult<decimal> Get()
        {
            return _balanceService.GetBalance();
        }
    }
}
