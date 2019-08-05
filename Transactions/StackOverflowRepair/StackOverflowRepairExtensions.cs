using System;
using System.Linq;
using System.Transactions;
using Miris.Transactions.Breacher;

namespace Miris.Transactions.StackOverflowRepair
{
    public static class StackOverflowRepairExtensions
    {

        public static void ApplyStackOverflowPatch(
            this TransactionScope scope)
        {
            Transaction transaction = scope.Breach().CommitableTransaction;
            
            // aplica patch.
            transaction.ApplyStackOverflowPatch();
        }

        public static void ApplyStackOverflowPatch(
            this Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            // se o patch já está aplicado, vaza...
            var enlistments = transaction.Breach().InternalTransaction.Phase0Volatiles.VolatileEnlistments;
            var patchJahAplicado = enlistments?.Select(_ => _.EnlistmentNotification).OfType<StackOverflowFixer>().Any() ?? false;
            if (patchJahAplicado)
            {
                return; // vaza...
            }

            // aplica patch.
            transaction.EnlistVolatile(
                new StackOverflowFixer(),
                EnlistmentOptions.EnlistDuringPrepareRequired);
        }
    }
}
