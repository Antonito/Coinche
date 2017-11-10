using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;

namespace Coinche.Server.Core
{
    using GameMode = Coinche.Common.Core.Game.GameMode;

    /// <summary>
    /// Fold.
    /// </summary>
    public sealed class Fold
    {
        /// <summary>
        /// The players.
        /// </summary>
        private readonly List<Player> _players;

        /// <summary>
        /// The game mode.
        /// </summary>
        private readonly GameMode _gameMode;

        /// <summary>
        /// The deck.
        /// </summary>
        private readonly Deck _deck;

        /// <summary>
        /// The cards.
        /// </summary>
        private readonly List<Card> _cards;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Core.Fold"/> class.
        /// </summary>
        /// <param name="players">Players.</param>
        /// <param name="gameMode">Game mode.</param>
        /// <param name="deck">Deck.</param>
        public Fold(List<Player> players, GameMode gameMode, Deck deck)
        {
            _players = players;
            _gameMode = gameMode;
            _deck = deck;
            _cards = new List<Card>();
        }

        /// <summary>
        /// Play.
        /// </summary>
        public void Run(out Player winner)
        {
            bool mustPlayAsset = false;
            bool isFirstLap = true;
         
            foreach (var player in _players)
            {
                Task.Run(() =>
                {
                    bool ret = false;
                    do
                    {
                        ret = AskCardToPlayer(player, mustPlayAsset);
                    } while (!ret);
                }).Wait();

                if (isFirstLap)
                {
                    mustPlayAsset |= _deck.IsCardAsset(_cards[0]);
                    isFirstLap = false;
                }
            }

            var maxIndex = FindMaxCardIndex();
            winner = _players[maxIndex];
            var points = _cards.Sum(c => _deck.GetCardValue(c));

            winner.Score = points;
            winner.WinFold();
        }

        /// <summary>
        /// Asks a card to the player.
        /// </summary>
        /// <param name="player">Player.</param>
        private bool AskCardToPlayer(Player player, bool mustPlayAsset)
        {
            bool ret = false;
            try
            {
                byte[] netRes = player.Connection.SendReceiveObject<byte[]>("GiveMeCard", "GiveCard", Timeout.Infinite);
                Common.PacketType.PlayCard res;
                using (var stream = new MemoryStream(netRes))
                {
                    res = Serializer.Deserialize<Common.PacketType.PlayCard>(stream);
                }

                Card cur = new Card(res.CardValue, res.CardColor);
                if (player.HaveCard(cur))
                {
                    if (player.HaveColor(_cards.First().Color))
                    {
                        if (cur.Color != _cards.First().Color)
                        {
                            throw new Exceptions.CardError("Player must follow the color, when possible");   
                        }
                    }
                    else if (player.HaveAsset())
                    {
                        if (mustPlayAsset && _deck.IsCardAsset(cur) &&
                            _deck.GetCardValue(cur) > _deck.GetCardValue(_cards.Last()) ||
                            !_deck.IsCardAsset(cur))
                        {
                            throw new Exceptions.CardError("Player must play an asset.");   
                        }
                    }
                    _cards.Add(cur);
                    player.PlayCard(cur);
                    ret = true;
                }
            }
            catch (Exception)
            {
                player.Connection.SendObject("InvalidCard");
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// Finds the index of the biggest card.
        /// </summary>
        /// <returns>The biggest card index.</returns>
        private int FindMaxCardIndex()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            int maxVal = int.MinValue;
            Card biggest = null;
            foreach (Card card in _cards)
            {
                var val = _deck.GetCardValue(card);
                if (val > maxVal)
                {
                    maxVal = val;
                    biggest = card;
                }
            }
            return _cards.IndexOf(biggest);
        }
    }
}
