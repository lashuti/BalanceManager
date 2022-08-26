using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Balances;

namespace BalanceManager.Persistence.Abstractions
{
    public interface IBalanceService
    {
        ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId);

    }
}
