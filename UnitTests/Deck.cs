using System;
using System.Linq;
using Xunit;
using TrainingProject.Core;

namespace UnitTests
{
    public class UnitTestDeck
    {
        // Check the validity of a deck for a "No Asset" game mode
        [Fact]
        public void NoAssetDeck()
        {
            Deck deck = new Deck(Game.GameMode.NoAsset);

            IsValidDeck(deck);
            foreach (Card cur in deck.Cards) {
                Assert.Equal(false, cur.Asset);
            }

            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.Equal(19, Array.Find(cardList, 
                                        p => p.Type == Card.CardType.Ace).Value);
            Assert.Equal(4, Array.Find(cardList,
                                        p => p.Type == Card.CardType.King).Value);
            Assert.Equal(3, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Queen).Value);
            Assert.Equal(2, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Jack).Value);
            Assert.Equal(10, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ten).Value);
            Assert.Equal(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Nine).Value);
            Assert.Equal(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Eight).Value);
            Assert.Equal(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Seven).Value);
        }

        // Check the validity of a deck for a "All Assets" game mode
        [Fact]
        public void AllAssetsDeck()
        {
            Deck deck = new Deck(Game.GameMode.AllAssets);

            IsValidDeck(deck);
            foreach (Card cur in deck.Cards)
            {
                Assert.Equal(true, cur.Asset);
            }

            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.Equal(7, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ace).Value);
            Assert.Equal(3, Array.Find(cardList,
                                        p => p.Type == Card.CardType.King).Value);
            Assert.Equal(2, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Queen).Value);
            Assert.Equal(14, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Jack).Value);
            Assert.Equal(5, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ten).Value);
            Assert.Equal(9, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Nine).Value);
            Assert.Equal(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Eight).Value);
            Assert.Equal(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Seven).Value);
        }

        // Check the validity of all possible decks for a "Classic" game mode
        [Fact]
        public void ClassicAssetsDeck() 
        {
            foreach (Card.CardColor color in 
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                Deck curDeck = new Deck(Game.GameMode.Classic, color);

                IsValidDeck(curDeck);

                // If the card of the color is the current color,
                // the card must be an asset.
                foreach (Card cur in curDeck.Cards)
                {
                    Assert.Equal(cur.Color == color, cur.Asset);
                }
            }

            Deck deck = new Deck(Game.GameMode.Classic, Card.CardColor.Clover);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            // Check when the card is Asset
            Assert.Equal(11, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(4, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.King &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(3, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(20, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(10, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(14, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(0, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(0, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);

            // Check when the card is not Asset
            Assert.Equal(11, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(4, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.King &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(3, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(2, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(10, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(0, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(0, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.Equal(0, Array.Find(cardList,
                           p => {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
        }

        // Check if a deck contains 32 cards.
        // Then check if the deck contains 8 cards of each color.
        // Then check if that there is only one occurence of each card.
        static private void IsValidDeck(Deck deck) {
            Assert.Equal(32, deck.Cards.Count());

            foreach (Card.CardColor color in
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                Assert.Equal(8, deck.Cards.Count(p => p.Color == color));
                foreach (Card.CardType type in
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    Assert.Equal(1, deck.Cards.Count(p =>
                    {
                      return p.Type == type && p.Color == color;
                    }));
                }
            }
        }
    }
}
