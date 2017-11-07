using System;
using System.IO;
using System.Linq;
using Coinche.Common.PacketType;
using NUnit.Framework;
using ProtoBuf;

namespace Tests.Server
{

    [TestFixture]
    public class UnitTestSerialize
    {
        [Test]
        public void SerializeTest()
        {
            StartGame g1 = new StartGame
            {
                IsReady = true
            };
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, g1);
            byte[] t = stream.ToArray();
            stream.Flush();
            stream.Position = 0;

            StartGame g2 = Serializer.Deserialize<StartGame>(stream);
            Assert.AreEqual(true, g2.IsReady);
        }
    }
}
