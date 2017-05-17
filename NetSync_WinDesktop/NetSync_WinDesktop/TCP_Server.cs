using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace NetSync_WinDesktop
{
    public class TCP_Server
    {
        const string CERTIFICATE_PATH = @"C:\Users\Admin\Desktop\123.pfx";
        const string CERTIFICATE_PASSWD = "vbhs456";
        const int PORT = 816;

        static IPAddress ipAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];
        static TcpListener listener;
        static TcpClient client;
        static X509Certificate2 serverCertificate = new X509Certificate2(CERTIFICATE_PATH, CERTIFICATE_PASSWD);

        public static SslStream SSLStream { get; private set; }

        public static void Proccess()
        {
            listener = new TcpListener(ipAddress, PORT);
            listener.Start();
            //System.Windows.MessageBox.Show("server started");

            try
            {
                while (true)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        if (client.Connected)
                        {
                            System.Windows.MessageBox.Show("client connected");
                            SSLStream = new SslStream(client.GetStream(), false);
                            //SSLStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls12, true);

                            ClientHandler clienthandler = new ClientHandler(SSLStream);
                            Thread clientThread = new Thread(new ThreadStart(clienthandler.ServeClient));
                            clientThread.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Exception: {0}", ex.Message);
                if (ex.InnerException != null)
                    System.Windows.MessageBox.Show("Inner exception: {0}", ex.InnerException.Message);
                SSLStream.Close();
            }
            finally
            {
                if (listener != null)
                {
                    SSLStream.Close();
                    listener.Stop();
                }
            }
        }
    }
}
