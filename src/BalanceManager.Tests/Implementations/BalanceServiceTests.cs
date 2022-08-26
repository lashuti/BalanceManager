using System;
using FluentAssertions;
using BalanceManager.Application.Implementations;
using Balances;
using Xunit;
using BalanceManager.Domain.Enums;
using BalanceManager.Application.Exceptions;

namespace BalanceManager.Application.Tests.Implementations
{
    public class BalanceServiceTests
    {
        private readonly BalanceService _balanceService;
        public BalanceServiceTests()
        {
            _balanceService = new BalanceService();
        }

        [Theory]
        [InlineData(0,"123",OperationType.Deposit)]
        public void TransferBalance_should_return_TransactionRejected_when_amount_is_zero_or_less
            (decimal amount, string transactionId, OperationType operationType)
        {
            var result = _balanceService.TransferBalance(amount, transactionId, operationType);
            result.Value.Should().Be(ErrorCode.TransactionRejected);
        }

        [Theory]
        [InlineData(100, "123", OperationType.Withdraw)]
        public void TransferBalance_should_return_Success_when_amount_everything_went_fine
            (decimal amount, string transactionId, OperationType operationType)
        {
            var result = _balanceService.TransferBalance(amount, transactionId, operationType);
            result.Value.Should().Be(ErrorCode.Success);
        }

        [Theory]
        [InlineData(0, "123", OperationType.Deposit)]
        public void TransferBalanceV2_should_throw_BalanceDecreaseException_when_amount_is_zero_or_less
            (decimal amount, string transactionId, OperationType operationType)
        {
            try
            {
                _balanceService.TransferBalanceV2(amount, transactionId, operationType);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<BalanceDecreaseException>();
            }
        }

        [Theory]
        [InlineData(0, "123", OperationType.Deposit)]
        public void TransferBalanceV2_should_throw_BalanceDecreaseException_when_transactionId_is_same
            (decimal amount, string transactionId, OperationType operationType)
        {
            try
            {
                _balanceService.TransferBalanceV2(amount, transactionId, operationType);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<BalanceDecreaseException>();
            }
        }

        [Theory]
        [InlineData(100, "12345", OperationType.Withdraw)]
        public void TransferBalanceV2_should_return_Success_when_everything_went_fine
            (decimal amount, string transactionId, OperationType operationType)
        {
            var result = _balanceService.TransferBalanceV2(amount, transactionId, operationType);
            result.Value.Should().Be(ErrorCode.Success);
        }
    }
}
