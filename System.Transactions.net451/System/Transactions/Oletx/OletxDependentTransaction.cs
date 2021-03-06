//-----------------------------------------------------------------------------
// <copyright file="OletxDependentTransaction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
    [Serializable]
    internal class OletxDependentTransaction : OletxTransaction
    {
        private OletxVolatileEnlistmentContainer volatileEnlistmentContainer;

        private int completed = 0;

        internal OletxDependentTransaction(
            RealOletxTransaction realTransaction,
            bool delayCommit
            ) : base( realTransaction )
        {
            if ( null == realTransaction )
            {
                throw new ArgumentNullException( "realTransaction" );
            }

            this.volatileEnlistmentContainer = realOletxTransaction.AddDependentClone( delayCommit );

            if ( DiagnosticTrace.Information )
            {
                DependentCloneCreatedTraceRecord.Trace( SR.GetString("SR.TraceSourceOletx"),
                    this.TransactionTraceId,
                    delayCommit ? DependentCloneOption.BlockCommitUntilComplete : DependentCloneOption.RollbackIfNotComplete
                    );
            }
        }

        public void Complete()
        {
            if ( DiagnosticTrace.Verbose )
            {
                MethodEnteredTraceRecord.Trace( SR.GetString("SR.TraceSourceOletx"),
                    "DependentTransaction.Complete"
                    );
            }

            Debug.Assert( ( 0 == this.disposed ), "OletxTransction object is disposed" );

            int localCompleted = Interlocked.CompareExchange( ref this.completed, 1, 0 );
            if ( 1 == localCompleted )
            {
                throw TransactionException.CreateTransactionCompletedException( SR.GetString("SR.TraceSourceOletx"), this.DistributedTxId);
            }

            if ( DiagnosticTrace.Information )
            {
                DependentCloneCompleteTraceRecord.Trace( SR.GetString("SR.TraceSourceOletx"),
                    this.TransactionTraceId
                    );
            }

            this.volatileEnlistmentContainer.DependentCloneCompleted();

            if ( DiagnosticTrace.Verbose )
            {
                MethodExitedTraceRecord.Trace( SR.GetString("SR.TraceSourceOletx"),
                    "DependentTransaction.Complete"
                    );
            }
        }

    }

}
