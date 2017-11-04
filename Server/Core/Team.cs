using System.Collections.Generic;

namespace Coinche.Server.Core
{
    public class Team
    {
        private readonly List<Player> _members;
        private readonly int _score;
        static private readonly int _maxScore = 3000;

        public int MaxScore { get { return _maxScore; } }
        public int Score { get { return _score; } }

        public Team(Player player1, Player player2)
        {
            _score = 0;
            _members = new List<Player> { player1, player2 };
        }
    }
}
