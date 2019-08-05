using Castle.DynamicProxy;
using System;
using System.Transactions;

namespace Miris.Transactions.Interceptor
{

    public class TransactionScopeInterceptor
        : IInterceptor
    {

        #region ' Members '
        private readonly InvocationSpecification _deveCriarTransactionScope;
        private readonly InvocationSpecification _deveCompletarTransaction;
        #endregion

        #region ' ctor '

        public TransactionScopeInterceptor(
            InvocationSpecification deveCriarTransactionScope,
            InvocationSpecification deveCompletarTransaction)
        {
            _deveCompletarTransaction = deveCompletarTransaction;
            _deveCriarTransactionScope = deveCriarTransactionScope;
        }

        #endregion

        #region ' IInterceptor'

        public void Intercept(IInvocation invocation)
        {
            if (!_deveCriarTransactionScope(invocation))
            {
                invocation.Proceed();
            }
            else
            {
                try
                {
                    using (var trans = new TransactionScope(
                        TransactionScopeOption.Required,
                        new TransactionOptions()
                        {
                            //IsolationLevel = IsolationLevel.Snapshot,   // https://blogs.msdn.microsoft.com/diego/2012/03/31/tips-to-avoid-deadlocks-in-entity-framework-applications/
                            Timeout = TimeSpan.FromDays(1)
                        },
                        TransactionScopeAsyncFlowOption.Enabled))
                    {
                        invocation.Proceed();

                        if (_deveCompletarTransaction(invocation))
                            trans.Complete();
                    }
                }
                catch (TransactionAbortedException e)
                {
                    throw e.InnerException ?? e;
                }
            }
        }

        #endregion

    }
}
