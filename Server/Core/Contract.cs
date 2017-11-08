using System.Collections.Generic;
using System.Linq;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Contract.
    /// </summary>
    public sealed class Contract
    {
        // TODO: English name ?
        /// <summary>
        /// Promise.
        /// </summary>
        public enum Promise
        {
            Passe = 0,
            Coinche = 1,
            ReCoinche = 2,
            Points80 = 80,
            Points90 = 90,
            Points100 = 100,
            Points110 = 110,
            Points120 = 120,
            Points130 = 130,
            Points140 = 140,
            Points150 = 150,
            Points160 = 160,
            Capot = 250,
            General = 500
        };

        /// <summary>
        /// The promise.
        /// </summary>
        private readonly Promise _promise;

        public Contract(Promise promise)
        {
            _promise = promise;
        }

        /// <summary>
        /// Check if the promise is respected
        /// </summary>
        /// <returns><c>true</c>, if promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="game">Game.</param>
        /// <param name="toCheck">To check.</param>
        /// <param name="enemy">Enemy.</param>
        public static bool IsPromiseRespected(Game game, Team toCheck, Team enemy)
        {
            // Check if a contract is present
            if (toCheck.Players[0].Contract == null && 
                toCheck.Players[1].Contract == null)
            {
                return false;
            }
            // Check if the score is greater 
            if (toCheck.ScoreCurrent < enemy.ScoreCurrent) 
            {
                return false;
            }
            // Check contract
            if (toCheck.Players[0].Contract._promise >= Promise.Points80 && 
                toCheck.Players[0].Contract._promise <= Promise.Points160) 
            {
                return IsScorePromiseRespected(toCheck);
            }
            return IsFoldPromiseRespected(game, toCheck);
        }

        /// <summary>
        /// Check if a score promise is respected
        /// </summary>
        /// <returns><c>true</c>, if score promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="player">Player.</param>
        private static bool IsScorePromiseRespected(Team team)
        {
            if (team.Players[0].Contract._promise != team.Players[1].Contract._promise) 
            {
                throw new Exceptions.ContractError("Promises should be the same");   
            }
            var promise = team.Players[0].Contract._promise;
            return team.ScoreCurrent >= (int)promise;
        }

        /// <summary>
        /// Check if a fold promise is respected
        /// </summary>
        /// <returns><c>true</c>, if fold promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="game">Game.</param>
        /// <param name="team">Team.</param>
        private static bool IsFoldPromiseRespected(Game game, Team team) 
        {
            // Check if promise is Capot
            if ((team.Players[0].Contract._promise == Promise.Capot && 
                 team.Players[1].Contract._promise != Promise.General) ||
                (team.Players[1].Contract._promise == Promise.Capot && 
                 team.Players[0].Contract._promise != Promise.General))
            {
                if (team.Players[0].Contract._promise != team.Players[1].Contract._promise)
                {
                    throw new Exceptions.ContractError("Promises should be the same");
                }
                return team.Players[0].Victories + team.Players[1].Victories == game.NumberOfFolds;
            }

            // Then promise must be General
            var player = (team.Players[0].Contract._promise == Promise.General) ? team.Players[0] : team.Players[1];

            if (player.Contract._promise != Promise.General) 
            {
                throw new Exceptions.ContractError("Invalid promise");
            }
            return player.Victories == game.NumberOfFolds;
        }
    }
}
