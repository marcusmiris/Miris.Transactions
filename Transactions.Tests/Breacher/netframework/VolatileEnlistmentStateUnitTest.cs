#if NETFRAMEWORK
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Miris.Transactions.Breacher.Proxies;

namespace Miris.Transactions.Tests.Breacher
{
    [TestClass]
    public class VolatileEnlistmentStateUnitTest
    {
        [TestMethod]
        public void GetVolatileEnlistmentDone()
        {
            var obj = VolatileEnlistmentStateProxy._VolatileEnlistmentActive;
            Assert.IsNotNull(obj);
        }
    }
}
#endif