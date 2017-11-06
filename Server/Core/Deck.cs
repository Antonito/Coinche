using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Deck.
    /// </summary>
    public sealed class Deck
    {
        /// <summary>
        /// The cards.
        /// </summary>
        private readonly List<Card> _cards;

        /// <summary>
        /// Gets the cards.
        /// </summary>
        /// <value>The cards.</value>
        public List<Card> Cards { get { return _cards; } }

        /// <summary>
        /// The game mode.
        /// </summary>
        private Game.GameMode? _gameMode;

        /// <summary>
        /// The color of the asset.
        /// </summary>
        private Card.CardColor? _assetColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Deck"/> class.
        /// </summary>
        public Deck()
        {
            _gameMode = null;
            _assetColor = null;
            _cards = CreateDeck();
        }

        /// <summary>
        /// Sets the game mode.
        /// </summary>
        /// <param name="gameMode">Game mode.</param>
        public void SetGameMode(Game.GameMode gameMode, 
                                Card.CardColor? assetColor = null)
        {
            if (assetColor == null && gameMode == Game.GameMode.Classic)
            {
                throw new Exceptions.DeckError("GameMode cannot be Classic");
            }
            else if (assetColor != null && gameMode != Game.GameMode.Classic) {
                throw new Exceptions.DeckError("GameMode must be Classic");
            }
            _gameMode = gameMode;
            _assetColor = assetColor;
        }

        /// <summary>
        /// Gets the card value.
        /// </summary>
        /// <returns>The card value.</returns>
        /// <param name="card">Card.</param>
        public int GetCardValue(Card card)
        {
            if (_gameMode == null) {
                throw new Exceptions.DeckError("GameMode must be set");
            }
            if (_gameMode == Game.GameMode.Classic) {
                if (_assetColor == null) {
                    throw new Exceptions.DeckError("Asset color must be set");
                } 
                return Card.GetCardValue(card, (Game.GameMode)_gameMode, (Card.CardColor)_assetColor);
            }
            return Card.GetCardValue(card, (Game.GameMode)_gameMode);
        }

        /// <summary>
        /// Distributes the cards.
        /// </summary>
        /// <param name="players">Players.</param>
        public void DistributeCards(List<Player> players)
        {
            if (_gameMode != null)
            {
                throw new Exceptions.DeckError("GameMode should not be set");
            }
            if (players.Count() != 4)
            {
                throw new ArgumentException("Invalid number of players, must be 4.");
            }
            var shuffledCards = _cards.OrderBy(a => Guid.NewGuid()).ToList();

            // Distribute 3 cards, then 2, then 3.
            foreach (Player player in players)
            {
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

        /// <summary>
        /// Creates a deck.
        /// </summary>
        /// <returns>The deck.</returns>
        static private List<Card> CreateDeck()
        {
            List<Card> cards = new List<Card>();

            foreach (Card.CardColor color in
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                foreach (Card.CardType type in
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    cards.Add(new Card(type, color));
                }
            }
            return cards;
        }
    }
}
