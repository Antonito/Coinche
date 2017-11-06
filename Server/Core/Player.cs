using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    // TODO: We must re-create a new set of Player each time a Deck is created
    public sealed class Player
    {
        private readonly List<Card> _cardsHand;
        private readonly List<Card> _cardsFold;
        private readonly Deck _deck;

        public List<Card> Hand { get { return _cardsHand; } }
        public List<Card> Folds { get { return _cardsFold; } set { _cardsFold.AddRange(value); } }

        public Player(Deck deck)
        {
            _cardsHand = new List<Card>();
            _cardsFold = new List<Card>();
            _deck = deck;
        }

        public void GiveCard(Card card)
        {
            _cardsHand.Add(card);
            if (_cardsHand.Count() > 8)
            {
                throw new ArgumentException("Added too many cards");
            }
        }

        public int GetPoints()
        {
            int points = 0;
            foreach (var card in _cardsFold)
            {
                points += _deck.GetCardValue(card);
            }
            return points;
        }

        public void ResetCards()
        {
            _cardsHand.Clear();
            _cardsFold.Clear();
        }
    }
}
