using System;
using System.Transactions;

namespace Miris.Transactions
{
    public static class EnlistmentExtensions
    {

        public static string GetTransactionId(
            this Enlistment enlistment)
        {
            return enlistment?.GetPrivateMemberValue<string>("InternalEnlistment.EnlistmentTraceId.TransactionTraceId.TransactionIdentifier");
        }

        public static EnlistmentState GetState(
            this Enlistment enlistment)
        {
            return (EnlistmentState) Enum.Parse(typeof(EnlistmentState),
                enlistment
                    .GetPrivateMemberValue<object>("InternalEnlistment.State")
                    .GetType().Name);
        }

    }
}
