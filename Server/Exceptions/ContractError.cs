namespace Coinche.Server.Exceptions
{
    /// <summary>
    /// Deck error.
    /// </summary>
    public sealed class ContractError : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.ContractError"/> class.
        /// </summary>
        public ContractError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.ContractError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ContractError(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Exceptions.ContractError"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public ContractError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}