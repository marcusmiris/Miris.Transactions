#if NETSTANDARD
using System;

namespace Miris.Transactions.Breacher.Proxies
{
    public class InternalTransactionProxy
        : BreacherProxy
    {
        public InternalTransactionProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }


        public VolatileEnlistmentSetProxy Phase0Volatiles
            => new VolatileEnlistmentSetProxy(() => _ProxiedInstance.GetPrivateMemberValue("_phase0Volatiles"));


        public int Phase0VolatileWaveCount
        {
            get => _ProxiedInstance.GetPrivateMemberValue<int>("_phase0VolatileWaveCount");
            set => _ProxiedInstance.SetPrivateMemberValue("_phase0VolatileWaveCount", value);
        }

    }
}
#endif