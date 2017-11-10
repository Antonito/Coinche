using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Coinche.Common.Core.Cards;
using Coinche.Common.PacketType;

namespace Coinche.Client
{
    public class ClientInformation
    {
        private Object thisLock = new Object();

        private readonly List<Card> _cards;
        private readonly List<Card> _cardsPlayed;

        public bool IsRun { get; set; }

        public ClientInformation()
        {
            IsRun = false;
            _cards = new List<Card>();
            _cardsPlayed = new List<Card>();
        }

        public void ResetCards()
        {
            lock (thisLock)
            {
                _cards.Clear();
                _cardsPlayed.Clear();
            }
        }

        public void AddCard(PlayCard card)
        {
            lock (thisLock)
            {
                _cards.Add(new Card(card.CardValue, card.CardColor));
            }
        }

        public void RevertPlay()
        {
            lock (thisLock)
            {
                _cards.Add(_cardsPlayed.Last());
                _cardsPlayed.Remove(_cardsPlayed.Last());
            }
        }

        public List<Card> GetCards()
        {
            lock (thisLock)
            {
                return _cards;
            }
        }

        public CardType GetCardType(int index)
        {
            return _cards[index].Value;
        }

        public CardColor GetCardColor(int index)
        {
            return _cards[index].Color;
        }

        public void PlayCard(int index)
        {
            lock (thisLock)
            {
                _cardsPlayed.Add(_cards[index]);
                _cards.RemoveAt(index);
            }
        }
    }
}
