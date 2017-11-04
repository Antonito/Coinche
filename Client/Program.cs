using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;

namespace Coinche.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Request server IP and port number
            Console.WriteLine("Please enter the server IP and port in the format 192.168.0.1:10000 and press return:");
            string serverInfo = Console.ReadLine();

            //Parse the necessary information out of the provided string
            string serverIP = serverInfo.Split(':').First();
            int serverPort = int.Parse(serverInfo.Split(':').Last());

            //Send the message in a single line
            string welcomeMsg = "Hello NetCoinche !";
            NetworkComms.SendObject("Welcome", serverIP, serverPort, welcomeMsg);
            NetworkComms.SendObject("Welcome", serverIP, serverPort, welcomeMsg);

            //Check if user wants to go around the loop
            Console.ReadLine();

            //We have used comms so we make sure to call shutdown
            NetworkComms.Shutdown();
        }
    }
}