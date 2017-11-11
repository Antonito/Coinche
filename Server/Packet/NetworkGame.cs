using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using Coinche.Common.PacketType;
using ProtoBuf;
using NetworkCommsDotNet.DPSBase;
using System.IO;

namespace Coinche.Server.Packet
{
    public static class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";
        private static readonly string _winner = "MatchWinner";
        private static int _gameReadyCount = 0;

        /// <summary>
        /// Register the specified connection.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="connection">Connection.</param>
        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<byte[]>(_type, Handler);
        }

        /// <summary>
        /// Unregister the specified connection.
        /// </summary>
        /// <returns>The unregister.</returns>
        /// <param name="connection">Connection.</param>
        public static void Unregister(Connection connection)
        {
            var connectInfos = ConnectionManager.Get(connection);
            if (connectInfos.IsGameReady)
            {
                Console.WriteLine("[debug] unregister {0} from game.", connectInfos.Pseudo);
                connectInfos.IsGameReady = false;
                _gameReadyCount--;
            }
            connection.RemoveIncomingPacketHandler(_type);
        }

        /// <summary>
        /// Handle the specified header, connection and game.
        /// </summary>
        /// <returns>The handler.</returns>
        /// <param name="header">Header.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="game">Game.</param>
        private static void Handler(PacketHeader header, Connection connection, byte[] game)
        {
            Console.WriteLine("[Debug] receiving Ready State from a client");

            // we get the ConnectInfos to access the client's stream (locally)
            var connectInfos = ConnectionManager.Get(connection);

            using (MemoryStream stream = new MemoryStream(game))
            {
                //and finllay we deserialize the data
                StartGame _g = Serializer.Deserialize<StartGame>(stream);

                if (!connectInfos.IsGameReady && _g.IsReady)
                {
                    _gameReadyCount++;
                    connectInfos.IsGameReady = true;
                    Console.WriteLine("{0} is ready", connectInfos.Pseudo);
                }
                if (_gameReadyCount == 4)
                {
                    PlayGame(connectInfos.Lobby);
                }
            }
        }

        /// <summary>
        /// Plays the game.
        /// </summary>
        /// <param name="lobby">Lobby.</param>
        private static void PlayGame(Lobby lobby)
        {
            // We must konw if the game is started or not when a player quits
            lobby.IsStarted = true;

            try
            {
                var match = new Core.Match(lobby.Connection);
                match.Run();
                var team = match.WinnerTeam();
                using (MemoryStream stream = new MemoryStream())
                {
                    MatchWinner win = new MatchWinner()
                    {
                        TeamWinner = match.WinnerId(),
                        PseudoA = ConnectionManager.Get(team.Players[0].Connection).Pseudo,
                        PseudoB = ConnectionManager.Get(team.Players[1].Connection).Pseudo
                    };
                    Serializer.Serialize(stream, win);
                    foreach (var connection in lobby.Connection)
                    {
                        connection.SendObject("MatchWinner", stream.ToArray());
                        lobby.RemovePlayer(connection);
                        connection.SendObject("LobbySelect");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[LobbyRoom - " + lobby.Name + "] Match has ended.");
            }
        }
    }
}
