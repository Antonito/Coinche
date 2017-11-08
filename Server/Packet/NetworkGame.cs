using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using Coinche.Common.PacketType;
using ProtoBuf;
using NetworkCommsDotNet.DPSBase;
using System.IO;

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
            connection.AppendIncomingPacketHandler<byte[]>(_type, Handler);

        }

        public static void Unregister(Connection connection)
        {
            var connectInfos = ConnectionManager.Get(connection);
            if (connectInfos.IsGameReady)
            {
                Console.WriteLine("[debug] unregister {0} from game.", connectInfos.Pseudo);
                connectInfos.IsGameReady = false;
                _gameReadyCount--;
            }
            connection.RemoveIncomingPacketHandler(_type);
        }

        private static void Handler(PacketHeader header, Connection connection, byte[] game)
        {
            Console.WriteLine("[Debug] receiving Ready State from a client");

            // we get the ConnectInfos to access the client's stream (locally)
            var connectInfos = ConnectionManager.Get(connection);

            //Wrtting the serialized data into client's stream.
            connectInfos.Stream.Write(game, 0, game.Length);

            //and finllay we deserialize the data
            StartGame _g = Serializer.Deserialize<StartGame>(connectInfos.Stream);

            if (!connectInfos.IsGameReady && _g.IsReady)
            {
                _gameReadyCount++;
                connectInfos.IsGameReady = true;
                Console.WriteLine("{0} is ready", connectInfos.Pseudo);
                // TODO: send to client ok waiting others for beauty purpose
            }
            if (_gameReadyCount == 4)
            {
                // We must konw if the game is started or not when a player quits
                connectInfos.Lobby.IsStarted = true;

                // TODO: game ready lets go
                Console.WriteLine("game launched,  time to continue");
            }
        }
    }
}
