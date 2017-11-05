using System;
using System.Collections.Generic;
using System.Linq;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

#if HAS_SSL
using System.Net;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Tools;
using Coinche.Common;
#endif

namespace Coinche.Server
{
    class App
    {
        static private readonly List<Lobby> _lobbies = new List<Lobby>();

        static void Main(string[] args)
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

        // Add a connection to the ConnectManager, for further use
        // and register a first packet handler
        private static void AddClient(Connection connection)
        {
            ConnectionManager.Add(connection);
            Packet.Welcome.Register(connection);
        }

        // Delete a client from the ConnectManager
        private static void RemoveClient(Connection connection)
        {
            var connectInfos = ConnectionManager.Get(connection);
            if (connectInfos.Lobby != null)
            {
                // Disconnect client from the current lobby
                var nbPlayers = connectInfos.Lobby.NbPlayers;
                connectInfos.Lobby.RemovePlayer(connection);
                if (nbPlayers == 1)
                {
                    // If there is no other player, delete the room
                    DeleteLobby(connectInfos.Lobby);
                }
            }

            ConnectionManager.Remove(connection);
        }

        static public void AddLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                throw new Exceptions.LobbyError("Lobby already exists");
            }
            _lobbies.Add(new Lobby(name));
        }

        static public Lobby GetLobby(Lobby lobby)
        {
            return _lobbies.FirstOrDefault(l => l.Name == lobby.Name);
        }

        static public Lobby GetLobby(string name)
        {
            return _lobbies.FirstOrDefault(l => l.Name == name);
        }

        static public void DeleteLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                DeleteLobby(matchingvalue);
            }
        }

        static public void DeleteLobby(Lobby lobby)
        {
            _lobbies.Remove(lobby);
        }
    }
}