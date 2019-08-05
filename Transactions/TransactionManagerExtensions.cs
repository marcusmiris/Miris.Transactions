using System;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace Miris.Transactions
{
    public static class TransactionManagerExtensions
    {
        /// <summary>
        ///     Sets the default maximum timeout interval for new transactions.
        /// </summary>
        /// <remarks>
        ///     This method avoids the limitation of to set the timeout intervals for transacions only in machine.config.
        /// </remarks>
        /// <returns>
        ///     A System.TimeSpan value that specifies the maximum timeout interval that is allowed
        ///     when creating new transactions.
        /// </returns>
        public static void OverrideSystemMaximumTimeout(TimeSpan timespan)
        {
            FieldInfo GetField(string fieldName) 
                => typeof(System.Transactions.TransactionManager).GetField(fieldName, NonPublic | Static)
                ?? throw new NotSupportedException("Não é possível sobrepor o valor máximo de timeout definido no sistema porque a implementação interna do TransactionManager foi alterada. Provavelmente será necessário refatorar esta função.");
            //            
            GetField("_cachedMaxTimeout").SetValue(null, true);
            GetField("_maximumTimeout").SetValue(null, timespan);
        }
    }
}
