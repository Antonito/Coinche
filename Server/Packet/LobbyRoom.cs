using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    /// <summary>
    /// Lobby room.
    /// </summary>
    public static class LobbyRoom
    {
        private static readonly string _type = "LobbyRoom";
        private static readonly string _messageType = "LobbyRoomMessage";
        private static readonly string _quit = "LobbyRoomQuit";

        /// <summary>
        /// Register the specified connection.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="connection">Connection.</param>
        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_type, Handler);
            connection.AppendIncomingPacketHandler<string>(_messageType, MessageHandler);
            connection.AppendIncomingPacketHandler<string>(_quit, QuitHandler);
        }

        /// <summary>
        /// Unregister the specified connection.
        /// </summary>
        /// <returns>The unregister.</returns>
        /// <param name="connection">Connection.</param>
        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
            connection.RemoveIncomingPacketHandler(_messageType);
            connection.RemoveIncomingPacketHandler(_quit);
        }

        /// <summary>
        /// Handle the specified header, connection and message.
        /// </summary>
        /// <returns>The handler.</returns>
        /// <param name="header">Header.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="message">Message.</param>
        private static void Handler(PacketHeader header, Connection connection, string message)
        {
            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            var room = connectInfos.Lobby.Name;
            Console.WriteLine("[LobbyRoom - " + room + "] " + pseudo + ": " + message);
        }

        /// <summary>
        /// Dispatch a message sent by a connection to the other players (chat)
        /// </summary>
        /// <param name="header">Header.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="message">Message.</param>
        private static void MessageHandler(PacketHeader header, Connection connection, string message)
        {
            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            var room = connectInfos.Lobby.Name;
            var msg = "[" + room + "] " + pseudo + ": " + message;

            foreach (var lobbyConnection in connectInfos.Lobby.Connection)
            {
                lobbyConnection.SendObject(_messageType, msg);
            }
        }

        /// <summary>
        // Disconnect a client from this room, and returns to the room selection
        /// </summary>
        /// <param name="header">Header.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="message">Message.</param>
        private static void QuitHandler(PacketHeader header, Connection connection, string message)
        {
            try
            {
                var connectInfos = ConnectionManager.Get(connection);
                connectInfos.Lobby.RemovePlayer(connection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
