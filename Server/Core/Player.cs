using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Player
    {
        private List<Card> _cards;

        public List<Card> Hand { get { return _cards; } }

        public Player()
        {
            _cards = new List<Card>();
        }

        public void GiveCard(Card card)
        {
            _cards.Add(card);
            if (_cards.Count() > 8)
            {
                throw new ArgumentException("Added too many cards");
            }
        }
    }
}
