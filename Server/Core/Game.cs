using System;
using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server.Core
{
    /// <summary>
    /// Game.
    /// </summary>
    public sealed class Game
    {
        /// <summary>
        /// Game mode.
        /// </summary>
        public enum GameMode
        {
            Classic,
            AllAssets,
            NoAsset
        }

        /// <summary>
        /// The actual deck for the current Game.
        /// </summary>
        private Deck _deck;

        /// <summary>
        /// The players.
        /// Even if we have players in teams
        /// Fold does not have to know about team
        /// so we only send players without team's notion
        /// </summary>
        private readonly List<Player> _players;

        /// <summary>
        /// The teams.
        /// </summary>
        private readonly List<Team> _teams;

        /// <summary>
        /// History of folds.
        /// </summary>
        private readonly List<Fold> _folds;

        /// <summary>
        /// Gets the number of folds.
        /// </summary>
        /// <value>The number of folds.</value>
        public int NumberOfFolds { get { return _folds.Count(); } }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <value>The teams.</value>
        public Team[] Teams { get { return _teams.ToArray(); } }

        // IMPORTANT MUST READ
        // TODO: Re-organize this.
        // This class should contain only informations about 1 game,
        // not looping until a team wins. A team should only be capable of
        // winning the match after they played several games.
        //
        // -> A Game is a few folds.
        // -> A Match is a few games.
        // -> The team with >= Team.MaxPScore wins a Match.

        public Game(List<Team> teams)
        {
            if (teams.Count != 2)
            {
                throw new ArgumentException("Invalid number of teams (must be 2)");
            }
            _teams = teams;
            _players = new List<Player>();
            _players.AddRange(_teams[0].Players);
            _players.AddRange(_teams[1].Players);
            _folds = new List<Fold>();
        }

        public void Run()
        {
            DistributeCards();

            //TODO: ask client for Contract's promise
            // and then get GameMode
            Contract contract = new Contract(Contract.Promise.Passe);

            // Set the GameMode we get via contract
            SetGameMode();

            // All player have the same amount of card that's why
            // we can loop like this.
            //TODO: or maybe implement IsHandEmpty
            // but it's logic that the current game is aware of
            // the number of card in the player hand

            while (_teams[0].Players[0].Hand.Count >= 1)
            {
                Fold fold = new Fold(_players);
                fold.Run();

                //TODO: check if it is necessary
                // Store the fold history for futur usage
                _folds.Add(fold);
            }
        }

        private void DistributeCards()
        {
            _deck = new Deck();
            _deck.DistributeCards(_players);
        }

        private void SetGameMode()
        {
            //TODO: set by the contract
            _deck.SetGameMode(GameMode.Classic);
        }

    }
}
