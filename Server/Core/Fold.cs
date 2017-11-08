using System;
using System.Linq;
using System.Collections.Generic;
using static Coinche.Server.Core.Game;

namespace Coinche.Server.Core
{
    public sealed class Fold
    {
        private readonly List<Player> _players;
        private readonly Game.GameMode _gameMode;
        private Deck _deck;

        public Fold(List<Player> players, GameMode gameMode)
        {
            _players = players;
            _gameMode = gameMode;
        }

        public void Run()
        {
            //TODO: add fold cards into player fold list
            //ask player for a card
            //check if the card can be played ( no cheat allowed )
            //someone win the fold
        }
    }
}
