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
    /// <summary>
    /// Lobby test.
    /// </summary>
    [TestFixture]
    public class LobbyTest
    {
        /// <summary>
        /// Tries to create 2 lobbies with different names
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tries to create 2 lobbies with same names
        /// </summary>
        [Test]
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

        /// <summary>
        /// Create a lobby and delete it by name
        /// </summary>
        [Test]
        public void CreateLobbyAndGetByName()
        {
            string lobbyName1 = "toto";

            LobbyManager.AddLobby(lobbyName1);
            Assert.AreNotEqual(null, LobbyManager.GetLobby(lobbyName1));
            LobbyManager.DeleteLobby(lobbyName1);
            Assert.AreEqual(null, LobbyManager.GetLobby(lobbyName1));
        }

        /// <summary>
        /// Create a lobby and delete it by value
        /// </summary>
        [Test]
        public void CreateLobbyAndGetByValue()
        {
            string lobbyName1 = "toto";

            LobbyManager.AddLobby(lobbyName1);
            var lobby = LobbyManager.GetLobby(lobbyName1);
            Assert.AreNotEqual(null, lobby);
            LobbyManager.DeleteLobby(lobby);
            Assert.AreEqual(null, LobbyManager.GetLobby(lobbyName1));
        }

        /// <summary>
        /// Creates the lobby and add one player.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Creates a lobby and add one player twice to the same lobby
        /// </summary>
        [Test]
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
