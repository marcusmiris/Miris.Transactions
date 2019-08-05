using System;

namespace Miris.Transactions.Breacher
{
    public abstract class BreacherProxy
    {
        public readonly Func<object> _ProxiedInstanceGetter;
        public object _ProxiedInstance => _ProxiedInstanceGetter();

        #region ' ctor '

        protected BreacherProxy(
            Func<object> _proxiedInstance)
        {
            _ProxiedInstanceGetter = _proxiedInstance;
        }

        #endregion

    }
}
