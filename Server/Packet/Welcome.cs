using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Packet
{
    // This message is the first received packet, must be 
    public class Welcome : IPacket
    {
        public void Register()
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Welcome", WelcomeClient);
        }

        private static void WelcomeClient(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nA message was received from " + 
                              connection.ToString() + " which said '" + 
                              message + "'.");
            RemoveIncomingPacketHandler("Welcome");
            if (message != "Hello NetCoinche !")
            {
                connection.CloseConnection(true);
            }
        }
    }
}
