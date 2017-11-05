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
            Unregister(connection);
            LobbyRoom.Register(connection);
            NetworkGame.Register(connection);

            // TODO: Move
            while (true)
            {
                Console.WriteLine("Send message to lobby: ");
                string msg = Console.ReadLine();

                if (msg.StartsWith("/quit"))
                {
                    connection.SendObject("LobbyRoomQuit");
                    LobbyRoom.Unregister(connection);
                    Lobby.Register(connection);
                    break;
                }
                else
                {
                    connection.SendObject("LobbyRoomMessage", msg);
                }
            }
        }
    }
}
