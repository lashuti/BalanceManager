using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
