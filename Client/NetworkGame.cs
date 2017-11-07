using System;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;
using NetworkCommsDotNet;
using ProtoBuf;
using System.IO;

namespace Coinche.Client
{
    public class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<byte[]>(_type, InfoGameHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
        }

        private static void InfoGameHandler(PacketHeader header, Connection connection, byte[] info)
        {
            
        }

        public static void SendReady(Connection connection)
        {
            Console.WriteLine("sending ready to server");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            //TODO: set the Stream into a 'client' class like in server
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, game);
            byte[] t = stream.ToArray();
            connection.SendObject(_type, t);
        }
    }
}
