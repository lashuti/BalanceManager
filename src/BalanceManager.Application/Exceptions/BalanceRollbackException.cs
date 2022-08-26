using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManager.Application.Exceptions
{
    class BalanceRollbackException : Exception
    {
        public BalanceRollbackException() { }

        public BalanceRollbackException(string transactionId)
            : base(string.Format("Possible balance loss! Rollback failed on transaction: {0}", transactionId))
        {

        }
    }
}
