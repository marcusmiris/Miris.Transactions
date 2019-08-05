#if NETFRAMEWORK
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

        public InternalEnlistmentProxy InternalEnlistment 
            => new InternalEnlistmentProxy(() => _ProxiedInstance.GetPrivateMemberValue("InternalEnlistment"));

        public InternalTransactionProxy InternalTransaction
            => new InternalTransactionProxy(() => _ProxiedInstance.GetPrivateMemberValue("internalTransaction"));

        #endregion

        #region ' operator '

        public static implicit operator Transaction(TransactionProxy proxy)
            => (Transaction) proxy?._ProxiedInstance;

        #endregion

    }
}
#endif