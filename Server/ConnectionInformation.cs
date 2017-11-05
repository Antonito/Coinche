using System;
using Coinche.Server.Utils;
using NetworkCommsDotNet.Connections;
             
namespace Coinche.Server
{
    public class ConnectionInformation
    {
        private readonly Connection _connection;
        private readonly SetOnce<string> _pseudo;
        private Lobby _lobby;

        public string Pseudo
        {
            get { return _pseudo.Value; }
            set { _pseudo.Value = value; }
        }

        public Lobby Lobby
        {
            get { return _lobby; }
            set 
            { 
                if (!value.IsInLobby(_connection)) 
                {
                    throw new Exception.LobbyError("Client is not in lobby");      
                }
                _lobby = value; 
            }
        }

        public ConnectionInformation(Connection connection)
        {
            _connection = connection;
            _pseudo = new SetOnce<string>();
            _lobby = null;
        }
    }
}
