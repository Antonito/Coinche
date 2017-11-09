using System;
using System.Linq;
using System.Collections.Generic;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    using GameMode = Coinche.Common.Core.Game.GameMode;

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
    }
}
