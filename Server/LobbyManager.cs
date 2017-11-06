using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server
{
    /// <summary>
    /// Manage all the lobbies
    /// Lobbies are identified by their name
    /// </summary>
    public static class LobbyManager
    {
        static private readonly List<Lobby> _lobbies = new List<Lobby>();

        /// <summary>
        /// Adds a lobby, if it does not exists already.
        /// </summary>
        /// <param name="name">Name.</param>
        static public void AddLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                throw new Exceptions.LobbyError("Lobby already exists");
            }
            _lobbies.Add(new Lobby(name));
        }

        /// <summary>
        /// Gets the lobby.
        /// </summary>
        /// <returns>The lobby.</returns>
        /// <param name="lobby">Lobby.</param>
        static public Lobby GetLobby(Lobby lobby)
        {
            return _lobbies.FirstOrDefault(l => l.Name == lobby.Name);
        }

        /// <summary>
        /// Get a lobby, by name
        /// </summary>
        /// <returns>The lobby.</returns>
        /// <param name="name">Name.</param>
        static public Lobby GetLobby(string name)
        {
            return _lobbies.FirstOrDefault(l => l.Name == name);
        }

        /// <summary>
        /// Delete a lobby, by name
        /// </summary>
        /// <param name="name">Name.</param>
        static public void DeleteLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                DeleteLobby(matchingvalue);
            }
        }

        /// <summary>
        /// Deletes the lobby.
        /// </summary>
        /// <param name="lobby">Lobby.</param>
        static public void DeleteLobby(Lobby lobby)
        {
            _lobbies.Remove(lobby);
        }
    }
}
