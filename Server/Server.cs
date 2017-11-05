﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Tools;
using Coinche.Common;

namespace Coinche.Server
{
    class App
    {
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
            ConnectionManager.Remove(connection);
        }
    }
}