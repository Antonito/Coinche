using System;
using System.Collections.Generic;

namespace TrainingProject.Core
{
    public class Deck
    {
        private readonly Queue<Card> _cards;

        public Queue<Card> Cards { get { return _cards; } }

        public Deck(Game.GameMode gameMode, Card.CardColor assetColor)
        {
            if (gameMode != Game.GameMode.Classic)
            {
                throw new ArgumentException("GameMode must be Classic.");
            }
            _cards = CreateClassicDeck(assetColor);
        }

        public Deck(Game.GameMode gameMode)
        {
            if (gameMode == Game.GameMode.Classic)
            {
                throw new ArgumentException("GameMode must not be Classic.");
            }
            _cards = CreateAssetsDeck(gameMode);
        }

        static private Queue<Card> CreateClassicDeck(Card.CardColor assetColor) {
            Queue<Card> cards = new Queue<Card>();

            foreach (Card.CardColor color in 
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                foreach (Card.CardType type in 
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    bool isAsset = (color == assetColor) ? true : false;

                    cards.Enqueue(new Card(type, color, isAsset, 
                                           Game.GameMode.Classic));
                }
            }
            return cards;
        }

        static private Queue<Card> CreateAssetsDeck(Game.GameMode gameMode)
        {
            Queue<Card> cards = new Queue<Card>();
            bool isAsset = (gameMode == Game.GameMode.AllAssets) ? true : false;

            foreach (Card.CardColor color in 
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                foreach (Card.CardType type in 
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    cards.Enqueue(new Card(type, color, isAsset, gameMode));
                }
            }
            return cards;
        }
    }
}
