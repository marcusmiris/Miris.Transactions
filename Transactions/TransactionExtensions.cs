using System;
using System.Transactions;

namespace Miris.Transactions
{
    public static class TransactionExtensions
    {

        public static void OnPhase0(
            this Transaction transaction,
            Action onPrepare,
            Action onRollback = null)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (onPrepare == null) throw new ArgumentNullException(nameof(onPrepare));

            transaction.EnlistVolatile(
                    new AnonymousResourceManager(
                        onPrepare: onPrepare,
                        onRollback: onRollback),
                    EnlistmentOptions.EnlistDuringPrepareRequired);
        }

        public static bool IsDisposed(
            this Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            //
            return transaction.GetPrivateMemberValue<bool>("Disposed");
        }

        public static void SetPhase0VolatileWaveCount(
            this Transaction transaction,
            int value)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            //
            transaction.SetPrivateMemberValue<int>("internalTransaction.phase0VolatileWaveCount", value);
        }

    }
}
