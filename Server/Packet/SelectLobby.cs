using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    // Handle lobby creation or selection here
    public static class SelectLobby
    {
        private static readonly string _type = "SelectLobby";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_type, Handler);
        }

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

            var lobby = App.GetLobby(message);
            if (lobby == null)
            {
                // Create lobby
                Console.WriteLine("Player " + pseudo + " created lobby " + message);
                App.AddLobby(message);
                lobby = App.GetLobby(message);
            }
            try
            {
                lobby.AddPlayer(connection);
                connectInfos.Lobby = lobby;
                Unregister(connection);
                LobbyRoom.Register(connection);
                connection.SendObject("LobbyInfo", "Joined Lobby " + message + ": there are " + lobby.NbPlayers + " players.");
            }
            catch (Exception e)
            {
                connection.SendObject("LobbyError", e.Message);
            }
        }
    }
}
