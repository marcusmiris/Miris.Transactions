#if NETFRAMEWORK
using System;
using System.Diagnostics;
using System.Threading;

namespace Miris.Transactions.Breacher.Proxies
{
    [DebuggerStepThrough]
    public class EnlistmentStateProxy
        : BreacherProxy
    {

        #region ' ctor '

        public EnlistmentStateProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }

        #endregion

        public void EnterState(InternalEnlistmentProxy enlistment)
            => _ProxiedInstance.InvokePrivateAction(
                "EnterState",
                new[] { enlistment._ProxiedInstance });

        public void ChangeStatePreparing(
            InternalEnlistmentProxy enlistment)
        {
            bool lockWasTaken = false;

            // O Método `ChangeStatePreparing` espera que exista um lock da transação do enlistment.
            // Assim, preferi colocar essa chamada aqui porque não existe a possibilidade de chamar 
            // o método sem que este lock exista.
            if (!Monitor.IsEntered(enlistment.Transaction._ProxiedInstance))
            {
                Monitor.Enter(enlistment.Transaction._ProxiedInstance, ref lockWasTaken);
            }

            try
            {
                // invoca método proxyado
                _ProxiedInstance.InvokePrivateAction(
                    "ChangeStatePreparing",
                    new[] { enlistment._ProxiedInstance });
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(enlistment.Transaction._ProxiedInstance);
                }
            }
            
        }
    }
}
#endif