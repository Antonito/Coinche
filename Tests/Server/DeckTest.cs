using System;
using System.Linq;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    /// <summary>
    /// Test deck.
    /// </summary>
    [TestFixture]
    public class UnitTestDeck
    {
        /// <summary>
        /// Test to get the value of a card from a desk with no game mode
        /// </summary>
        [Test]
        public void NoGameModeSet()
        {
            Deck deck = new Deck();
            Card[] cardList = deck.Cards.ToArray();
            bool hasThrown = false;

            try 
            {
                deck.GetCardValue(cardList[0]);
            }
            catch (Exception) 
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test to get the value of a card from a desk with a game mode
        /// </summary>
        [Test]
        public void GameModeSet()
        {
            Deck deck = new Deck();
            Card[] cardList = deck.Cards.ToArray();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.NoAsset);
                deck.GetCardValue(cardList[0]);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to set an invalid game mode to a deck #1
        /// </summary>
        [Test]
        public void WrongGameModeSet1()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.NoAsset, Card.CardColor.Clover);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test to set an invalid game mode to a deck #2
        /// </summary>
        [Test]
        public void WrongGameModeSet2()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.AllAssets, Card.CardColor.Clover);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test to set an invalid game mode to a deck #3
        /// </summary>
        [Test]
        public void WrongGameModeSet3()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.Classic);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test to set a valid game mode to a deck #1
        /// </summary>
        [Test]
        public void ValidGameModeSet1()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.NoAsset);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to set a valid game mode to a deck #2
        /// </summary>
        [Test]
        public void ValidGameModeSet2()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.AllAssets);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to set a valid game mode to a deck #3
        /// </summary>
        [Test]
        public void ValidGameModeSet3()
        {
            Deck deck = new Deck();
            bool hasThrown = false;

            try
            {
                deck.SetGameMode(Game.GameMode.Classic, Card.CardColor.Clover);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        // Check the validity of a deck for a "No Asset" game mode
        /// </summary>
        [Test]
        public void NoAssetDeck()
        {
            Deck deck = new Deck();
            deck.SetGameMode(Game.GameMode.NoAsset);

            IsValidDeck(deck);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(19, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == Card.CardType.Ace)));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.King)));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Queen)));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Jack)));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == Card.CardType.Ten)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Nine)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Eight)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Seven)));
        }

        /// <summary>
        /// Check the validity of a deck for a "All Assets" game mode
        /// </summary>
        [Test]
        public void AllAssetsDeck()
        {
            Deck deck = new Deck();
            deck.SetGameMode(Game.GameMode.AllAssets);

            IsValidDeck(deck);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(7, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Ace)));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.King)));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Queen)));
            Assert.AreEqual(14, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == Card.CardType.Jack)));
            Assert.AreEqual(5, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Ten)));
            Assert.AreEqual(9, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Nine)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Eight)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == Card.CardType.Seven)));
        }

        /// <summary>
        /// Check the validity of all possible decks for a "Classic" game mode
        /// </summary>
        [Test]
        public void ClassicAssetsDeck()
        {
            foreach (Card.CardColor color in
                     Enum.GetValues(typeof(Card.CardColor)))
            {
                Deck curDeck = new Deck();
                    curDeck.SetGameMode(Game.GameMode.Classic, color);

                IsValidDeck(curDeck);
            }

            Deck deck = new Deck();
            deck.SetGameMode(Game.GameMode.Classic, Card.CardColor.Clover);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            // Check when the card is Asset
            Assert.AreEqual(11, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.King &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(20, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(14, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color == Card.CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color == Card.CardColor.Clover;
            })));

            // Check when the card is not Asset
            Assert.AreEqual(11, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ace &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.King &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Queen &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Jack &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Ten &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Nine &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Eight &&
                                                   p.Color != Card.CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == Card.CardType.Seven &&
                                                   p.Color != Card.CardColor.Clover;
            })));
        }


        /// <summary>
        /// Check if a deck contains 32 cards.
        /// Then check if the deck contains 8 cards of each color.
        /// Then check if that there is only one occurence of each card.
        /// </summary>
        /// <param name="deck">Deck.</param>
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
