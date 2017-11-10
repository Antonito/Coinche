using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Shifts the list left.
        /// </summary>
        [Test]
        public void ShiftListLeft()
        {
            List<int> l = new List<int> { 1, 2, 3, 4 };

            l = l.ShiftLeft(2);
            Assert.AreEqual(3, l[0]);
            Assert.AreEqual(4, l[1]);
            Assert.AreEqual(1, l[2]);
            Assert.AreEqual(2, l[3]);
            l = l.ShiftLeft(2);
            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(4, l[3]);
            l = l.ShiftLeft(1);
            Assert.AreEqual(2, l[0]);
            Assert.AreEqual(3, l[1]);
            Assert.AreEqual(4, l[2]);
            Assert.AreEqual(1, l[3]);
        }

        /// <summary>
        /// Shifts the list right.
        /// </summary>
        [Test]
        public void ShiftListRight()
        {
            List<int> l = new List<int> { 1, 2, 3, 4 };

            l = l.ShiftRight(2);
            Assert.AreEqual(3, l[0]);
            Assert.AreEqual(4, l[1]);
            Assert.AreEqual(1, l[2]);
            Assert.AreEqual(2, l[3]);
            l = l.ShiftRight(2);
            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(4, l[3]);
            l = l.ShiftRight(1);
            Assert.AreEqual(4, l[0]);
            Assert.AreEqual(1, l[1]);
            Assert.AreEqual(2, l[2]);
            Assert.AreEqual(3, l[3]);
        }
    }
}
