using System;
using System.Diagnostics.CodeAnalysis;

namespace BalanceManager.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BalanceDecreaseException : Exception
    {
        public BalanceDecreaseException() { }

        public BalanceDecreaseException(string transactionId)
            : base(string.Format("Balance decrease failed on transaction: {0}", transactionId))
        {

        }
    }
}
