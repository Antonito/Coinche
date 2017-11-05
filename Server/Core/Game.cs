using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Game
    {
        public enum GameMode
        {
            Classic,
            AllAssets,
            NoAsset
        }

        private readonly List<Player> _players;
        private readonly List<Team> _teams;

        //we store all fold history
        private List<Fold> _folds;

        static private readonly int _maxPoints = 3000;

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
            while (_teams[0].Score < _maxPoints || _teams[1].Score < _maxPoints)
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
