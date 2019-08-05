#if NETSTANDARD
using System;

namespace Miris.Transactions.Breacher.Proxies
{
    public class VolatileEnlistmentStateProxy
        : EnlistmentStateProxy
    {
#region ' ctor '
        public VolatileEnlistmentStateProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }
        #endregion

        #region ' static '

        public static VolatileEnlistmentStateProxy _VolatileEnlistmentActive =>
            new VolatileEnlistmentStateProxy(
                () => SystemTransactionAssemblyProxy
                    .GetType("VolatileEnlistmentState")
                    .GetPrivateStaticMemberValue("_VolatileEnlistmentActive"));

        public static VolatileEnlistmentStateProxy _VolatileEnlistmentPreparing =>
            new VolatileEnlistmentStateProxy(
                () => SystemTransactionAssemblyProxy
                    .GetType("VolatileEnlistmentState")
                    .GetPrivateStaticMemberValue("VolatileEnlistmentPreparing"));

        #endregion
    }
}
#endif