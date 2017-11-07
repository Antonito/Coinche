using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;
using Coinche.Server.Core;
using Coinche.Server.Utils;

namespace Coinche.Server
{
    /// <summary>
    /// Lobby.
    /// </summary>
    public sealed class Lobby
    {
        /// <summary>
        /// The name.
        /// </summary>
        private readonly string _name;

        private readonly SetOnce<bool> _isStarted;

        /// <summary>
        /// The connections.
        /// </summary>
        private readonly List<Connection> _connections;

        /// <summary>
        /// Gets the name of the lobby.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Coinche.Server.Lobby"/> is started.
        /// </summary>
        /// <value><c>true</c> if is started; otherwise, <c>false</c>.</value>
        public bool IsStarted {
            
            get
            {
                return _isStarted.Value;
            }

            set
            {
                _isStarted.Value = value;
            }
        }

        /// <summary>
        /// Gets the number of players.
        /// </summary>
        /// <value>The nb players.</value>
        public int NbPlayers { get { return _connections.Count(); } }


        /// <summary>
        /// Gets the list of players.
        /// </summary>
        /// <value>The connection.</value>
        // TODO: Improve with something using yield ?
        public List<Connection> Connection { get { return _connections; }}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Coinche.Server.Lobby"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        public Lobby(string name)
        {
            _name = name;
            _connections = new List<Connection>();
            _isStarted = new SetOnce<bool>();
        }

        /// <summary>
        /// Adds the player to the lobby.
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <exception cref="Exceptions.LobbyError">
        /// Throws if the lobby if full or if the client is already in the lobby
        /// </exception>
        public void AddPlayer(Connection connection)
        {
            if (IsFull())
            {
                throw new Exceptions.LobbyError("Lobby is full.");
            }
            if (IsInLobby(connection)) 
            {
                throw new Exceptions.LobbyError("Client is already in lobby.");
            }
            _connections.Add(connection);
        }

        /// <summary>
        /// Check if a player is in the lobby
        /// </summary>
        /// <returns><c>true</c>, if if the player is in the lobby, <c>false</c> otherwise.</returns>
        /// <param name="connection">Connection.</param>
        public bool IsInLobby(Connection connection)
        {
            return _connections.Contains(connection);
        }

        /// <summary>
        /// Removes the player.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public void RemovePlayer(Connection connection)
        {
            _connections.Remove(connection);
            if (_connections.Count() == 0)
            {
                LobbyManager.DeleteLobby(_name);
            }
        }

        /// <summary>
        /// Is the lobby full ?
        /// </summary>
        /// <returns><c>true</c>, if lobby is full, <c>false</c> otherwise.</returns>
        public bool IsFull()
        {
            return _connections.Count() == 4;
        }
    }
}
