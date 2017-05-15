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
        static string certificatePath = @"C:\Users\Admin\Desktop\123.pfx";
        static string certificatePassword = "vbhs456";
        static X509Certificate2 serverCertificate = new X509Certificate2(certificatePath, certificatePassword);

        const int port = 8888;
        static IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

        public static SslStream SSLStream { get; private set; }

        static public void Proccess()
        {
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            System.Windows.MessageBox.Show("server started");

            try
            {
                while (true)
                {
                    System.Windows.MessageBox.Show("waiting");
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        if (client.Connected)
                        {
                            SSLStream = new SslStream(client.GetStream(), false);
                            //sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls12, true);
                            System.Windows.MessageBox.Show("client connected");

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
