using System;
using System.Linq;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{

    [TestFixture]
    public class UnitTestDeck
    {
        // Check the validity of a deck for a "No Asset" game mode
        [Test]
        public void NoAssetDeck()
        {
            Deck deck = new Deck(Game.GameMode.NoAsset);

            IsValidDeck(deck);
            foreach (Card cur in deck.Cards)
            {
                Assert.AreEqual(false, cur.Asset);
            }

            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(19, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ace).Value);
            Assert.AreEqual(4, Array.Find(cardList,
                                        p => p.Type == Card.CardType.King).Value);
            Assert.AreEqual(3, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Queen).Value);
            Assert.AreEqual(2, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Jack).Value);
            Assert.AreEqual(10, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ten).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Nine).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Eight).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Seven).Value);
        }

        // Check the validity of a deck for a "All Assets" game mode
        [Test]
        public void AllAssetsDeck()
        {
            Deck deck = new Deck(Game.GameMode.AllAssets);

            IsValidDeck(deck);
            foreach (Card cur in deck.Cards)
            {
                Assert.AreEqual(true, cur.Asset);
            }

            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(7, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ace).Value);
            Assert.AreEqual(3, Array.Find(cardList,
                                        p => p.Type == Card.CardType.King).Value);
            Assert.AreEqual(2, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Queen).Value);
            Assert.AreEqual(14, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Jack).Value);
            Assert.AreEqual(5, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Ten).Value);
            Assert.AreEqual(9, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Nine).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Eight).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                                        p => p.Type == Card.CardType.Seven).Value);
        }

        // Check the validity of all possible decks for a "Classic" game mode
        [Test]
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
                    Assert.AreEqual(cur.Color == color, cur.Asset);
                }
            }

            Deck deck = new Deck(Game.GameMode.Classic, Card.CardColor.Clover);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            // Check when the card is Asset
            Assert.AreEqual(11, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(4, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.King &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(3, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(20, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(10, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(14, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color == Card.CardColor.Clover;
                           }).Value);

            // Check when the card is not Asset
            Assert.AreEqual(11, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(4, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.King &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(3, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(2, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(10, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
            Assert.AreEqual(0, Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color != Card.CardColor.Clover;
                           }).Value);
        }

        // Check if a deck contains 32 cards.
        // Then check if the deck contains 8 cards of each color.
        // Then check if that there is only one occurence of each card.
        static private void IsValidDeck(Deck deck)
        {
            Assert.AreEqual(32, deck.Cards.Count());

            foreach (Card.CardColor color in
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                Assert.AreEqual(8, deck.Cards.Count(p => p.Color == color));
                foreach (Card.CardType type in
                         Enum.GetValues(typeof(Card.CardType)))
                {
                    Assert.AreEqual(1, deck.Cards.Count(p =>
                    {
                        return p.Type == type && p.Color == color;
                    }));
                }
            }
        }
    }
}
