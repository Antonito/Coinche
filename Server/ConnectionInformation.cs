using Coinche.Server.Utils;
             
namespace Coinche.Server
{
    public class ConnectionInformation
    {
        private SetOnce<string> _pseudo;

        public string Pseudo
        {
            get { return _pseudo.Value; }
            set { _pseudo.Value = value; }
        }

        public ConnectionInformation()
        {
            _pseudo = new SetOnce<string>();
        }
    }
}
