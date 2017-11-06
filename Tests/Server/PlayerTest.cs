using System;
using System.Linq;
using System.Collections.Generic;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    [TestFixture]
    public class PlayerTest
    {
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
