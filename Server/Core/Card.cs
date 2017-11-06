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

        private readonly int _value;
        private readonly CardType _type;
        private readonly bool _isAsset;
        private readonly Game.GameMode _gameMode;
        private readonly CardColor _color;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get { return _value; } }

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
        /// Gets a value indicating whether this <see cref="T:Coinche.Server.Core.Card"/> is asset.
        /// </summary>
        /// <value><c>true</c> if asset; otherwise, <c>false</c>.</value>
        public bool Asset { get { return _isAsset; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Card"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="color">Color.</param>
        /// <param name="isAsset">If set to <c>true</c> is asset.</param>
        /// <param name="gameMode">Game mode.</param>
        public Card(CardType type, CardColor color, bool isAsset,
                    Game.GameMode gameMode)
        {
            _type = type;
            _color = color;
            _isAsset = isAsset;
            _gameMode = gameMode;
            _value = SetCardValue(type);
        }

        /// <summary>
        /// Get and set the correct card value for the current game mode and card
        /// </summary>
        /// <returns>The card value.</returns>
        /// <param name="type">Type.</param>
        private int SetCardValue(CardType type)
        {
            Dictionary<CardType, int> valueSet;

            if (_gameMode == Game.GameMode.Classic)
            {
                if (_isAsset)
                {
                    valueSet = cardValueIsAsset;
                }
                else
                {
                    valueSet = cardValueIsNotAsset;
                }
            }
            else if (_gameMode == Game.GameMode.AllAssets)
            {
                valueSet = cardValueAllAssets;
            }
            else
            {
                valueSet = cardValueNoAsset;
            }
            return valueSet[type];
        }
    }
}
