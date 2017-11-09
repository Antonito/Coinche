using System;
using System.Linq;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    using CardColor = Coinche.Common.Core.Cards.CardColor;
    using CardType = Coinche.Common.Core.Cards.CardType;
    using GameMode = Coinche.Common.Core.Game.GameMode;
    
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
                deck.SetGameMode(GameMode.NoAsset);
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
                deck.SetGameMode(GameMode.NoAsset, CardColor.Clover);
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
                deck.SetGameMode(GameMode.AllAssets, CardColor.Clover);
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
                deck.SetGameMode(GameMode.Classic);
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
                deck.SetGameMode(GameMode.NoAsset);
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
                deck.SetGameMode(GameMode.AllAssets);
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
                deck.SetGameMode(GameMode.Classic, CardColor.Clover);
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
            deck.SetGameMode(GameMode.NoAsset);

            IsValidDeck(deck);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(19, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == CardType.Ace)));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.King)));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Queen)));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Jack)));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == CardType.Ten)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Nine)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Eight)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Seven)));
        }

        /// <summary>
        /// Check the validity of a deck for a "All Assets" game mode
        /// </summary>
        [Test]
        public void AllAssetsDeck()
        {
            Deck deck = new Deck();
            deck.SetGameMode(GameMode.AllAssets);

            IsValidDeck(deck);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            Assert.AreEqual(7, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Ace)));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.King)));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Queen)));
            Assert.AreEqual(14, deck.GetCardValue(Array.Find(cardList,
                                                             p => p.Type == CardType.Jack)));
            Assert.AreEqual(5, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Ten)));
            Assert.AreEqual(9, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Nine)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Eight)));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                                                            p => p.Type == CardType.Seven)));
        }

        /// <summary>
        /// Check the validity of all possible decks for a "Classic" game mode
        /// </summary>
        [Test]
        public void ClassicAssetsDeck()
        {
            foreach (CardColor color in
                     Enum.GetValues(typeof(CardColor)))
            {
                Deck curDeck = new Deck();
                    curDeck.SetGameMode(GameMode.Classic, color);

                IsValidDeck(curDeck);
            }

            Deck deck = new Deck();
            deck.SetGameMode(GameMode.Classic, CardColor.Clover);
            // Check the values of the cards
            Card[] cardList = deck.Cards.ToArray();

            // Check when the card is Asset
            Assert.AreEqual(11, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Ace &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.King &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Queen &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(20, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Jack &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Ten &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(14, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Nine &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Eight &&
                                                   p.Color == CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Seven &&
                                                   p.Color == CardColor.Clover;
            })));

            // Check when the card is not Asset
            Assert.AreEqual(11, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Ace &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(4, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.King &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(3, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Queen &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(2, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Jack &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(10, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Ten &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Nine &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Eight &&
                                                   p.Color != CardColor.Clover;
            })));
            Assert.AreEqual(0, deck.GetCardValue(Array.Find(cardList,
                           p =>
                           {
                               return p.Type == CardType.Seven &&
                                                   p.Color != CardColor.Clover;
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
            Assert.AreEqual(32, deck.Cards.Count);

            foreach (CardColor color in
                     Enum.GetValues(typeof(CardColor)))
            {
                Assert.AreEqual(8, deck.Cards.Count(p => p.Color == color));
                foreach (CardType type in
                         Enum.GetValues(typeof(CardType)))
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
