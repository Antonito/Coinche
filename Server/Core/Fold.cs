using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Fold
    {
        private readonly List<Player> _players;
        private Game.GameMode _gameMode;
        private Deck _deck;

        public Fold(List<Player> players)
        {
            //distribute cards
            //wait for contrat - passe - coinche - re coinche
            //playCard
            //atribute point


            _players = players;
            //Todo: set gameMode by contract
            _gameMode = Game.GameMode.Classic;
            _deck = new Deck(_gameMode);

            /*if (asset == null)
            {
                _deck = new Deck(mode);
            }
            else
            {
                _deck = new Deck(mode, asset.Value);
            }*/

            // Distribute cards
            _deck.DistributeCards(_players);

        }

        public void Compute()
        {
            // TODO: set contrat
            //       check fold
            //       add fold cards into player fold list
        }

        public void SetResult(List<Team> teams)
        {
            int scoreTeam = _players[0].GetPoints() + _players[1].GetPoints();
            teams[0].AddScore(scoreTeam);

            scoreTeam = _players[2].GetPoints() + _players[3].GetPoints();
            teams[1].AddScore(scoreTeam);

            ResetPlayers();
        }

        private void ResetPlayers()
        {
            foreach (var player in _players)
            {
                player.ResetCards();
            }
        }
    }
}
