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
        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        //TODO Change to Dependecy Injection mayb
        //Add Controoler calls
        //Add XML Comments in Swagger
        //Add Unit Tests
        //Change into custom exception throw // V1 THIS //V2 Exception Throw
        public BalanceService()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
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
            //deposit = game -    casino +


            var startingGameBalance = _gameBalanceManager.GetBalance();

            var decreaseGameBalanceStatus = _gameBalanceManager.DecreaseBalance(amount, transactionId);

            if (decreaseGameBalanceStatus != ErrorCode.Success && decreaseGameBalanceStatus != ErrorCode.UnknownError)
                return decreaseGameBalanceStatus;

            if (decreaseGameBalanceStatus is ErrorCode.UnknownError)
            {
                if (startingGameBalance != _gameBalanceManager.GetBalance())
                {
                    var rollback = _gameBalanceManager.Rollback(transactionId);

                    if (rollback == ErrorCode.UnknownError)
                        while (startingGameBalance != _gameBalanceManager.GetBalance())
                            _gameBalanceManager.Rollback(transactionId);

                    return ErrorCode.TransactionRollbacked;
                }
            }
            //////////////////////////////////////////////////////
            //exception handling THROW Exceptions and then Handle them globally from that STATIA

            var startingCasinoBalance = _casinoBalanceManager.GetBalance();

            var increaseCasinoBalanceStatus = _casinoBalanceManager.IncreaseBalance(amount, transactionId);

            var casinoStatus = ErrorCode.Success;

            if (increaseCasinoBalanceStatus != ErrorCode.Success && increaseCasinoBalanceStatus != ErrorCode.UnknownError) //RollBack Decrease of GameBal
                casinoStatus = increaseCasinoBalanceStatus;

            if (increaseCasinoBalanceStatus is ErrorCode.UnknownError)
            {
                if (startingCasinoBalance != _casinoBalanceManager.GetBalance())
                {
                    var rollback = _casinoBalanceManager.Rollback(transactionId);

                    if (rollback == ErrorCode.UnknownError)
                        while (startingCasinoBalance != _casinoBalanceManager.GetBalance())
                            _casinoBalanceManager.Rollback(transactionId);

                    casinoStatus = ErrorCode.TransactionRollbacked;
                }
            }

            if (casinoStatus != ErrorCode.Success)
            {
                var rollback = _gameBalanceManager.Rollback(transactionId);

                if (rollback == ErrorCode.UnknownError)
                {
                    while (startingGameBalance != _gameBalanceManager.GetBalance())
                        _gameBalanceManager.Rollback(transactionId);
                    while (startingCasinoBalance != _casinoBalanceManager.GetBalance())
                        _casinoBalanceManager.Rollback(transactionId);
                }
                return ErrorCode.TransactionRollbacked;
            }
            return ErrorCode.Success;
        }
    }
}
