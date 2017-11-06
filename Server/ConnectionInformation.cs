using System;
using Coinche.Server.Utils;
using NetworkCommsDotNet.Connections;
             
namespace Coinche.Server
{
    /// <summary>
    /// This class contains informations concerning one connection
    /// </summary>
    public sealed class ConnectionInformation
    {
        /// <summary>
        /// The connection.
        /// </summary>
        private readonly Connection _connection;

        /// <summary>
        /// The pseudo (can only be set once).
        /// </summary>
        private readonly SetOnce<string> _pseudo;

        /// <summary>
        /// The lobby.
        /// </summary>
        private Lobby _lobby;

        /// <summary>
        /// Gets or sets the pseudo.
        /// The pseudo can be set only once
        /// </summary>
        /// <value>The pseudo.</value>
        public string Pseudo
        {
            get { return _pseudo.Value; }
            set { _pseudo.Value = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Coinche.Server.ConnectionInformation"/> is game ready.
        /// </summary>
        /// <value><c>true</c> if is game ready; otherwise, <c>false</c>.</value>
        public bool IsGameReady { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Coinche.Server.ConnectionInformation"/> is round ready.
        /// </summary>
        /// <value><c>true</c> if is round ready; otherwise, <c>false</c>.</value>
        public bool IsRoundReady { get; set; } = false;

        /// <summary>
        /// Gets or sets the lobby.
        /// </summary>
        /// <value>The lobby.</value>
        public Lobby Lobby
        {
            /// <summary>
            /// Gets the lobby.
            /// </summary>
            /// <returns>The lobby.</returns>
            get { return _lobby; }

            /// <summary>
            /// Sets the lobby.
            /// </summary>
            /// <param name="value">Value.</param>
            /// <exception cref="Exceptions.LobbyError">
            /// Throws if the client is already in the lobby
            /// </exception>
            set 
            { 
                // Check that the client is already in the lobby
                if (value != null && !value.IsInLobby(_connection)) 
                {
                    throw new Exceptions.LobbyError("Client is not in lobby");      
                }
                _lobby = value; 
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.ConnectionInformation"/> class.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public ConnectionInformation(Connection connection)
        {
            _connection = connection;
            _pseudo = new SetOnce<string>();
            _lobby = null;
        }
    }
}
