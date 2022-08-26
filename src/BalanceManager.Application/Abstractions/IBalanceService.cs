using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Balances;
using BalanceManager.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace BalanceManager.Application.Abstractions
{
    public interface IBalanceService
    {
        ActionResult<decimal> GetBalance();
        ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId, OperationType operationType);
        ActionResult<ErrorCode> TransferBalanceV2(decimal amount, string transactionId, OperationType operationType);
    }
}
