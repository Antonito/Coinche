using System;
using System.Collections.Generic;
using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    /// <summary>
    /// Contract test.
    /// </summary>
    [TestFixture]
    public class ContractTest
    {
        /// <summary>
        /// Game test.
        /// </summary>
        private class GameTest
        {
            private readonly Game.GameMode gameMode;
            private readonly Card.CardColor? assetColor;
            private readonly Deck deck;
            private readonly List<Player> players;
            public readonly Game game;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Server.ContractTest.GameTest"/> class.
            /// </summary>
            /// <param name="gameMode">Game mode.</param>
            /// <param name="assetColor">Asset color.</param>
            public GameTest(Game.GameMode gameMode, Card.CardColor assetColor)
            {
                this.gameMode = gameMode;
                this.assetColor = assetColor;
                deck = new Deck();
                deck.SetGameMode(gameMode, assetColor);                
                players = new List<Player> {
                new Player(deck), new Player(deck),
                new Player(deck), new Player(deck)
                };
                game = new Game(players, gameMode, assetColor);
            }
        };

        /// <summary>
        /// Test if a test contract is respected.
        /// </summary>
        [Test]
        public void ContractPointRespected()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Points80);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Points80);
                team[0].ScoreCurrent = 90;

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(true, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a test contract is not respected.
        /// </summary>
        [Test]
        public void ContractPointNotRespected()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Points100);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Points100);
                team[0].ScoreCurrent = 90;

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a test contract is invalid.
        /// </summary>
        [Test]
        public void ContractPointInvalid()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Points100);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Points80);
                team[0].ScoreCurrent = 90;

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is respected with one player.
        /// </summary>
        [Test]
        public void ContractFoldCapotRespectedSamePlayer()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);

                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(true, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is respected with both players.
        /// </summary>
        [Test]
        public void ContractFoldCapotRespectedMultiplePlayers()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);

                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[1].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(true, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is respected with both players 
        /// but enemy has a higher score.
        /// </summary>
        [Test]
        public void ContractFoldCapotNotRespectedEnemyHigherScore()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[1].Players[0].WinFold();
                game.Run();
                team[1].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is not respected, because of equality
        /// </summary>
        [Test]
        public void ContractFoldCapotNotRespectedEquality()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[1].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is not respected.
        /// </summary>
        [Test]
        public void ContractFoldCapotNotRespected()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is not invalid #1.
        /// </summary>
        [Test]
        public void ContractFoldCapotInvalid1()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Points80);
                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract Capot is not invalid #2.
        /// </summary>
        [Test]
        public void ContractFoldCapotInvalid2()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                team[0].Players[0].Contract = new Contract(Contract.Promise.Points80);
                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(true, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract General is respected.
        /// </summary>
        [Test]
        public void ContractFoldGeneralRespected()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.General);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(true, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract General is not respected.
        /// </summary>
        [Test]
        public void ContractFoldGeneralNotRespected()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.General);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[1].WinFold();
                game.Run();
                team[0].Players[1].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract General is not respected because of ally.
        /// </summary>
        [Test]
        public void ContractFoldGeneraNotlRespectedAlly()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.General);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[0].Players[1].WinFold();
                game.Run();
                team[0].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }

        /// <summary>
        /// Test if a fold contract General is not respected because of enemy.
        /// </summary>
        [Test]
        public void ContractFoldGeneraNotlRespectedEnemy()
        {
            bool isRespected = false;
            bool hasThrown = false;

            try
            {
                GameTest test = new GameTest(Game.GameMode.Classic, Card.CardColor.Heart);

                var game = test.game;
                var team = test.game.Teams;
                team[0].Players[0].Contract = new Contract(Contract.Promise.General);
                team[0].Players[1].Contract = new Contract(Contract.Promise.Capot);
                game.Run();
                team[0].Players[0].WinFold();
                game.Run();
                team[1].Players[0].WinFold();
                game.Run();
                team[0].Players[0].WinFold();

                isRespected = Contract.IsPromiseRespected(game, team[0], team[1]);
            }
            catch (Exception)
            {
                isRespected = false;
                hasThrown = true;
            }
            Assert.AreEqual(false, isRespected);
            Assert.AreEqual(false, hasThrown);
        }
    }
}
