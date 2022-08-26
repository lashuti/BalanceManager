using Balances;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        //TODO
        private readonly CasinoBalanceManager _casinoBalanceManager;

        public BalanceController()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
        }

        [HttpGet]
        public ActionResult<decimal> Get()
        {
            return _casinoBalanceManager.GetBalance();
        }
    }
}
