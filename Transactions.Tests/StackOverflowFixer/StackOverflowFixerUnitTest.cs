using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Miris.Transactions;
using Miris.Transactions.Breacher;
using Miris.Transactions.StackOverflowRepair;

namespace Miris.Transactions.Tests
{
    [TestClass]
    public class StackOverflowFixerUnitTest
    {

        [TestMethod]
        public void NaoAplicaPatch2Vezes()
        {

            using (var scope = new TransactionScope())
            {
                // tenta aplicar 2 vezes o patch.
                scope.ApplyStackOverflowPatch();
                scope.ApplyStackOverflowPatch();

                // verifica que na verdade foi adicionado apenas 1.
                var qtdRMs = scope.Breach().CommitableTransaction.InternalTransaction.Phase0Volatiles.VolatileEnlistmentCount;

                Assert.AreNotEqual(0, qtdRMs, $"Não foi encontrato o { nameof(StackOverflowFixer) } na lista de resource managers apensados à transação");
                Assert.AreEqual(1, qtdRMs, $"Foi registrado mais de uma vez o { nameof(StackOverflowFixer) } na lista de resource managers apensados à transação");
            }

        }

        [TestMethod]
        public void RealmenteMitigaStackOverflowException()
        {
            const int nTimes = 100;
            int qtdDeRMsExecutados = 0;
            var callStackSizes = new List<int>();     // usado para manter o histórico dos callbacks na 

            #region ' void OnPhase0NTimes(...) '
            void OnPhase0NTimes(int n, Transaction transaction, Action action)
            {
                Debug.Assert(n >= 0);

                if (n-- == 0) return;

                transaction.OnPhase0(() =>
                {
                    // executa RM
                    action();   

                    // Apensa novo RM
                    OnPhase0NTimes(n, transaction, action);
                });
            }
            #endregion

            using (var scope = new TransactionScope())
            {
                // adiciona `StackOverflowFixer`.
                scope.ApplyStackOverflowPatch();

                // Adiciona RM.
                OnPhase0NTimes(nTimes, Transaction.Current, () =>
                {
                    qtdDeRMsExecutados++;
                    callStackSizes.Add(Environment.StackTrace.Length);
                });
                
                // completa transação
                scope.Complete();
            }

            // verifica se foram executados as vezes corretamente.
            Assert.AreEqual(nTimes, qtdDeRMsExecutados);

            // verifica se geraria call stack...
            if (callStackSizes.Distinct().Count() != 1)
            {
                // ... então o código certamente geraria stack overflow.
                // Assim eu quebro teste gerando a exception.
                throw new StackOverflowException();
            }
        }


        [TestMethod]
        public void RealizaRollbackDevidamenteESemStackOverflow()
        {
            const int nTimes = 100;
            int qtdDeRMsExecutados = 0;
            int qtdDeRollbacksExecutados = 0;
            var callStackSizes = new List<int>();     // usado para manter o histórico dos callbacks na 

            #region ' void OnPhase0NTimes(...) '
            void OnPhase0NTimes(int n, Transaction transaction, Action onPrepare, Action onRollback)
            {
                Debug.Assert(n >= 0);

                if (n-- == 0) return;

                transaction.OnPhase0(() =>
                {
                    // executa RM
                    onPrepare();

                    // Apensa novo RM
                    OnPhase0NTimes(n, transaction, onPrepare, onRollback);
                },
                onRollback: onRollback);
            }
            #endregion

            try
            {
                using (var scope = new TransactionScope())
                {
                    // adiciona `StackOverflowFixer`.
                    scope.ApplyStackOverflowPatch();

                    // Adiciona RM.
                    OnPhase0NTimes(nTimes, Transaction.Current, () =>
                    {
                        qtdDeRMsExecutados++;
                        callStackSizes.Add(Environment.StackTrace.Length);

                        // levanta erro no último RM com o fim de forçar o rollback da transçaão.
                        if (qtdDeRMsExecutados == nTimes)
                        {
                            throw new Exception("...para forçar rollback");
                        }
                    },
                    onRollback: () => qtdDeRollbacksExecutados++);

                    // completa transação
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex) when (ex.InnerException?.Message == "...para forçar rollback")
            {
                // show de bola! Vida que segue
            }
            catch (Exception ex)
            {
                Assert.Fail($"Ops... deveria ter gerado exception! \r\nDetalhes: { ex.ToString() }");
            }
            
            // verifica se foram executados as vezes corretamente.
            Assert.AreEqual(nTimes - 1, qtdDeRollbacksExecutados);

            // verifica se geraria call stack...
            if (callStackSizes.Distinct().Count() != 1)
            {
                // ... então o código certamente geraria stack overflow.
                // Assim eu quebro teste gerando a exception.
                throw new StackOverflowException();
            }

        }
    }
}
