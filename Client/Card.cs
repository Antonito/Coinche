using Coinche.Common.Core.Cards;

namespace Coinche.Client
{
    /// <summary>
    /// Card.
    /// </summary>
    public struct Card
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public CardType Value { get; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public CardColor Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Client.Card"/> struct.
        /// </summary>
        /// <param name="val">Value.</param>
        /// <param name="color">Color.</param>
        public Card(CardType val, CardColor color)
        {
            Value = val;
            Color = color;
        }
    }
}
