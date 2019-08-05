using Castle.DynamicProxy;

namespace Miris.Transactions.Interceptor
{
    public delegate bool InvocationSpecification(
        IInvocation invocation);

}
