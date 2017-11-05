using System;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;
using NetworkCommsDotNet;

namespace Coinche.Client
{
    public class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<StartGame>(_type, StartGameHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
        }

        private static void StartGameHandler(PacketHeader header, Connection connection, StartGame youReady)
        {
            Console.WriteLine("sending ready status for game");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            connection.SendObject(_type, game);
        }
    }
}
