using System;
using System.Linq;
using System.Collections.Generic;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                Assert.AreEqual(0, player.Hand.Count());
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
            var cards = deck.Cards.Take(1);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                Assert.AreEqual(0, player.Hand.Count());
                player.GiveCard(cards.ToArray()[0]);
                Assert.AreEqual(1, player.Hand.Count());
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
            var cards = deck.Cards.Take(8);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                Assert.AreEqual(0, player.Hand.Count());
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                Assert.AreEqual(8, player.Hand.Count());
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
            bool hasThrown = false;
            var player = new Player();
            var deck = new Deck(Game.GameMode.AllAssets);
            var cards = deck.Cards.Take(8);

            try
            {
                foreach (var curCard in cards)
                {
                    player.GiveCard(curCard);
                }
                player.ResetCards();
                Assert.AreEqual(0, player.Hand.Count());
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
    }
}
