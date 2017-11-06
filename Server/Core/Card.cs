using System.Collections.Generic;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Card.
    /// </summary>
    public sealed class Card
    {
        /// <summary>
        /// Card type.
        /// </summary>
        public enum CardType
        {
            Ace,
            King,
            Queen,
            Jack,
            Ten,
            Nine,
            Eight,
            Seven
        };

        /// <summary>
        /// Card color.
        /// </summary>
        public enum CardColor
        {
            Clover,
            Tile,
            Heart,
            Pike
        };

        /// <summary>
        /// The values of the cards when playing in standard mode 
        /// and that the card is an asset
        /// </summary>
        private static readonly Dictionary<CardType, int> cardValueIsAsset =
            new Dictionary<CardType, int> {
            { CardType.Ace, 11 },
            { CardType.King, 4 },
            { CardType.Queen, 3 },
            { CardType.Jack, 20 },
            { CardType.Ten, 10 },
            { CardType.Nine, 14 },
            { CardType.Eight, 0 },
            { CardType.Seven, 0 }
        };

        /// <summary>
        /// The values of the cards when playing in standard mode 
        /// and that the card is not an asset
        /// </summary>
        private static readonly Dictionary<CardType, int> cardValueIsNotAsset =
            new Dictionary<CardType, int> {
            { CardType.Ace, 11 },
            { CardType.King, 4 },
            { CardType.Queen, 3 },
            { CardType.Jack, 2 },
            { CardType.Ten, 10 },
            { CardType.Nine, 0 },
            { CardType.Eight, 0 },
            { CardType.Seven, 0 }
        };

        /// <summary>
        /// The values of the card when playing in "All assets" mode
        /// </summary>
        private static readonly Dictionary<CardType, int> cardValueAllAssets =
            new Dictionary<CardType, int> {
            { CardType.Ace, 7 },
            { CardType.King, 3 },
            { CardType.Queen, 2 },
            { CardType.Jack, 14 },
            { CardType.Ten, 5 },
            { CardType.Nine, 9 },
            { CardType.Eight, 0 },
            { CardType.Seven, 0 }
        };

        /// <summary>
        /// The values of the card when playing in "No asset" mode
        /// </summary>
        private static readonly Dictionary<CardType, int> cardValueNoAsset =
            new Dictionary<CardType, int> {
            { CardType.Ace, 19 },
            { CardType.King, 4 },
            { CardType.Queen, 3 },
            { CardType.Jack, 2 },
            { CardType.Ten, 10 },
            { CardType.Nine, 0 },
            { CardType.Eight, 0 },
            { CardType.Seven, 0 }
        };

        /// <summary>
        /// The type.
        /// </summary>
        private readonly CardType _type;

        /// <summary>
        /// The color.
        /// </summary>
        private readonly CardColor _color;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CardType Type { get { return _type; } }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public CardColor Color { get { return _color; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Card"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="color">Color.</param>
        public Card(CardType type, CardColor color)
        {
            _type = type;
            _color = color;
        }

        /// <summary>
        /// Gets the card value.
        /// </summary>
        /// <returns>The card value.</returns>
        /// <param name="card">Card.</param>
        /// <param name="gameMode">Game mode.</param>
        public static int GetCardValue(Card card, Game.GameMode gameMode)
        {
            Dictionary<CardType, int> valueSet;

            if (gameMode == Game.GameMode.Classic) {
                throw new Exceptions.CardError("GameMode cannot be Classic");
            }
            if (gameMode == Game.GameMode.AllAssets) {
                valueSet = cardValueAllAssets;
            }
            else {
                valueSet = cardValueNoAsset;
            }
            return valueSet[card.Type];
        }

        /// <summary>
        /// Gets the card value.
        /// </summary>
        /// <returns>The card value.</returns>
        /// <param name="card">Card.</param>
        /// <param name="gameMode">Game mode.</param>
        /// <param name="asset">Asset.</param>
        public static int GetCardValue(Card card, Game.GameMode gameMode, CardColor asset)
        {
            Dictionary<CardType, int> valueSet;

            if (gameMode != Game.GameMode.Classic)
            {
                throw new Exceptions.CardError("GameMode must be Classic");
            }
            if (asset == card.Color)
            {
                valueSet = cardValueIsAsset;
            }
            else
            {
                valueSet = cardValueIsNotAsset;
            }
            return valueSet[card.Type];
        }
    }
}
