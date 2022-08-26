using BalanceManager.Domain.Enums;
using BalanceManager.Persistence.Abstractions;
using Balances;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Persistence.Implementations
{
    public class BalanceService : IBalanceService
    {
        private IBalanceManagerRepository _balanceManager;

        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        //TODO Change to Dependecy Injection mayb
        //Add Controoler calls
        //Add XML Comments in Swagger
        //Add Unit Tests
        //Change into custom exception throw // V1 THIS //V2 Exception Throw
        //Display Enum as String
        //Authorization
        public BalanceService(IBalanceManagerRepository balanceManager)
        {
            _balanceManager = balanceManager;

            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
        }

        public ActionResult<decimal> GetBalance()
        {
            return _casinoBalanceManager.GetBalance();
        }

        public ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId, OperationType operationType)
        {
            //_balanceManager.

            IBalanceManager _balanceToDecrease;
            IBalanceManager _balanceToIncrease;

            if (operationType is OperationType.Deposit)
            {
                _balanceToDecrease = _gameBalanceManager;
                _balanceToIncrease = _casinoBalanceManager;
            }
            else
            {
                _balanceToDecrease = _casinoBalanceManager;
                _balanceToIncrease = _gameBalanceManager;
            }


            var startingBalanceBeforeDecrease = _balanceToDecrease.GetBalance();

            var decreaseBalance = _balanceToDecrease.DecreaseBalance(amount, transactionId);

            if (decreaseBalance != ErrorCode.Success && decreaseBalance != ErrorCode.UnknownError)
                return decreaseBalance;

            if (decreaseBalance is ErrorCode.UnknownError)
            {
                if (startingBalanceBeforeDecrease != _balanceToDecrease.GetBalance())
                {
                    var rollback = _balanceToDecrease.Rollback(transactionId);

                    if (rollback == ErrorCode.UnknownError)
                        while (startingBalanceBeforeDecrease != _balanceToDecrease.GetBalance())
                            _balanceToDecrease.Rollback(transactionId);

                    return ErrorCode.TransactionRollbacked;
                }
            }
            //////////////////////////////////////////////////////
            //exception handling THROW Exceptions and then Handle them globally from that STATIA

            var startingBalanceBeforeIncrease = _balanceToIncrease.GetBalance();

            var increaseBalance = _balanceToIncrease.IncreaseBalance(amount, transactionId);

            var statusForIncreaseBalance = ErrorCode.Success;

            if (increaseBalance != ErrorCode.Success && increaseBalance != ErrorCode.UnknownError)
                statusForIncreaseBalance = increaseBalance;

            if (increaseBalance is ErrorCode.UnknownError)
            {
                if (startingBalanceBeforeIncrease != _balanceToIncrease.GetBalance())
                {
                    var rollback = _balanceToIncrease.Rollback(transactionId);

                    if (rollback == ErrorCode.UnknownError)
                        while (startingBalanceBeforeIncrease != _balanceToIncrease.GetBalance())
                            _balanceToIncrease.Rollback(transactionId);

                    statusForIncreaseBalance = ErrorCode.TransactionRollbacked;
                }
            }

            if (statusForIncreaseBalance != ErrorCode.Success)
            {
                var rollback = _balanceToDecrease.Rollback(transactionId);

                if (rollback == ErrorCode.UnknownError)
                {
                    while (startingBalanceBeforeDecrease != _balanceToDecrease.GetBalance())
                        _balanceToDecrease.Rollback(transactionId);
                    while (startingBalanceBeforeIncrease != _balanceToIncrease.GetBalance())
                        _balanceToIncrease.Rollback(transactionId);
                }
                return ErrorCode.TransactionRollbacked;
            }
            return ErrorCode.Success;
        }
    }
}
