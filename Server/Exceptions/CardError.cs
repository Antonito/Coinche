namespace Coinche.Server.Exceptions
{
    /// <summary>
    /// Deck error.
    /// </summary>
    public sealed class CardError : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.CardError"/> class.
        /// </summary>
        public CardError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.CardError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public CardError(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.CardError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public CardError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}