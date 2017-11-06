using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    /// <summary>
    /// This packet is the first packet received by the server
    /// The received message should be the pseudo of the player
    /// </summary>
    public static class Welcome
    {
        private static readonly string _type = "WelcomeRequest";
     
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

        /// <summary>
        /// Gets the pseudo of a player, welcomes the player, then switch to lobby selection
        /// </summary>
        /// <returns>The handler.</returns>
        /// <param name="header">Header.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="message">Message.</param>
        private static void Handler(PacketHeader header, Connection connection, string message)
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
