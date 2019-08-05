using System.Transactions;
using Miris.Transactions.Breacher.Proxies;

namespace Miris.Transactions.Breacher
{
    public static class BreacherExtensions
    {

        public static TransactionProxy Breach(this Transaction transaction)
            => new TransactionProxy(() => transaction);

        public static EnlistmentProxy Breach(this Enlistment enlistment)
            => new EnlistmentProxy(() => enlistment);

        public static TransactionScopeProxy Breach(this TransactionScope scope)
            => new TransactionScopeProxy(() => scope);

    }
}
