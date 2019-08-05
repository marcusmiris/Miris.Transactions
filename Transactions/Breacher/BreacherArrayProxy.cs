using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Miris.Transactions.Breacher
{
    public abstract class BreacherArrayProxy<T>
        : IEnumerable<T> where T : BreacherProxy

    {
        private Array _wrapped;

        protected BreacherArrayProxy(Array wrapped)
        {
            _wrapped = wrapped;
        }

        protected abstract T ItemProxyFactory(Func<object> arg);

        public T this[int index]
        {
            get
            {
                return ItemProxyFactory(() => _wrapped.GetValue(index));
            }
        }

        public IEnumerable<T> ToEnumerable() => _wrapped.Cast<object>().Select(_ => _ != default ? ItemProxyFactory(() => _) : default);

        #region ' IEnumerable<T> '

        public IEnumerator<T> GetEnumerator() => this.ToEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.ToEnumerable().GetEnumerator();

        #endregion
    }
}
