using System.Collections.Generic;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Team.
    /// </summary>
    public sealed class Team
    {
        /// <summary>
        /// The members of the team.
        /// </summary>
        private readonly List<Player> _members;

        /// <summary>
        /// The current score.
        /// </summary>
        private int _score;

        /// <summary>
        /// The maximum score possible.
        /// </summary>
        static private readonly int _maxScore = 3000;

        /// <summary>
        /// Gets the maximum score.
        /// </summary>
        /// <value>The maximum score possible.</value>
        static public int MaxScore { get { return _maxScore; } }

        /// <summary>
        /// Gets the current score.
        /// </summary>
        /// <value>The current score.</value>
        public int Score { get { return _score; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Team"/> class.
        /// </summary>
        /// <param name="player1">Player1.</param>
        /// <param name="player2">Player2.</param>
        public Team(Player player1, Player player2)
        {
            _score = 0;
            _members = new List<Player> { player1, player2 };
        }

        /// <summary>
        /// Adds points the score.
        /// </summary>
        /// <param name="scoreToAdd">Score to add.</param>
        public void AddScore(int scoreToAdd)
        {
            _score += scoreToAdd;
        }

        /// <summary>
        /// Check if the team has won
        /// </summary>
        /// <returns><c>true</c>, if then team has won, <c>false</c> otherwise.</returns>
        public bool HasWon()
        {
            return _score >= _maxScore;
        }
    }
}
