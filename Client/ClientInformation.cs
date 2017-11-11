using System;
using System.Collections.Generic;
using System.Linq;
using Coinche.Common.Core.Cards;
using Coinche.Common.PacketType;

namespace Coinche.Client
{
    /// <summary>
    /// Client information.
    /// </summary>
    public class ClientInformation
    {
        /// <summary>
        /// The lock
        /// </summary>
        private readonly Object thisLock = new Object();

        /// <summary>
        /// The cards.
        /// </summary>
        private readonly List<Card> _cards;

        /// <summary>
        /// The cards played.
        /// </summary>
        private readonly List<Card> _cardsPlayed;

        public bool IsRun { get; set; }
        public bool CanRecoinche { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Client.ClientInformation"/> class.
        /// </summary>
        public ClientInformation()
        {
            IsRun = false;
            CanRecoinche = false;
            _cards = new List<Card>();
            _cardsPlayed = new List<Card>();
        }

        /// <summary>
        /// Resets the cards.
        /// </summary>
        public void ResetCards()
        {
            lock (thisLock)
            {
                _cards.Clear();
                _cardsPlayed.Clear();
            }
        }

        /// <summary>
        /// Adds the card.
        /// </summary>
        /// <param name="card">Card.</param>
        public void AddCard(PlayCard card)
        {
            lock (thisLock)
            {
                _cards.Add(new Card(card.CardValue, card.CardColor));
            }
        }

        /// <summary>
        /// Reverts the cards.
        /// </summary>
        public void RevertPlay()
        {
            lock (thisLock)
            {
                _cards.Add(_cardsPlayed.Last());
                _cardsPlayed.Remove(_cardsPlayed.Last());
            }
        }

        /// <summary>
        /// Gets the cards.
        /// </summary>
        /// <returns>The cards.</returns>
        public List<Card> GetCards()
        {
            lock (thisLock)
            {
                return _cards;
            }
        }

        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        /// <returns>The card type.</returns>
        /// <param name="index">Index.</param>
        public CardType GetCardType(int index)
        {
            return _cards[index].Value;
        }

        /// <summary>
        /// Gets the color of the card.
        /// </summary>
        /// <returns>The card color.</returns>
        /// <param name="index">Index.</param>
        public CardColor GetCardColor(int index)
        {
            return _cards[index].Color;
        }

        /// <summary>
        /// Plays the card.
        /// </summary>
        /// <param name="index">Index.</param>
        public void PlayCard(int index)
        {
            lock (thisLock)
            {
                _cardsPlayed.Add(_cards[index]);
                _cards.RemoveAt(index);
            }
        }
    }
}
