using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;

namespace Coinche.Server.Core
{
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
        /// The deck.
        /// </summary>
        private Deck _deck;

        /// <summary>
        /// The folds won.
        /// </summary>
        private int _foldsWon;

        /// <summary>
        /// The connection.
        /// </summary>
        private Connection _connection;

        /// <summary>
        /// Gets the hand.
        /// </summary>
        /// <value>The hand.</value>
        public List<Card> Hand { get { return _cardsHand; } }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public int Score { get; set; }

        /// <summary>
        /// Returns the victories.
        /// </summary>
        /// <value>The victories.</value>
        public int Victories { get { return _foldsWon; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Coinche.Server.Core.Player"/> hand is empty.
        /// </summary>
        /// <value><c>true</c> if hand is empty; otherwise, <c>false</c>.</value>
        public bool IsHandEmpty { get { return Hand.Count == 0; } }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public Connection Connection
        {
            get
            {
                if (_connection == null)
                {
                    throw new Exceptions.PlayerError("Connection is not set.");
                }
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        /// <summary>
        /// Is unit testing ?
        /// </summary>
        private readonly bool _unitTest;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Player"/> class.
        /// </summary>
        /// <param name="deck">Deck.</param>
        public Player(bool unitTest = false)
        {
            _cardsHand = new List<Card>();
            _deck = null;
            _foldsWon = 0;
            _unitTest = unitTest;
            Score = 0;
        }

        /// <summary>
        /// Gives the deck.
        /// </summary>
        /// <param name="deck">Deck.</param>
        public void GiveDeck(Deck deck)
        {
            _deck = deck;
        }

        /// <summary>
        /// Gives the card.
        /// </summary>
        /// <param name="card">Card.</param>
        public void GiveCard(Card card)
        {
            if (_deck == null)
            {
                throw new Exceptions.PlayerError("Player must have a deck associated.");
            }

            if (!_unitTest)
            {
                Console.WriteLine("Giving card to player " + 
                                  ConnectionManager.Get(_connection).Pseudo + ": " + 
                                  card.Type.ToString() + " | " + card.Color.ToString());
                PlayCard cardPck = new PlayCard
                {
                    CardValue = card.Type,
                    CardColor = card.Color
                };
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, cardPck);
                    _connection.SendObject("PlayerGetGameCard", stream.ToArray());
                }
            }

            _cardsHand.Add(card);
            if (_cardsHand.Count > 8)
            {
                throw new ArgumentException("Added too many cards");
            }
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
            _foldsWon = 0;
            _deck = null;
            Score = 0;
        }

        /// <summary>
        /// Play the card.
        /// </summary>
        /// <param name="card">Card.</param>
        public void PlayCard(Card card)
        {
            var cur = _cardsHand.FirstOrDefault(c => c.Color == card.Color && c.Type == card.Type);
            Score += _deck.GetCardValue(cur);
            _cardsHand.Remove(cur);
        }

        /// <summary>
        /// Check if has an asset
        /// </summary>
        /// <returns><c>true</c>, if asset was had, <c>false</c> otherwise.</returns>
        public bool HaveAsset()
        {
            return _cardsHand.Any(_deck.IsCardAsset);
        }

        /// <summary>
        /// Check if the player has the card
        /// </summary>
        /// <returns><c>true</c>, if player has the card, <c>false</c> otherwise.</returns>
        public bool HaveCard(Card card)
        {
            return _cardsHand.Count(c => 
            {
                return c.Color == card.Color && c.Type == card.Type;
            }) == 1;
        }

        /// <summary>
        /// Check if has a card of the color
        /// </summary>
        /// <returns><c>true</c>, if color was had, <c>false</c> otherwise.</returns>
        /// <param name="color">Color.</param>
        public bool HaveColor(Common.Core.Cards.CardColor color)
        {
            return _cardsHand.Any(c => 
            {
                return c.Color == color;
            });
        }
    }
}
