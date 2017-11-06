using System.IO;
using Coinche.Server.Utils;
using NetworkCommsDotNet.Connections;
             
namespace Coinche.Server
{
    // This class contains informations concerning one connection
    public sealed class ConnectionInformation
    {
        private readonly Connection _connection;
        private readonly SetOnce<string> _pseudo;
        private readonly MemoryStream _stream;
        private Lobby _lobby;

        public string Pseudo
        {
            get { return _pseudo.Value; }
            set { _pseudo.Value = value; }
        }

        //TODO: check this value to readonly
        public bool IsGameReady { get; set; } = false;
        public bool IsRoundReady { get; set; } = false;
        public MemoryStream Stream
        {
            get
            {
                _stream.Position = 0;
                _stream.SetLength(0);
                return _stream;
            }
        }

        public Lobby Lobby
        {
            get { return _lobby; }
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

        public ConnectionInformation(Connection connection)
        {
            _connection = connection;
            _pseudo = new SetOnce<string>();
            _lobby = null;
            _stream = new MemoryStream();
        }
    }
}
