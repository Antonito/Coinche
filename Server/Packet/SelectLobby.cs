using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    /// <summary>
    /// Handle lobby creation or selection
    /// </summary>
    public static class SelectLobby
    {
        private static readonly string _type = "SelectLobby";

        /// <summary>
        /// Register the specified connection.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="connection">Connection.</param>
        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_type, Handler);
        }

        /// <summary>
        /// Unregister the specified connection.
        /// </summary>
        /// <returns>The unregister.</returns>
        /// <param name="connection">Connection.</param>
        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
        }


        private static void Handler(PacketHeader header, Connection connection, string message)
        {
            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            Console.WriteLine("Player " + 
                              pseudo + 
                              " selected lobby " + message);
            // Check if requested lobby exists.
            // I.  If it exists, try to join it.
            //       1) If it is full -> send error back to client and ask for another lobby
            //       2) Else -> Unregister this handler, respond to client, and switch to lobby room
            // II. If it does not exist, create lobby, respond to client, and switch to lobby room

            var lobby = LobbyManager.GetLobby(message);
            if (lobby == null)
            {
                // Create lobby
                Console.WriteLine("Player " + pseudo + " created lobby " + message);
                LobbyManager.AddLobby(message);
                lobby = LobbyManager.GetLobby(message);
            }
            try
            {
                lobby.AddPlayer(connection);
                connectInfos.Lobby = lobby;
                Unregister(connection);
                LobbyRoom.Register(connection);
                connection.SendObject("LobbyInfo", "Joined Lobby " + message + ": there are " + lobby.NbPlayers + " players.");
                NetworkGame.Register(connection);
                NetworkGame.AskClientReady(connection);
                if (lobby.IsFull())
                {
                    // TODO: start game here
                    Console.WriteLine("preparing game");
                }
            }
            catch (Exception e)
            {
                connection.SendObject("LobbyError", e.Message);
            }
        }
    }
}
