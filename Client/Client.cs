using System;
using System.Linq;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;

#if HAS_SSL
using System.Collections.Generic;
using System.Text;
using Coinche.Common;
#endif

namespace Coinche.Client
{
    class Program
    {
        static public ClientInformation clientInfos = new ClientInformation();

        static void Main(string[] args)
        {
            try
            {
#if HAS_SSL
                SSL _ssl = new SSL("coinche_cli.pfx");
#endif

                // Request server IP and port number
                Console.WriteLine("Please enter the server IP and port in the format 192.168.0.1:10000 and press return:");
                string serverInfo = Console.ReadLine();

                // Parse the necessary information out of the provided string
                string serverIP = serverInfo.Split(':').First();
                int serverPort = int.Parse(serverInfo.Split(':').Last());

                ConnectionInfo connInfo = new ConnectionInfo(serverIP, serverPort);
#if HAS_SSL
                Connection co = TCPConnection.GetConnection(connInfo, _ssl.SendingSendReceiveOptions, _ssl.ConnectionOptions);
#else
                Connection co = TCPConnection.GetConnection(connInfo);
#endif
                co.AppendIncomingPacketHandler<string>("WelcomeResponse", WelcomeResponseHandler);

                // Send the message in a single line
                Console.WriteLine("Please enter your pseudonyme:");
                string pseudo = Console.ReadLine();
                co.SendObject("WelcomeRequest", pseudo);


                while (co.ConnectionAlive())
                {
                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine("Please double check that the server is up and running and try again.");
            }

            finally
            {
                // We have used comms so we make sure to call shutdown
                NetworkComms.Shutdown();   
            }
        }

        private static void WelcomeResponseHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
            connection.RemoveIncomingPacketHandler("WelcomeResponse");
            Lobby.Register(connection);
            Lobby.Connect(connection);
        }
    }
}