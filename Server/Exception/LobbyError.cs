namespace Coinche.Server.Exception
{
    public class LobbyError : System.Exception
    {
        public LobbyError()
        {
        }

        public LobbyError(string message)
        : base(message)
        {
        }

        public LobbyError(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}