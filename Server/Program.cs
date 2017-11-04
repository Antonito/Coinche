using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server
{
    class App
    {
        static void Main(string[] args)
        {
            // Register packets handler
            RegisterPackets();

            // Start listening for connections
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            // Print out the IPs and ports we are now listening on
            Console.WriteLine("Server listening for TCP connection on:");
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
            {
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);
            }

            //Let the user close the server
            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

            //We have used NetworkComms so we should ensure that we correctly call shutdown
            NetworkComms.Shutdown();
        }

        // Get all class inheriting from IMessage and register them
        private static void RegisterPackets()
        {
            var type = typeof(Packet.IPacket);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);
            foreach (var curType in types) {
                Console.WriteLine("Type is {0}", curType.ToString());
                Packet.IPacket instance = (Packet.IPacket)Activator.CreateInstance(curType);
                instance.Register();
            }
        }
    }
}