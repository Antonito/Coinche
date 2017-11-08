using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    /// <summary>
    /// Network contract.
    /// </summary>
    public static class NetworkContract
    {
        private static readonly string _type = "Contract";

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
            // TODO
        }
    }
}
