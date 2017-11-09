using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

#if HAS_SSL
using System.Net;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Tools;
using Coinche.Common;
#endif

namespace Coinche.Server
{
    /// <summary>  
    /// Entry point of the application
    /// </summary> 
    class App
    {
        /// <summary>
        /// Main function
        /// </summary>
        static void Main()
        {
            try
            {
#if HAS_SSL
                SSL _ssl = new SSL("coinche_srv.pfx");
#endif

                // Register packets handler
                NetworkComms.AppendGlobalConnectionEstablishHandler(AddClient);
                NetworkComms.AppendGlobalConnectionCloseHandler(RemoveClient);

#if HAS_SSL
                List<IPEndPoint> desiredlocalEndPoints = (from current in HostInfo.IP.FilteredLocalAddresses()
                                                          select new IPEndPoint(current, 0)).ToList();

                // Create a list of matching TCP listeners where we provide the listenerSSLOptions
                List<ConnectionListenerBase> listeners = (from current in desiredlocalEndPoints
                                                          select (ConnectionListenerBase)(new TCPConnectionListener(NetworkComms.DefaultSendReceiveOptions,
                                                               ApplicationLayerProtocolStatus.Enabled,
                                                               _ssl.ListenerOptions))).ToList();

                // Start listening for connections
                Connection.StartListening(listeners, desiredlocalEndPoints, true);

#else
                Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
#endif

                // Print out the IPs and ports we are now listening on
                Console.WriteLine("Server listening for TCP connection on:");
                foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                {
                    Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                // Let the user close the server
                Console.WriteLine("\nPress any key to close server.");
                Console.ReadKey(true);

                // We have used NetworkComms so we should ensure that we correctly call shutdown
                NetworkComms.Shutdown();
            }
        }

        /// <summary>
        /// Add a connection to the ConnectManager, for further user
        /// and register a first packet handler.
        /// </summary>
        /// <param name="connection">Client's connection</param>
        private static void AddClient(Connection connection)
        {
            ConnectionManager.Add(connection);
            Packet.Welcome.Register(connection);
        }

        /// <summary>
        /// Delete a client from the ConnectManager
        /// This method is called is a client exit during a game
        /// Then all client are kicked from the lobby
        /// </summary>
        /// <param name="connection">Connection.</param>
        public static void RemoveClient(Connection connection)
        {
            connection.CloseConnection(true);

            var connectInfos = ConnectionManager.Get(connection);
            var pseudo = connectInfos.Pseudo;
            Console.WriteLine("Client: " + pseudo + " left.");
            if (connectInfos.Lobby != null)
            {
                // Disconnect client from the current lobby
                var nbPlayers = connectInfos.Lobby.NbPlayers;
                connectInfos.Lobby.RemovePlayer(connection);
            }
            ConnectionManager.Remove(connection);
        }
    }
}