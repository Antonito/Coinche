using System.Collections.Concurrent;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server
{
    /// <summary>
    /// Connection manager.
    /// </summary>
    public static class ConnectionManager
    {
        /// <summary>
        /// The connections.
        /// </summary>
        private static ConcurrentDictionary<Connection, ConnectionInformation> 
        _connections = new ConcurrentDictionary<Connection, ConnectionInformation>();

        /// <summary>
        /// Add the specified connection.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public static void Add(Connection connection) {
            _connections[connection] = new ConnectionInformation(connection);
        }

        /// <summary>
        /// Remove the specified connection.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public static void Remove(Connection connection) {
            _connections.TryRemove(connection, out ConnectionInformation infos);
        }

        /// <summary>
        /// Get the specified connection.
        /// </summary>
        /// <returns>The connection.</returns>
        /// <param name="connection">Connection.</param>
        public static ConnectionInformation Get(Connection connection) {
            return _connections[connection];
        }
    }
}
