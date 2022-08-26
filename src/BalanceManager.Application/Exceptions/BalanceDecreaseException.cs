using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
