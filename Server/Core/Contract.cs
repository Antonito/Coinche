using System.Collections.Generic;
using System.Linq;

namespace Coinche.Server.Core
{
    using Promise = Common.Core.Contract.Promise;

    /// <summary>
    /// Contract.
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// Contract Data.
        /// </summary>
        private struct Data
        {
            /// <summary>
            /// Gets or sets the promise.
            /// </summary>
            /// <value>The promise.</value>
            public Promise Promise { get; set; }

            /// <summary>
            /// Gets or sets the owner.
            /// </summary>
            /// <value>The owner.</value>
            public Player Owner { get; set; }

            /// <summary>
            /// Gets or sets the target.
            /// </summary>
            /// <value>The target.</value>
            public Player Target { get; set; }
        };

        /// <summary>
        /// The history of Datas.
        /// </summary>
        private readonly Stack<Data> _history;

        /// <summary>
        /// Gets the promise.
        /// </summary>
        /// <value>The promise.</value>
        public Promise Promise { get { return _history.Peek().Promise; } }

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
            _history = new Stack<Data>();
            ChangeContract(promise, owner, target);
         }

        /// <summary>
        /// Changes the contract.
        /// </summary>
        /// <param name="promise">Promise.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="target">Target, if needed (defaults to null).</param>
        public void ChangeContract(Promise promise, Player owner, 
                                   Player target = null)
        {
            _history.Push(new Data
            {
                Promise = promise,
                Owner = owner,
                Target = target
            });
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
            var promiseData = game.Contract._history.Peek();
            if (promiseData.Promise == Promise.Passe)
            {
                throw new Exceptions.ContractError("Invalid contract.");
            }

            // Check contract
            if (promiseData.Promise >= Promise.Points80 && 
                promiseData.Promise <= Promise.Points160) 
            {
                return IsScorePromiseRespected(game, toCheck, enemy);
            }
            if (promiseData.Promise == Promise.Coinche || 
                promiseData.Promise == Promise.ReCoinche)
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
        /// <param name="team">Enemy.</param>
        private static bool IsScorePromiseRespected(Game game, Team team, Team enemy)
        {
            var promiseData = game.Contract._history.Peek();
            var promise = promiseData.Promise;


            // Check if the score is greater 
            if (team.ScoreCurrent < enemy.ScoreCurrent ||
                !team.Players.Contains(promiseData.Owner))
            {
                return false;
            }
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
            var promiseData = game.Contract._history.Peek();
            var otherTeam = (game.Teams[0] == team) ? game.Teams[1] : game.Teams[0];

            if (promiseData.Promise == Promise.Coinche)
            {
                if (game.Contract._history.Count < 2)
                {
                    throw new Exceptions.ContractError("Invalid promise.");
                }
                // We remove the contract, so we can check the previous contract
                game.Contract._history.Pop();
                return !IsPromiseRespected(game, otherTeam, team);
            }

            if (promiseData.Promise != Promise.ReCoinche ||
                game.Contract._history.Count < 3)
            {
                throw new Exceptions.ContractError("Invalid promise.");
            }
            game.Contract._history.Pop();

            return !IsPromiseRespected(game, otherTeam, team);
        }

        /// <summary>
        /// Check if a fold promise is respected
        /// </summary>
        /// <returns><c>true</c>, if fold promise was respected, <c>false</c> otherwise.</returns>
        /// <param name="game">Game.</param>
        /// <param name="team">Team.</param>
        private static bool IsFoldPromiseRespected(Game game, Team team) 
        {
            var promiseData = game.Contract._history.Peek();
            var contract = game.Contract;

            // Check if promise is Capot
            if (promiseData.Promise == Promise.Capot)
            {
                return team.Players[0].Victories + team.Players[1].Victories == game.NumberOfFolds;
            }

            // Then promise must be General
            if (promiseData.Promise != Promise.General) 
            {
                throw new Exceptions.ContractError("Invalid promise");
            }
            return promiseData.Target.Victories == game.NumberOfFolds;
        }
    }
}
