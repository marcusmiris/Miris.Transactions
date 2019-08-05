using System;
using System.Reflection;
using System.Transactions;

namespace Miris.Transactions.Breacher
{
    public static class SystemTransactionAssemblyProxy
    {
        #region ' Assembly '

        private static Assembly _assembly = null;

        public static Assembly Assembly 
            => _assembly
            ?? (_assembly = typeof(Transaction).Assembly);

        #endregion

        #region ' GetType(...) '

        public static Type GetType(string name)
            => Assembly.GetType($"System.Transactions.{name}");

        #endregion
    }
}
