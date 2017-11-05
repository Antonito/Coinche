using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.DPSBase;
using Coinche.Common.PacketType;
using ProtoBuf;

namespace Coinche.Server.Packet
{
    public static class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";
        private static int _gameReadyCount = 0;

        public static void Register(Connection connection)
        {
            // TODO: 
            //SendReceiveOptions customSendReceiveOptions = new SendReceiveOptions<ProtobufSerializer>();
            connection.AppendIncomingPacketHandler<StartGame>(_type, Handler);

        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
        }

        private static void Handler(PacketHeader header, Connection connection, StartGame game)
        {
            var connectInfos = ConnectionManager.Get(connection);
            if (!connectInfos.IsGameReady)
            {
                _gameReadyCount++;
                connectInfos.IsGameReady = true;
                Console.WriteLine("{0} is ready", connectInfos.Pseudo);
                // TODO: send to client ok waiting others
            }
            if (_gameReadyCount == 4)
            {
                // TODO: game ready lets go
                Console.WriteLine("game launched");
            }
        }

        public static void AskClientReady(Connection connection)
        {
            Console.WriteLine("Asking client for ready state");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            //ProtoBuf.Serializer.Serialize(connection.Conn, game);
            connection.SendObject(_type, game);
        }
    }
}
