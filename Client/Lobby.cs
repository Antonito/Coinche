using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Client
{
    public static class Lobby
    {
        private static readonly string _infos = "LobbyInfo";
        private static readonly string _err = "LobbyError";
        private static readonly string _select = "LobbySelect";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_infos, LobbyInfoHandler);
            connection.AppendIncomingPacketHandler<string>(_err, LobbyErrorHandler);
            connection.AppendIncomingPacketHandler<string>(_select, LobbySelectHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_err);
            connection.RemoveIncomingPacketHandler(_infos);
            connection.RemoveIncomingPacketHandler(_select);
        }

        public static void Connect(Connection connection)
        {
            Console.WriteLine("Which lobby do you want to join ?");
            string msg = Console.ReadLine();
            connection.SendObject("SelectLobby", msg);
        }

        private static void LobbySelectHandler(PacketHeader header, Connection connection, string message)
        {
            Program.clientInfos.IsRun = false;
            Connect(connection);
        }

        private static void LobbyErrorHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);

            Connect(connection);
        }

        private static void LobbyInfoHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
            //Unregister(connection);
            NetworkGame.Register(connection);
            LobbyRoom.Register(connection);

            // TODO: Move
            Program.clientInfos.IsRun = true;
            while (Program.clientInfos.IsRun)
            {
                Console.WriteLine("Send message to lobby: ");
                string msg = Console.ReadLine();

                if (msg.StartsWith("/quit"))
                {
                    Program.clientInfos.IsRun = false;
                }
                else if (msg.StartsWith("/ready"))
                {
                    NetworkGame.SendReady(connection);
                }
                else
                {
                    connection.SendObject("LobbyRoomMessage", msg);
                }
            }
            Console.WriteLine("quit lobby");
            NetworkGame.Unregister(connection);
            connection.SendObject("LobbyRoomQuit");
            LobbyRoom.Unregister(connection);
            //Lobby.Register(connection);
            Console.WriteLine("End and final quit lobby");
        }
    }
}
