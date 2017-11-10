using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Client
{
    public static class LobbyRoom
    {
        private static readonly string _message = "LobbyRoomMessage";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_message, LobbyRoomMessageHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_message);
        }

        // Displays chatroom messages
        private static void LobbyRoomMessageHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
        }
    }
}
