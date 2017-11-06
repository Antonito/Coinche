using Coinche.Server.Core;
using NUnit.Framework;

namespace Server
{
    [TestFixture]
    public class TeamsTest
    {
        [Test]
        public void TeamInitialScore()
        {
            var team = new Team(new Player(), new Player());

            Assert.AreEqual(0, team.Score);
        }

        [Test]
        public void TeamAddScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(200);
            Assert.AreEqual(200, team.Score);
        }

        [Test]
        public void TeamHasNotWonNoScore()
        {
            var team = new Team(new Player(), new Player());

            Assert.AreEqual(false, team.HasWon());
        }

        [Test]
        public void TeamHasNotWonSmallScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(200);
            Assert.AreEqual(false, team.HasWon());
        }

        [Test]
        public void TeamHasWonEqualScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(team.MaxScore);
            Assert.AreEqual(true, team.HasWon());
        }

        [Test]
        public void TeamHasWonBigScore()
        {
            var team = new Team(new Player(), new Player());

            team.AddScore(team.MaxScore + 5000);
            Assert.AreEqual(true, team.HasWon());
        }
    }
}
