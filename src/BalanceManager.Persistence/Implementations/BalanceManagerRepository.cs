using BalanceManager.Domain.Enums;
using BalanceManager.Persistence.Abstractions;
using Balances;
using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Persistence.Implementations
{
    public class BalanceManagerRepository : IBalanceManagerRepository
    {
        private readonly Func<InstanceEnum, IBalanceManager> _balanceManager;

        public BalanceManagerRepository(Func<InstanceEnum, IBalanceManager> balanceManager)
        {
            _balanceManager = balanceManager;
        }

        public ErrorCode DecreaseBalance(decimal amount, string transactionId)
        {
            var obj = _balanceManager(InstanceEnum.CasinoInstance);
            return obj.DecreaseBalance(amount, transactionId);
        }

        public decimal GetBalance()
        {
            throw new NotImplementedException();
        }

        public ErrorCode IncreaseBalance(decimal amount, string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
