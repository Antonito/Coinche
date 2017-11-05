using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server
{
    // Manage all the lobbies
    // Lobbies are identified by their name
    public static class LobbyManager
    {
        static private readonly List<Lobby> _lobbies = new List<Lobby>();

        // Create a new lobby, if it does not exist already
        static public void AddLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                throw new Exceptions.LobbyError("Lobby already exists");
            }
            _lobbies.Add(new Lobby(name));
        }

        // Get a lobby, by name
        static public Lobby GetLobby(Lobby lobby)
        {
            return _lobbies.FirstOrDefault(l => l.Name == lobby.Name);
        }

        // Get a lobby, by name
        static public Lobby GetLobby(string name)
        {
            return _lobbies.FirstOrDefault(l => l.Name == name);
        }

        // Delete a lobby, by name
        static public void DeleteLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                DeleteLobby(matchingvalue);
            }
        }

        // Delete a lobby, by name
        static public void DeleteLobby(Lobby lobby)
        {
            _lobbies.Remove(lobby);
        }
    }
}
