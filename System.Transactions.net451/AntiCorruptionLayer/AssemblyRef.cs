using System.EnterpriseServices;

namespace System.Transactions
{
    internal static class AssemblyRef
    {
        public static string SystemEnterpriseServices => typeof(ContextUtil).Assembly.FullName;

    }
}
