using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Tools;

namespace Coinche.Common
{
    public class SSL
    {
        private readonly X509Certificate _certificate;
        private readonly SSLOptions _listenerSSLOptions;
        private readonly SSLOptions _connectionSSLOptions;
        private readonly SendReceiveOptions _sendingSendReceiveOptions;

        public SSLOptions ListenerOptions 
        { 
            get { return _listenerSSLOptions; } 
        }
        public SSLOptions ConnectionOptions 
        { 
            get { return _connectionSSLOptions; } 
        }
        public SendReceiveOptions SendingSendReceiveOptions 
        { 
            get { return _sendingSendReceiveOptions; }
        }

        public SSL(string certName)
        {
            if (!File.Exists(certName))
            {
                // Create a new certificate
                CertificateDetails details = 
                    new CertificateDetails("CN=bache_a_troncy_l_netcoinche2020.net", 
                                           DateTime.Now, DateTime.Now.AddYears(1));


                details.KeyLength = 2048;
                SSLTools.CreateSelfSignedCertificatePFX(details, certName);
                _certificate = new X509Certificate2(certName);
                Console.WriteLine("\t... certificate successfully created.");
            }
            else
            {
                // Load an existing certificate
                _certificate = new X509Certificate2(certName);
            }
            // Require clients to provide certificate
            _listenerSSLOptions = new SSLOptions(_certificate, true, true);

            // Provide certificate for outgoing connections
            _connectionSSLOptions = new SSLOptions(_certificate, true);

            // De-activate padding
            _sendingSendReceiveOptions = NetworkComms.DefaultSendReceiveOptions;
        }
    }
}
