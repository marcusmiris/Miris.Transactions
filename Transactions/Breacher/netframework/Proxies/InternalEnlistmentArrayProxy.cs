#if NETFRAMEWORK
using System;

namespace Miris.Transactions.Breacher.Proxies
{

    public class InternalEnlistmentArrayProxy
        : BreacherArrayProxy<InternalEnlistmentProxy>
    {
        public InternalEnlistmentArrayProxy(Array wrapped) : base(wrapped)
        {
        }

        protected override InternalEnlistmentProxy ItemProxyFactory(Func<object> proxiedInstance)
        {
            return new InternalEnlistmentProxy(proxiedInstance);
        }
    }
}
#endif