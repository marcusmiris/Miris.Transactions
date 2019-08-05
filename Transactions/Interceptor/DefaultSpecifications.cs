using System;
using System.Linq;
using System.Transactions;

namespace Miris.Transactions.Interceptor
{
    public static class DefaultSpecifications
    {


        /// <summary>
        ///     Determina se o TransactionScope deve ser criado.
        /// </summary>
        public static InvocationSpecification DeveCriarTransactionScope => invocation =>
        {
            var explicitlyDeniedByAttribute = ((Func<bool>)(() =>
            {
                var implicitScopeConfig = invocation.MethodInvocationTarget
                    .GetCustomAttributes(inherit: true)
                    .OfType<ImplicitTransactionScopeAttribute>()
                    .SingleOrDefault();
                return implicitScopeConfig?.Criar == false;
            }))();
            var existeEscopoTransacional = Transaction.Current != null;
            //
            return !existeEscopoTransacional && !explicitlyDeniedByAttribute;
        };



        /// <summary>
        ///     Determina se, uma vez criada a transação, ela deve ser comitada.
        ///     Isso pode ser deterinada baseada no resultado do método proxiado.
        /// </summary>
        public static InvocationSpecification DeveCompletarTransacao => _ => true;
    }
}
