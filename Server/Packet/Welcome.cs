using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    // This packet is the first packet received by the server
    // The received message should be the pseudo of the player
    public static class Welcome
    {
        private static readonly string _type = "WelcomeRequest";
        
        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_type, WelcomeClient);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);   
        }

        // Gets the pseudo of a player, welcomes the player, then switch to lobby selection
        private static void WelcomeClient(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("[" + connection.ToString() + "] Player " + 
                               message + " logged to the game.");
            Unregister(connection);
            SelectLobby.Register(connection);
            ConnectionManager.Get(connection).Pseudo = message;
                connection.SendObject("WelcomeResponse", "Welcome To NetCoinche");
         }
    }
}
