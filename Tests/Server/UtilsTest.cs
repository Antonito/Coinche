using System;
using NUnit.Framework;
using Coinche.Server.Utils;

namespace Server
{
    [TestFixture]
    public class UtilsTest
    {
        private class TestSetOnce
        {
            public SetOnce<int> nb;

            public TestSetOnce()
            {
                nb = new SetOnce<int>();
            }
        }

        [Test]
        public void SetOnce()
        {
            TestSetOnce test = new TestSetOnce();
            bool thrown = false;

            try
            {
                test.nb.Value = 5;
            }
            catch (Exception)
            {
                thrown = true;                
            }
            Assert.AreEqual(false, thrown);
        }

        [Test]
        public void SetTwice()
        {
            TestSetOnce test = new TestSetOnce();
            bool thrown = false;

            try
            {
                test.nb.Value = 5;
                test.nb.Value = 10;
            }
            catch (Exception)
            {
                thrown = true;
            }
            Assert.AreEqual(true, thrown);
        }
    }
}
