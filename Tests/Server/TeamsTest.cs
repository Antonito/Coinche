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
            var team = new Team(new Player(), new Player());

            Assert.AreEqual(0, team.Score);
        }

        /// <summary>
        /// Test to add score to a team.
        /// </summary>
        [Test]
        public void TeamAddScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(200);
            Assert.AreEqual(200, team.Score);
        }

        /// <summary>
        /// Test if a team with no score has won
        /// </summary>
        [Test]
        public void TeamHasNotWonNoScore()
        {
            var team = new Team(new Player(), new Player());

            Assert.AreEqual(false, team.HasWon());
        }

        /// <summary>
        /// Test if a team with a small score has won
        /// </summary>
        [Test]
        public void TeamHasNotWonSmallScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(200);
            Assert.AreEqual(false, team.HasWon());
        }

        /// <summary>
        /// Test if a team with the maximum score has won
        /// </summary>
        [Test]
        public void TeamHasWonEqualScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(team.MaxScore);
            Assert.AreEqual(true, team.HasWon());
        }

        /// <summary>
        /// Test if a team with a big score has won
        /// </summary>
        [Test]
        public void TeamHasWonBigScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(team.MaxScore + 5000);
            Assert.AreEqual(true, team.HasWon());
        }
    }
}
