using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BalanceManager.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    class BalanceRollbackException : Exception
    {
        public BalanceRollbackException() { }

        public BalanceRollbackException(string transactionId)
            : base(string.Format("Possible balance loss! Rollback failed on transaction: {0}", transactionId))
        {

        }
    }
}
