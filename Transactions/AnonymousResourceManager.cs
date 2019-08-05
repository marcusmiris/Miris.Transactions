using System;
using System.Transactions;

namespace Miris.Transactions
{
    public class AnonymousResourceManager
        : ISinglePhaseNotification
    {
        private readonly Action _onPrepare;
        private readonly Action _onCommit;
        private readonly Action _onRollback;
        private readonly Action _inDoubt;

        #region ' ctor '

        public AnonymousResourceManager(
            Action onPrepare = null,
            Action onCommit = null,
            Action onRollback = null,
            Action inDoubt = null)
        {
            _inDoubt = inDoubt;
            _onRollback = onRollback;
            _onCommit = onCommit;
            _onPrepare = onPrepare;
        }

        #endregion

        #region ' IEnlistmentNotification ' 

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                _onPrepare?.Invoke();
                preparingEnlistment.Prepared();
            }
            catch (Exception e) when (preparingEnlistment.GetState() == EnlistmentState.VolatileEnlistmentPreparing)
            {
                preparingEnlistment.ForceRollback(e);
            }
        }

        public void Commit(Enlistment enlistment)
        {
            _onCommit?.Invoke();
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            _onRollback?.Invoke();
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            _inDoubt?.Invoke();
            enlistment.Done();
        }

        #endregion

        #region ' ISinglePhaseNotification '

        public void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment)
        {
            try
            {
                _onPrepare?.Invoke();
                _onCommit?.Invoke();
                singlePhaseEnlistment.Committed();
            }
            catch (Exception originalException)
            {
                try
                {
                    _onRollback?.Invoke();
                    singlePhaseEnlistment.Aborted(originalException);
                }
                catch (Exception rollbackException)
                {
                    _inDoubt?.Invoke();
                    singlePhaseEnlistment.InDoubt(new AggregateException(originalException, rollbackException));
                }

            }

        }

        #endregion
    }
}
