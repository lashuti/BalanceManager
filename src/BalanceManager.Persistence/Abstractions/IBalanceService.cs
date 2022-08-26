using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Balances;
using BalanceManager.Domain.Enums;

namespace BalanceManager.Persistence.Abstractions
{
    public interface IBalanceService
    {
        ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId, OperationType operationType);

    }
}
