using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetSync_WinDesktop
{
    public class TCP_Server
    {
        const int port = 8888;
        static IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

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
                            System.Windows.MessageBox.Show("client connected");
                            ClientHandler clienthandler = new ClientHandler(client);

                            Thread clientthread = new Thread(new ThreadStart(clienthandler.ServeClient));
                            clientthread.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Exception: {0}", ex.Message);
                if (ex.InnerException != null)
                    System.Windows.MessageBox.Show("Inner exception: {0}", ex.InnerException.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
