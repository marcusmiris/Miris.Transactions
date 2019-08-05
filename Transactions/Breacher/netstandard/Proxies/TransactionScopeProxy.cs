#if NETSTANDARD
using System;
using System.Transactions;

namespace Miris.Transactions.Breacher.Proxies
{
    public class TransactionScopeProxy
        : BreacherProxy
    {

        #region ' ctor '
        public TransactionScopeProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }
        #endregion


        #region ' Members '

        public TransactionProxy CommitableTransaction
            => new TransactionProxy(() => _ProxiedInstance.GetPrivateMemberValue<Transaction>("_committableTransaction"));

        #endregion

    }
}
#endif