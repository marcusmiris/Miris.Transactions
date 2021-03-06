﻿#if NETSTANDARD
using System;
using System.Transactions;

namespace Miris.Transactions.Breacher.Proxies
{
    public class TransactionProxy
        : BreacherProxy
    {

        #region ' ctor '

        public TransactionProxy(Func<Transaction> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }

        #endregion

        #region ' Members '

        public InternalTransactionProxy InternalTransaction
            => new InternalTransactionProxy(() => _ProxiedInstance.GetPrivateMemberValue("_internalTransaction"));

        #endregion

        #region ' operator '

        public static implicit operator Transaction(TransactionProxy proxy)
            => (Transaction)proxy?._ProxiedInstance;

        #endregion

    }
}
#endif