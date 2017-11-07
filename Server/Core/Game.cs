using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Game.
    /// </summary>
    public sealed class Game
    {
        /// <summary>
        /// Game mode.
        /// </summary>
        public enum GameMode
        {
            Classic,
            AllAssets,
            NoAsset
        }

        /// <summary>
        /// The players.
        /// </summary>
        private readonly List<Player> _players;

        /// <summary>
        /// The teams.
        /// </summary>
        private readonly List<Team> _teams;

        /// <summary>
        /// History of folds.
        /// </summary>
        private readonly List<Fold> _folds;

        /// <summary>
        /// Gets the number of folds.
        /// </summary>
        /// <value>The number of folds.</value>
        public int NumberOfFolds { get { return _folds.Count(); } }

        // IMPORTANT MUST READ
        // TODO: Re-organize this.
        // This class should contain only informations about 1 game,
        // not looping until a team wins. A team should only be capable of
        // winning the match after they played several games.
        //
        // -> A Game is a few folds.
        // -> A Match is a few games.
        // -> The team with >= Team.MaxPScore wins a Match.

        public Game(List<Player> players, GameMode mode, Card.CardColor? asset)
        {
            // A few validity checks
            if (players.Count() != 4)
            {
                throw new ArgumentException("Invalid number of players (must be 4)");
            }
            if (mode != GameMode.Classic && asset != null)
            {
                throw new ArgumentException("Asset must be null if GameMode is not classic.");
            }
            if (mode == GameMode.Classic && asset == null)
            {
                throw new ArgumentException("Asset must not be null if GameMode is classic.");
            }
            _players = players;
            _folds = new List<Fold>();

            //while not enought point to terminate the game
            _teams = new List<Team>();
            _teams.Add(new Team(_players[0], _players[1]));
            _teams.Add(new Team(_players[2], _players[3]));
            while (!_teams[0].HasWon() && !_teams[1].HasWon())
            {
                Fold fold = new Fold(_players);
                fold.Compute();
                fold.SetResult(_teams);

                //optional
                _folds.Add(fold);
            }
            //Game ended
        }
    }
}
