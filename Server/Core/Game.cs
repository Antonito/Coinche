﻿using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using ProtoBuf;
using NetworkCommsDotNet.Connections;

//TODO: Memory stream using, change, unique

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
        /// Is game prepared ?
        /// </summary>
        private bool _prepared;

        /// <summary>
        /// The game mode.
        /// </summary>
        private GameMode _gameMode;

        /// <summary>
        /// The actual deck for the current Game.
        /// </summary>
        private readonly Deck _deck;

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
        /// The contract.
        /// </summary>
        private Contract _contract;

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <value>The contract.</value>
        public Contract Contract { get { return _contract; } }

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
            _prepared = false;
            _teams = teams;
            _players = new List<Player>();
            _players.AddRange(_teams[0].Players);
            _players.AddRange(_teams[1].Players);
            _folds = new List<Fold>();
            _deck = new Deck();
            foreach (var team in teams) {
                foreach (var player in team.Players) {
                    player.GiveDeck(_deck);
                }
            }
        }

        ~Game()
        {
            // Prepare players for another game
            foreach (var team in _teams) 
            {
                foreach (var player in team.Players)
                {
                    player.ResetCards();
                }
            }    
        }

        /// <summary>
        /// Prepares the game.
        /// </summary>
        /// <param name="gameMode">Game mode.</param>
        /// <param name="contract">Contract.</param>
        public void PrepareGame(GameMode gameMode, Contract contract)
        {
            DistributeCards();
            _contract = contract;
            _gameMode = gameMode;
            _prepared = true;
        }

        /// <summary>
        /// Run the game.
        /// </summary>
        /// <returns>The run.</returns>
        /// <param name="play">Useful for unit tests only</param>
        public void Run(bool play = true)
        {
            if (!_prepared)
            {
                SelectContract();
            }

            // TODO: ?
#if false
            // Set the GameMode we get via contract
            SetGameMode();
#endif

            // All player have the same amount of card that's why
            // we can loop like this.
            //TODO: or maybe implement IsHandEmpty
            // but it's logic that the current game is aware of
            // the number of card in the player hand

            if (play)
            {
                // This should not be executed during unit tests
                while (_teams[0].Players[0].Hand.Count >= 1)
                {
                    Fold fold = new Fold(_players, _gameMode);
                    fold.Run();

                    //TODO: check if it is necessary
                    // Store the fold history for futur usage
                    _folds.Add(fold);
                }
            }
            else 
            {
                Fold fold = new Fold(_players, _gameMode);
                fold.Run();
                _folds.Add(fold);
            }

            // Set the Game result
            SetResult();

            //TODO: reset
            // Reset Player's cards (hand and fold)
            //player.ResetCards();
        }

        private void SelectContract()
        {
            foreach (var player in _players)
            {
                Packet.NetworkContract.Register(player.Connection);
            }
            DistributeCards();

            // The contract need to be establised here
            _prepared = false;
            Contract contract = null;
            var minimumContractValue = Core.Contract.Promise.Passe;
            while (!_prepared)
            {
                foreach (var player in _players)
                {
                    // Ask player to choose something
                    Task.Run(() => ContractTask(player, (int)minimumContractValue, _players)).Wait();
                }

                // Check if choosen contrat is valid and final
                // TODO: rm
                _prepared = true;
            }

            //TODO: ask client for Contract's promise
            // and then get GameMode   
            _contract = new Contract(Contract.Promise.Passe, _teams[0].Players[0]);
            _gameMode = GameMode.Classic;
            _prepared = true;

            // Unregister Contract's
            foreach (var player in _players)
            {
                Packet.NetworkContract.Unregister(player.Connection);
            }
        }

        private void DistributeCards()
        {
            _deck.DistributeCards(_players);
        }

        private void SetResult()
        {
            int scoreTeam = _players[0].GetPoints() + _players[1].GetPoints();
            _teams[0].AddScore(scoreTeam);

            scoreTeam = _players[2].GetPoints() + _players[3].GetPoints();
            _teams[1].AddScore(scoreTeam);
        }

        static private void ContractTask(Player player, int minimumContractValue, 
                                         List<Player> players)
        {
            MemoryStream stream = ConnectionManager.Get(player.Connection).Stream;
            Common.PacketType.ContractRequest requ = new Common.PacketType.ContractRequest
            {
                MinimumValue = minimumContractValue
            };
            Serializer.Serialize(stream, requ);
            var netRes = player.Connection.SendReceiveObject<byte[], byte[]>("ChooseContract", "ChooseContractResp",
                                                                             10000, stream.ToArray());

            MemoryStream streamRes = ConnectionManager.Get(player.Connection).Stream;
            streamRes.Write(netRes, 0, netRes.Count());
            var res = Serializer.Deserialize<Common.PacketType.ContractResponse>(streamRes);

            // Notify other players of its choice
            foreach (var curPlayer in players)
            {
                if (curPlayer != player)
                {
                    Common.PacketType.ContractInfo info = new Common.PacketType.ContractInfo
                    {
                        Promise = res.Promise,
                        Color = res.Color,
                        Pseudo = ConnectionManager.Get(curPlayer.Connection).Pseudo
                    };
                    MemoryStream infoStream = ConnectionManager.Get(curPlayer.Connection).Stream;
                    Serializer.Serialize(infoStream, info);
                    player.Connection.SendObject("ChooseContractInfo", infoStream.ToArray());
                }
            }
        }

    }
}
