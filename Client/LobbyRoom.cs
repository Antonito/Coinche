using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Client
{
    /// <summary>
    /// Lobby room.
    /// </summary>
    public static class LobbyRoom
    {
        private static readonly string _message = "LobbyRoomMessage";

        /// <summary>
        /// Register the specified connection.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="connection">Connection.</param>
        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_message, LobbyRoomMessageHandler);
        }

        /// <summary>
        /// Unregister the specified connection.
        /// </summary>
        /// <returns>The unregister.</returns>
        /// <param name="connection">Connection.</param>
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
