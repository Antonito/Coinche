using System;
using System.Linq;
using System.Collections.Generic;

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

        public Match()
        {
            //TODO: set players and teams
            _players = new List<Player>();
            _teams = new List<Team>();

            //TODO: create players first !!!
            _teams.Add(new Team(_players[0], _players[1]));
            _teams.Add(new Team(_players[2], _players[3]));
            _games = new List<Game>();
        }

        public void Run()
        {
            Console.WriteLine("Starting Match between both teams");
            while (!_teams[0].HasWon() && !_teams[1].HasWon())
            {
                Game game = new Game(_teams);
                game.Run();

                //TODO: check if it is necessary
                // Store the game history for futur usage
                _games.Add(game);
            }
        }

        //TODO: return a Team instead of an int ?
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
