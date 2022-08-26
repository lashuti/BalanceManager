using Balances;
using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Persistence.Abstractions
{
    public interface IBalanceManagerRepository
    {
        ErrorCode IncreaseBalance(decimal amount, string transactionId);
        ErrorCode DecreaseBalance(decimal amount, string transactionId);
        decimal GetBalance();
    }
}
