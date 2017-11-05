using System.Linq;
using System.Collections.Generic;

namespace Coinche.Server
{
    public static class LobbyManager
    {
        static private readonly List<Lobby> _lobbies = new List<Lobby>();

        static public void AddLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                throw new Exceptions.LobbyError("Lobby already exists");
            }
            _lobbies.Add(new Lobby(name));
        }

        static public Lobby GetLobby(Lobby lobby)
        {
            return _lobbies.FirstOrDefault(l => l.Name == lobby.Name);
        }

        static public Lobby GetLobby(string name)
        {
            return _lobbies.FirstOrDefault(l => l.Name == name);
        }

        static public void DeleteLobby(string name)
        {
            var matchingvalue = _lobbies.FirstOrDefault(l => l.Name == name);
            if (matchingvalue != null)
            {
                DeleteLobby(matchingvalue);
            }
        }

        static public void DeleteLobby(Lobby lobby)
        {
            _lobbies.Remove(lobby);
        }
    }
}
