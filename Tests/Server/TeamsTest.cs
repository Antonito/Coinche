using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    /// <summary>
    /// Teams test.
    /// </summary>
    [TestFixture]
    public class TeamsTest
    {
        /// <summary>
        /// Test a team's the initial score.
        /// </summary>
        [Test]
        public void TeamInitialScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            Assert.AreEqual(0, team.Score);
        }

        /// <summary>
        /// Test to add score to a team.
        /// </summary>
        [Test]
        public void TeamAddScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            team.AddScore(200);
            Assert.AreEqual(200, team.Score);
        }

        /// <summary>
        /// Test if a team with no score has won
        /// </summary>
        [Test]
        public void TeamHasNotWonNoScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            Assert.AreEqual(false, team.HasWon());
        }

        /// <summary>
        /// Test if a team with a small score has won
        /// </summary>
        [Test]
        public void TeamHasNotWonSmallScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            team.AddScore(200);
            Assert.AreEqual(false, team.HasWon());
        }

        /// <summary>
        /// Test if a team with the maximum score has won
        /// </summary>
        [Test]
        public void TeamHasWonEqualScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            team.AddScore(Team.MaxScore);
            Assert.AreEqual(true, team.HasWon());
        }

        /// <summary>
        /// Test if a team with a big score has won
        /// </summary>
        [Test]
        public void TeamHasWonBigScore()
        {
            Deck deck = new Deck();
            var team = new Team(new Player(true), new Player(true));

            team.AddScore(Team.MaxScore + 5000);
            Assert.AreEqual(true, team.HasWon());
        }
    }
}
