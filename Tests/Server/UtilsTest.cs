using System;
using NUnit.Framework;
using Coinche.Server.Utils;

namespace Server
{
    /// <summary>
    /// Utils test.
    /// </summary>
    [TestFixture]
    public class UtilsTest
    {
        /// <summary>
        /// Test set once.
        /// </summary>
        private class TestSetOnce
        {
            /// <summary>
            /// The value to set.
            /// </summary>
            public SetOnce<int> nb;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Server.UtilsTest.TestSetOnce"/> class.
            /// </summary>
            public TestSetOnce()
            {
                nb = new SetOnce<int>();
            }
        }

        /// <summary>
        /// Try to set once a value, should not thrown
        /// </summary>
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

        /// <summary>
        /// Try to set twice a value, should throw
        /// </summary>
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
