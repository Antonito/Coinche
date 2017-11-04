using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Card
    {
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

        public enum CardColor
        {
            Clover,
            Tile,
            Heart,
            Pike
        };

        // The values of the cards when playing in standard mode 
        // and that the card is an asset
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

        // The values of the cards when playing in standard mode 
        // and that the card is not an asset
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

        // The values of the card when playing in "All assets" mode
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

        // The values of the card when playing in "No asset" mode
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

        public int Value { get { return _value; } }
        public CardType Type { get { return _type; } }
        public CardColor Color { get { return _color; } }
        public bool Asset { get { return _isAsset; } }

        // Initialize a card
        public Card(CardType type, CardColor color, bool isAsset,
                    Game.GameMode gameMode)
        {
            _type = type;
            _color = color;
            _isAsset = isAsset;
            _gameMode = gameMode;
            _value = SetCardValue(type);
        }

        // Get the correct card value for the current game mode and card
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
