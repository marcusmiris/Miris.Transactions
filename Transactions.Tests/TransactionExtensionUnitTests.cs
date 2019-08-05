using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Miris.Transactions;

namespace Miris.Transactions.Tests
{
    [TestClass]
    public class TransactionExtensionUnitTests
    {

        [TestMethod]
        public void TransactionIsDisposedUnitTest()
        {
            using (new TransactionScope())
            {
                Assert.IsFalse(Transaction.Current.IsDisposed());
                Transaction.Current.Dispose();
                Assert.IsTrue(Transaction.Current.IsDisposed());
            }
        }
    }
}
