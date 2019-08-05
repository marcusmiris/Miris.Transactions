#if NETSTANDARD
using System;

namespace Miris.Transactions.Breacher.Proxies
{
    public class EnlistmentProxy
        : BreacherProxy
    {

        #region ' ctor '

        public EnlistmentProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }

        #endregion

        public InternalEnlistmentProxy InternalEnlistment
            => new InternalEnlistmentProxy(() => _ProxiedInstance.GetPrivateMemberValue("InternalEnlistment"));

    }
}
#endif