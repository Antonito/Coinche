using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;
using System.IO;
using Coinche.Common.PacketType;
using ProtoBuf;

namespace Coinche.Server.Core
{
    public class Match
    {
        /// <summary>
        /// The players.
        /// </summary>
        private readonly List<Player> _players;

        /// <summary>
        /// The teams.
        /// </summary>
        private readonly List<Team> _teams;

        /// <summary>
        /// The game list.
        /// </summary>
        private readonly List<Game> _games;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Match"/> class.
        /// </summary>
        /// <param name="connections">Connections.</param>
        public Match(List<Connection> connections)
        {
            if (connections.Count != 4)
            {
                throw new Exceptions.LobbyError("There must be 4 players");
            }
            _players = new List<Player>{
                new Player(),
                new Player(),
                new Player(),
                new Player()
            };
            for (var i = 0; i < connections.Count; ++i)
            {
                _players[i].Connection = connections[i];
            }
            _teams = new List<Team>
            {

                new Team(_players[0], _players[2]),
                new Team(_players[1], _players[3])
            };
            _games = new List<Game>();
        }

        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Starting Match between both teams");
            while (!_teams[0].HasWon() && !_teams[1].HasWon())
            {
                Game game = new Game(_teams);

                game.Run();

                // Store the game history for futur usage
                _games.Add(game);

                SendResult();
            }
        }

        private void SendResult()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                MatchWinner win = new MatchWinner()
                {
                    TeamWinner = WinnerId(),
                    PseudoA = ConnectionManager.Get(_teams[WinnerId()].Players[0].Connection).Pseudo,
                    PseudoB = ConnectionManager.Get(_teams[WinnerId()].Players[1].Connection).Pseudo
                };
                Serializer.Serialize(stream, win);
                foreach (var player in _players)
                {
                    player.Connection.SendObject("MatchWinner", stream.ToArray());
                    player.Connection.SendObject("LobbySelect");
                }
            }
        }

        /// <summary>
        /// Returns the team winner id.
        /// </summary>
        /// <returns>The winner.</returns>
        private int WinnerId()
        {
            if (_teams[0].HasWon())
                return 0;
            return 1;
        }
    }
}
