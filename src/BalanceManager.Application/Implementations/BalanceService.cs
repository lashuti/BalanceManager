using BalanceManager.Application.Abstractions;
using BalanceManager.Application.Exceptions;
using BalanceManager.Domain.Enums;
using Balances;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Application.Implementations
{
    public class BalanceService : IBalanceService
    {
        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        //Add XML Comments in Swagger
        //Add Unit Tests
        //Authorization
        public BalanceService()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
        }

        public ActionResult<decimal> GetBalance()
        {
            return _casinoBalanceManager.GetBalance();
        }

        public ActionResult<ErrorCode> TransferBalance(decimal amount, string transactionId, OperationType operationType)
        {
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

            #region DecreaseBalance
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
            #endregion
            
            #region IncreaseBalance
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
            #endregion
        }

        public ActionResult<ErrorCode> TransferBalanceV2(decimal amount, string transactionId, OperationType operationType)
        {
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

            #region DecreaseBalance
            var startingBalanceBeforeDecrease = _balanceToDecrease.GetBalance();

            var decreaseBalance = _balanceToDecrease.DecreaseBalance(amount, transactionId);

            if (decreaseBalance != ErrorCode.Success && decreaseBalance != ErrorCode.UnknownError)
                throw new BalanceDecreaseException(transactionId);

            if (decreaseBalance is ErrorCode.UnknownError)
            {
                if (startingBalanceBeforeDecrease != _balanceToDecrease.GetBalance())
                {
                    var rollback = _balanceToDecrease.Rollback(transactionId);

                    if (rollback == ErrorCode.UnknownError)
                        throw new BalanceRollbackException(transactionId);

                    throw new BalanceDecreaseException(transactionId);
                }
                throw new BalanceDecreaseException(transactionId);
            }
            #endregion

            #region IncreaseBalance
            var startingBalanceBeforeIncrease = _balanceToIncrease.GetBalance();

            var increaseBalance = _balanceToIncrease.IncreaseBalance(amount, transactionId);

            if (increaseBalance != ErrorCode.Success && increaseBalance != ErrorCode.UnknownError)
            {
                RollbackDecrease(_balanceToDecrease, transactionId);
            }

            if (increaseBalance is ErrorCode.UnknownError)
            {
                if (startingBalanceBeforeIncrease != _balanceToIncrease.GetBalance())
                {
                    var rollbackIncrease = _balanceToIncrease.Rollback(transactionId);

                    if (rollbackIncrease == ErrorCode.UnknownError)
                        throw new BalanceRollbackException(transactionId);

                    RollbackDecrease(_balanceToDecrease, transactionId);
                }
                RollbackDecrease(_balanceToDecrease, transactionId);
            }

            return ErrorCode.Success;
            #endregion
        }

        private static void RollbackDecrease(IBalanceManager balanceToDecrease, string transactionId)
        {
            var rollback = balanceToDecrease.Rollback(transactionId);

            if (rollback == ErrorCode.UnknownError)
                throw new BalanceRollbackException(transactionId);

            throw new BalanceIncreaseException(transactionId);
        }
    }
}
