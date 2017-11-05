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
            connection.AppendIncomingPacketHandler<string>(_type, SelectLobbyClient);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
        }

        private static void SelectLobbyClient(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("Player " + 
                              ConnectionManager.Get(connection).Pseudo + 
                              " selected lobby " + message);
            // TODO: Check if requested lobby exists.
            // I.  If it exists, try to join it.
            //       1) If it is full -> send error back to client and ask for another lobby
            //       2) Else -> Unregister this handler, respond to client, and switch to game mode (register new handler)
            // II. If it does not exist, create lobby, respond to client, and switch to game
        }
    }
}
