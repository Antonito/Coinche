using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    // TODO: We must re-create a new set of Player each time a Deck is created
    /// <summary>
    /// Player.
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// The hand (cards).
        /// </summary>
        private readonly List<Card> _cardsHand;

        /// <summary>
        /// The folds (cards).
        /// </summary>
        private readonly List<Card> _cardsFold;

        /// <summary>
        /// The deck.
        /// </summary>
        private readonly Deck _deck;

        /// <summary>
        /// The folds won.
        /// </summary>
        private int _foldsWon;

        /// <summary>
        /// Gets the hand.
        /// </summary>
        /// <value>The hand.</value>
        public List<Card> Hand { get { return _cardsHand; } }

        /// <summary>
        /// Gets or sets the folds.
        /// </summary>
        /// <value>The folds.</value>
        public List<Card> Folds { get { return _cardsFold; } set { _cardsFold.AddRange(value); } }

        /// <summary>
        /// Returns the victories.
        /// </summary>
        /// <value>The victories.</value>
        public int Victories { get { return _foldsWon; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Player"/> class.
        /// </summary>
        /// <param name="deck">Deck.</param>
        public Player(Deck deck)
        {
            _cardsHand = new List<Card>();
            _cardsFold = new List<Card>();
            _deck = deck;
            _foldsWon = 0;
        }

        /// <summary>
        /// Gives the card.
        /// </summary>
        /// <param name="card">Card.</param>
        public void GiveCard(Card card)
        {
            _cardsHand.Add(card);
            if (_cardsHand.Count() > 8)
            {
                throw new ArgumentException("Added too many cards");
            }
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <returns>The points.</returns>
        public int GetPoints()
        {
            int points = 0;
            foreach (var card in _cardsFold)
            {
                points += _deck.GetCardValue(card);
            }
            return points;
        }

        /// <summary>
        /// Has won the fold !
        /// </summary>
        public void WinFold()
        {
            ++_foldsWon;
        }

        /// <summary>
        /// Resets the cards.
        /// </summary>
        public void ResetCards()
        {
            _cardsHand.Clear();
            _cardsFold.Clear();
        }
    }
}
