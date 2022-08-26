using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Application.Exceptions
{
    public class BalanceDecreaseException : Exception
    {
        public BalanceDecreaseException() { }

        public BalanceDecreaseException(string transactionId)
            : base(string.Format("Balance decrease failed on transaction: {0}", transactionId))
        {

        }
    }
}
