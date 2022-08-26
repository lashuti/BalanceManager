using Microsoft.AspNetCore.Mvc;
using Balances;
using BalanceManager.Domain.Enums;

namespace BalanceManager.Application.Abstractions
{
    public interface IBalanceService
    {
        ActionResult<decimal> GetBalance();
        ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId, OperationType operationType);
        ActionResult<ErrorCode> TransferBalanceV2(decimal amount, string transactionId, OperationType operationType);
    }
}
