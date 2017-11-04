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
        private readonly GameMode _gameMode;
        private readonly Deck _deck;

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
            _gameMode = mode;
            if (asset == null)
            {
                _deck = new Deck(mode);
            }
            else
            {
                _deck = new Deck(mode, asset.Value);
            }

            // Distribute cards
            _deck.DistributeCards(_players);
        }
    }
}
