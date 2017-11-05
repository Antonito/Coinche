using System;
namespace Coinche.Server.Core
{
    public class Contract
    {
        private readonly int _value;
        private readonly Card.CardColor _color;

        public int Value { get { return _value; }}
        public Card.CardColor Color { get { return _color; }}
        public Contract(int valueToSet, Card.CardColor colorToSet)
        {
            _value = valueToSet;
            _color = colorToSet;
        }
    }
}
