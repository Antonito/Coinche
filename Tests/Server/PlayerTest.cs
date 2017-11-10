using System;
using System.Linq;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    using GameMode = Coinche.Common.Core.Game.GameMode;
    using CardColor = Coinche.Common.Core.Cards.CardColor;
    using CardType = Coinche.Common.Core.Cards.CardType;

    /// <summary>
    /// Player test.
    /// </summary>
    [TestFixture]
    public class PlayerTest
    {
        /// <summary>
        /// Test to give one card to a player
        /// </summary>
        [Test]
        public void PlayerAddOneCard()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards) {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to give a full hand to a player
        /// </summary>
        [Test]
        public void PlayerAddMaxCard()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(8);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to give too many cards to a player
        /// </summary>
        [Test]
        public void PlayerAddTooManyCard()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(10);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test to give a full hand to a player and then to reset it
        /// </summary>
        [Test]
        public void PlayerAddMaxCardThenReset()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                Assert.AreEqual(0, player.Hand.Count);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to give a full hand to a player, then reset it, 
        /// and give one card to the player
        /// </summary>
        [Test]
        public void PlayerAddMaxCardThenResetThenAddOne()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                player.GiveDeck(deck);
                Assert.AreEqual(0, player.Hand.Count);
                player.GiveCard(cards.ToArray()[0]);
                Assert.AreEqual(1, player.Hand.Count);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to give a full hand to a player, then reset it, 
        /// and give another full hand to the player
        /// </summary>
        [Test]
        public void PlayerAddMaxCardThenResetThenAddMax()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(8);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                player.GiveDeck(deck);
                Assert.AreEqual(0, player.Hand.Count);
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                Assert.AreEqual(8, player.Hand.Count);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test to give a full hand to a player, then reset it, 
        /// and then give too many cards to the player
        /// </summary>
        [Test]
        public void PlayerAddMaxCardThenResetThenAddTooMany()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(8);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                player.GiveDeck(deck);
                Assert.AreEqual(0, player.Hand.Count);
                cards = deck.Cards.Take(10);
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Check if a player have an asset in all asset mode
        /// </summary>
        [Test]
        public void PlayerHaveAssetAllAssets()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, player.HaveAsset());
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Check if a player have an asset in no asset mode
        /// </summary>
        [Test]
        public void PlayerHaveAssetNoAssets()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.NoAsset);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, player.HaveAsset());
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Check if a player have an asset in classic Pike mode (OK)
        /// </summary>
        [Test]
        public void PlayerHaveAssetClassicPike()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.ClassicPike);
            var cards = deck.Cards.Take(1);

            try
            {
                player.GiveCard(new Card(CardType.Eight, CardColor.Pike));
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, player.HaveAsset());
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Check if a player have an asset in classic Pike mode (KO)
        /// </summary>
        [Test]
        public void PlayerHaveAssetClassicNoPike()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.ClassicPike);
            var cards = deck.Cards.Take(1);

            try
            {
                player.GiveCard(new Card(CardType.Eight, CardColor.Clover));
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(false, player.HaveAsset());
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Check if a player has a card of color
        /// </summary>
        [Test]
        public void PlayerHaveColor()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.ClassicPike);
            var cards = deck.Cards.Take(1);
            var card1 = new Card(CardType.Eight, CardColor.Clover);
            var card2 = new Card(CardType.Eight, CardColor.Pike);

            try
            {
                player.GiveCard(card1);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, player.HaveColor(CardColor.Clover));
            Assert.AreEqual(false, player.HaveColor(CardColor.Pike));
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Check if a player has a card
        /// </summary>
        [Test]
        public void PlayerHaveCard()
        {
            Deck deck = new Deck();
            bool hasThrown = false;
            var player = new Player(true);
            player.GiveDeck(deck);
            deck.SetGameMode(GameMode.ClassicPike);
            var cards = deck.Cards.Take(1);
            var card1 = new Card(CardType.Eight, CardColor.Clover);
            var card2 = new Card(CardType.Eight, CardColor.Pike);

            try
            {
                player.GiveCard(card1);
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.AreEqual(true, player.HaveCard(card1));
            Assert.AreEqual(false, player.HaveCard(card2));
            Assert.AreEqual(false, hasThrown);
        }
    }
}
