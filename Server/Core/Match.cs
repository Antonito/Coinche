using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;

namespace Coinche.Server.Core
{
    public class Match
    {
        //TODO: is pertinent to set SetOnce on collection type like list ?

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

                new Team(_players[0], _players[1]),
                new Team(_players[2], _players[3])
            };
            _games = new List<Game>();
        }

        public void Run()
        {
            Console.WriteLine("Starting Match between both teams");
            while (!_teams[0].HasWon() && !_teams[1].HasWon())
            {
                Game game = new Game(_teams);

                game.Run();

                // TODO: check if it is necessary
                // Store the game history for futur usage
                _games.Add(game);
            }
        }

        // TODO: return a Team instead of an int ?
        /// <summary>
        /// Returns the team winner.
        /// </summary>
        /// <returns>The winner.</returns>
        public int Winner()
        {
            return 0;
        }
    }
}
