using System;
using System.Diagnostics.CodeAnalysis;

namespace BalanceManager.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BalanceIncreaseException : Exception
    {
        public BalanceIncreaseException() { }

        public BalanceIncreaseException(string transactionId)
            : base(string.Format("Balance increase failed on transaction: {0}", transactionId))
        {

        }
    }
}
