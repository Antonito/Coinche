using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public sealed class Player
    {
        private List<Card> _cardsHand;
        private List<Card> _cardsFold;

        public List<Card> Hand { get { return _cardsHand; } }
        public List<Card> Folds { get { return _cardsFold; } set { _cardsFold.AddRange(value); } }

        public Player()
        {
            _cardsHand = new List<Card>();
            _cardsFold = new List<Card>();
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
                points += card.Value;
            }
            return points;
        }

        public void ResetCards()
        {
            _cardsHand = new List<Card>();
            _cardsFold = new List<Card>();
        }
    }
}
