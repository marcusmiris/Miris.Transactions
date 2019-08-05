using System.Transactions;
using Miris.Transactions.Breacher;

namespace Miris.Transactions.StackOverflowRepair
{
    public class StackOverflowFixer
        : IEnlistmentNotification
    {

        #region ' ctor '
        internal StackOverflowFixer() { }
        #endregion


        #region' IEnlistmentNotification '

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            var enlistment = preparingEnlistment.Breach();
            var internalEnlistment = enlistment.InternalEnlistment;

            // aumenta o tamanho da Wave.
            var internalTransaction = internalEnlistment.Transaction;
            var phase0Volatiles = internalTransaction.Phase0Volatiles;

            void AumentaTamanhoDaWave() => internalTransaction.Phase0VolatileWaveCount = phase0Volatiles.VolatileEnlistmentCount;

            // como aumentei o tamanho da Wave, os outros RM's não serão notificados,
            // a menos que a gente notifique...

            // prepara os outros enlistments. 
            // PS: é usado o "+1" pq já considera que o enlistment atual já está preparado.
            for (var i = phase0Volatiles.PreparedVolatileEnlistments + 1; i < phase0Volatiles.VolatileEnlistmentCount; i++)
            {
                AumentaTamanhoDaWave();

                var _ = phase0Volatiles.VolatileEnlistments[i];

                // se for o enlistment atual, vaza...
                if (object.ReferenceEquals(_.EnlistmentNotification, this)) continue;

                // processa enlistment.
                _.TwoPhaseState.ChangeStatePreparing(_);
            }
            preparingEnlistment.Done();
        }

        public void Rollback(Enlistment enlistment) => enlistment.Done();

        public void Commit(Enlistment enlistment) => enlistment.Done();

        public void InDoubt(Enlistment enlistment) => enlistment.Done();

        #endregion
    }
}
