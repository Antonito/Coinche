namespace Coinche.Server.Exceptions
{
    // TODO: Rm ?
    /// <summary>
    /// LobbyError exception
    /// </summary>
    public sealed class PlayerError : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.PlayerError"/> class.
        /// </summary>
        public PlayerError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.PlayerError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public PlayerError(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.PlayerError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public PlayerError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}