using System;
using System.Net;
using Coinche.Server;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Tools;
using NUnit.Framework;

namespace Server
{
    [TestFixture]
    public class LobbyTest
    {
        [Test]
        // Tries to create 2 lobbies with different names
        public void CreateTwoLobbiesDifferentName()
        {
            string lobbyName1 = "toto";
            string lobbyName2 = "totoqwd";
            bool hasThrown = false;

            try
            {
                LobbyManager.AddLobby(lobbyName1);
                LobbyManager.AddLobby(lobbyName2);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            finally
            {
                LobbyManager.DeleteLobby(lobbyName1);
                LobbyManager.DeleteLobby(lobbyName2);
            }
            Assert.AreEqual(false, hasThrown);
        }

        [Test]
        // Tries to create 2 lobbies with same names
        public void CreateTwoLobbiesSameName()
        {
            string lobbyName1 = "toto";
            string lobbyName2 = lobbyName1;
            bool hasThrown = false;

            try
            {
                LobbyManager.AddLobby(lobbyName1);
                LobbyManager.AddLobby(lobbyName2);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            finally
            {
                LobbyManager.DeleteLobby(lobbyName1);
                LobbyManager.DeleteLobby(lobbyName2);
            }
            Assert.AreEqual(true, hasThrown);
        }

        [Test]
        // Create a lobby and add 1 player
        public void CreateLobbyAndAddOnePlayer()
        {
            bool hasThrown = false;
            string lobbyName = "toto";

            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            IPEndPoint lastServerIPEndPoint = (System.Net.IPEndPoint)Connection.ExistingLocalListenEndPoints(ConnectionType.TCP)[0];
            ConnectionInfo targetServerConnectionInfo = new 
                ConnectionInfo(lastServerIPEndPoint.Address.MapToIPv4().ToString(), 
                               lastServerIPEndPoint.Port);

            try
            {
                LobbyManager.AddLobby(lobbyName);
                TCPConnection connection = TCPConnection.GetConnection(targetServerConnectionInfo);
                LobbyManager.GetLobby(lobbyName).AddPlayer(connection);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            finally
            {
                LobbyManager.DeleteLobby(lobbyName);
            }
            NetworkComms.Shutdown();
            Assert.AreEqual(false, hasThrown);
        }

        [Test]
        // Create a lobby and add 1 player twice to the same lobby
        public void CreateLobbyAndAddOnePlayerAndSetTwiceLobby()
        {
            bool hasThrown = false;
            string lobbyName = "toto";

            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            IPEndPoint lastServerIPEndPoint = (System.Net.IPEndPoint)Connection.ExistingLocalListenEndPoints(ConnectionType.TCP)[0];
            ConnectionInfo targetServerConnectionInfo = new
                ConnectionInfo(lastServerIPEndPoint.Address.MapToIPv4().ToString(),
                               lastServerIPEndPoint.Port);

            try
            {
                LobbyManager.AddLobby(lobbyName);
                TCPConnection connection = TCPConnection.GetConnection(targetServerConnectionInfo);
                LobbyManager.GetLobby(lobbyName).AddPlayer(connection);
                LobbyManager.GetLobby(lobbyName).AddPlayer(connection);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            finally
            {
                LobbyManager.DeleteLobby(lobbyName);
            }
            NetworkComms.Shutdown();
            Assert.AreEqual(true, hasThrown);
        }

    }
}
