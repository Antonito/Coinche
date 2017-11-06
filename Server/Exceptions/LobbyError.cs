namespace Coinche.Server.Exceptions
{
    /// <summary>
    /// LobbyError exception
    /// </summary>
    public sealed class LobbyError : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.LobbyError"/> class.
        /// </summary>
        public LobbyError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.LobbyError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public LobbyError(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.LobbyError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public LobbyError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}