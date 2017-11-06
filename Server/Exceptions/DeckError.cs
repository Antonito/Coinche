namespace Coinche.Server.Exceptions
{
    /// <summary>
    /// Deck error.
    /// </summary>
    public sealed class DeckError : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.DeckError"/> class.
        /// </summary>
        public DeckError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.DeckError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public DeckError(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.DeckError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public DeckError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}