#if NETSTANDARD
using System;

namespace Miris.Transactions.Breacher.Proxies
{
    public class VolatileEnlistmentSetProxy
        : BreacherProxy
    {

        #region ' ctor '
        public VolatileEnlistmentSetProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }
        #endregion

        public InternalEnlistmentArrayProxy VolatileEnlistments
        {
            get
            {
                var array = _ProxiedInstance.GetPrivateMemberValue<Array>("_volatileEnlistments");
                return array != null ? new InternalEnlistmentArrayProxy(array) : null;
            }
        }

        public int VolatileEnlistmentCount
            => _ProxiedInstance.GetPrivateMemberValue<int>("_volatileEnlistmentCount");

        public int VolatileEnlistmentSize
            => _ProxiedInstance.GetPrivateMemberValue<int>("_volatileEnlistmentSize");

        public int DependentClones
            => _ProxiedInstance.GetPrivateMemberValue<int>("_dependentClones");

        public int PreparedVolatileEnlistments
            => _ProxiedInstance.GetPrivateMemberValue<int>("_preparedVolatileEnlistments");

    }
}
#endif