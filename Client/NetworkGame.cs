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
        private static readonly string _getCard = "PlayerGetGameCard";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<byte[]>(_type, InfoGameHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_getCard, GetCardHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
            connection.RemoveIncomingPacketHandler(_getCard);
        }

        private static void InfoGameHandler(PacketHeader header, Connection connection, byte[] info)
        {
        }

        private static void GetCardHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Console.WriteLine("Receiving card...");

            MemoryStream stream = new MemoryStream(info);
            var card = Serializer.Deserialize<PlayCard>(stream);
            Console.WriteLine("Got: " + card.CardValue + " | " + card.CardColor);

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
