using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Deck
    {
        private readonly List<Card> _cards;

        public List<Card> Cards { get { return _cards; } }

        public Deck(Game.GameMode gameMode, Card.CardColor assetColor)
        {
            if (gameMode != Game.GameMode.Classic)
            {
                throw new ArgumentException("GameMode must be Classic.");
            }
            _cards = CreateClassicDeck(assetColor);
        }

        public Deck(Game.GameMode gameMode)
        {
            if (gameMode == Game.GameMode.Classic)
            {
                throw new ArgumentException("GameMode must not be Classic.");
            }
            _cards = CreateAssetsDeck(gameMode);
        }

        public void DistributeCards(List<Player> players) {
            if (players.Count() != 4) {
                throw new ArgumentException("Invalid number of players, must be 4.");
            }
            var shuffledCards = _cards.OrderBy(a => Guid.NewGuid()).ToList();

            // Distribute 3 cards, then 2, then 3.
            foreach (Player player in players) {
                for (int i = 0; i < 3; ++i)
                {
                    player.GiveCard(shuffledCards[0]);
                    shuffledCards.RemoveAt(0);
                }
            }
            foreach (Player player in players)
            {
                for (int i = 0; i < 2; ++i)
                {
                    player.GiveCard(shuffledCards[0]);
                    shuffledCards.RemoveAt(0);
                }
            }
            foreach (Player player in players)
            {
                for (int i = 0; i < 3; ++i)
                {
                    player.GiveCard(shuffledCards[0]);
                    shuffledCards.RemoveAt(0);
                }
            }
        }

        static private List<Card> CreateClassicDeck(Card.CardColor assetColor) {
            List<Card> cards = new List<Card>();

            foreach (Card.CardColor color in 
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                foreach (Card.CardType type in 
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    bool isAsset = (color == assetColor) ? true : false;

                    cards.Add(new Card(type, color, isAsset, 
                                           Game.GameMode.Classic));
                }
            }
            return cards;
        }

        static private List<Card> CreateAssetsDeck(Game.GameMode gameMode)
        {
            List<Card> cards = new List<Card>();
            bool isAsset = (gameMode == Game.GameMode.AllAssets) ? true : false;

            foreach (Card.CardColor color in 
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                foreach (Card.CardType type in 
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    cards.Add(new Card(type, color, isAsset, gameMode));
                }
            }
            return cards;
        }
    }
}
