using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    // Handle lobby creation or selection here
    public static class LobbyRoom
    {
        private static readonly string _type = "LobbyRoom";
        private static readonly string _messageType = "LobbyRoomMessage";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_type, Handler);
            connection.AppendIncomingPacketHandler<string>(_messageType, MessageHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
            connection.RemoveIncomingPacketHandler(_messageType);
        }

        private static void Handler(PacketHeader header, Connection connection, string message)
        {
            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            var room = connectInfos.Lobby.Name;
            Console.WriteLine("[LobbyRoom - " + room + "] " + pseudo + ": " + message);
        }

        private static void MessageHandler(PacketHeader header, Connection connection, string message)
        {
            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            var room = connectInfos.Lobby.Name;
            var msg = "[" + room + "] " + pseudo + ": " + message;

            foreach (var lobbyConnection in connectInfos.Lobby.Connection)
            {
                if (lobbyConnection != connection)
                {
                    lobbyConnection.SendObject(_messageType, msg);
                }
            }
        }
    }
}
