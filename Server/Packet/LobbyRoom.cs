using System;
using System.Collections.Generic;
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
                if (lobbyConnection != connection)
                {
                    lobbyConnection.SendObject(_messageType, msg);
                }
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
            var connectInfos = ConnectionManager.Get(connection);

            try
            {
                if (connectInfos.Lobby.IsStarted == false)
                {
                    // All others players stay in the lobby because the game is not started yet
                    Console.WriteLine("[Debug] we disconnect one client");
                    DisconnectOnePlayer(connection, connectInfos);
                }
                else
                {
                    // The game is launched, so we kick all players from the lobby
                    var cos = connectInfos.Lobby.Connection.ToArray();
                    foreach (var lobbyConnection in cos)
                    {
                        Console.WriteLine("we got lobbyConnection");
                        var lobbyConnectInfos = ConnectionManager.Get(lobbyConnection);
                        Console.WriteLine("we got lobbyConnectI");
                        DisconnectOnePlayer(lobbyConnection, lobbyConnectInfos);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Disconnects one player from a lobby.
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <param name="connectInfos">Connect infos.</param>
        private static void DisconnectOnePlayer(Connection connection, ConnectionInformation connectInfos)
        {
            var room = connectInfos.Lobby.Name;
            var pseudo = connectInfos.Pseudo;
            Console.WriteLine("[LobbyRoom - " + room + "] " + pseudo + " Disconnected.");
            connectInfos.Lobby.RemovePlayer(connection);
            connectInfos.Lobby = null;
            Unregister(connection);
            NetworkGame.Unregister(connection);
            SelectLobby.Register(connection);
            connection.SendObject("LobbySelect");
        }
    }
}
