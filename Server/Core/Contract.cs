using System.Collections.Generic;
using System.Linq;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Contract.
    /// </summary>
    public sealed class Contract
    {
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

        /// <summary>
        /// The owner of the contract.
        /// </summary>
        private readonly Player _owner;

        /// <summary>
        /// The target of the contract, if needed.
        /// </summary>
        private readonly Player _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Contract"/> class.
        /// </summary>
        /// <param name="promise">Promise.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="target">Target, if needed (defaults to null).</param>
        public Contract(Promise promise, Player owner, Player target = null)
        {
            if ((promise == Promise.Coinche || 
                 promise == Promise.ReCoinche || 
                 promise == Promise.General) && target == null)
            {
                throw new Exceptions.ContractError("Contract should have a target.");
            }
            _promise = promise;
            _owner = owner;
            _target = target;
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
            // The contract must be different from Promise.Passe
            if (game.Contract._promise == Promise.Passe)
            {
                throw new Exceptions.ContractError("Invalid contract.");
            }

            // Check if the score is greater 
            if (toCheck.ScoreCurrent < enemy.ScoreCurrent || 
                !toCheck.Players.Contains(game.Contract._owner)) 
            {
                return false;
            }

            // Check contract
            if (game.Contract._promise >= Promise.Points80 && 
                game.Contract._promise <= Promise.Points160) 
            {
                return IsScorePromiseRespected(game, toCheck);
            }
            if (game.Contract._promise == Promise.Coinche || game.Contract._promise == Promise.ReCoinche)
            {
                return IsCoinchePromiseRespected(game, toCheck);
            }
            return IsFoldPromiseRespected(game, toCheck);
        }

        /// <summary>
        /// Check if a score promise is respected
        /// </summary>
        /// <returns><c>true</c>, if score promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="player">Player.</param>
        /// <param name="team">Team.</param>
        private static bool IsScorePromiseRespected(Game game, Team team)
        {
            var promise = game.Contract._promise;
            return team.ScoreCurrent >= (int)promise;
        }

        /// <summary>
        /// Check if a coinche promise is respected
        /// </summary>
        /// <returns><c>true</c>, if coinche promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="game">Game.</param>
        /// <param name="team">Team.</param>
        private static bool IsCoinchePromiseRespected(Game game, Team team)
        {
            // TODO: List of previous Contracts ?
            throw new System.NotImplementedException();
            return false;
        }

        /// <summary>
        /// Check if a fold promise is respected
        /// </summary>
        /// <returns><c>true</c>, if fold promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="game">Game.</param>
        /// <param name="team">Team.</param>
        private static bool IsFoldPromiseRespected(Game game, Team team) 
        {
            var contract = game.Contract;
            // Check if promise is Capot
            if (contract._promise == Promise.Capot)
            {
                return team.Players[0].Victories + team.Players[1].Victories == game.NumberOfFolds;
            }

            // Then promise must be General
            if (contract._promise != Promise.General) 
            {
                throw new Exceptions.ContractError("Invalid promise");
            }
            return contract._target.Victories == game.NumberOfFolds;
        }
    }
}
