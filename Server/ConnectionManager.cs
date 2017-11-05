using System.Collections.Concurrent;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server
{
    // This class manages connections
    public static class ConnectionManager
    {
        private static ConcurrentDictionary<Connection, ConnectionInformation> 
        _connections = new ConcurrentDictionary<Connection, ConnectionInformation>();

        public static void Add(Connection connection) {
            _connections[connection] = new ConnectionInformation(connection);
        }

        public static void Remove(Connection connection) {
            _connections.TryRemove(connection, out ConnectionInformation infos);
        }

        public static ConnectionInformation Get(Connection connection) {
            return _connections[connection];
        }
    }
}
