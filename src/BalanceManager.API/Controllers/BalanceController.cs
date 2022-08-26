using BalanceManager.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BalanceManager.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        }

        [HttpGet]
        public ActionResult<decimal> Get()
        {
            return _balanceService.GetBalance();
        }
    }
}
