using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using System.Collections.Generic;

namespace NetSync_WinDesktop
{
    public class SSL_Server
    {
        private SslStream sslStream;
        private List<TcpClient> tcpClientsList;
        public List<TcpClient> TcpClientsList
        {
            get
            {
                return tcpClientsList;
            }

            private set
            {
                tcpClientsList = value;
            }
        }

        private TcpListener listener;
        //private X509Certificate2 serverCertificate;

        public SSL_Server()
        {
            string host = Dns.GetHostName();
            //serverCertificate = new X509Certificate2(@"C:\Users\Admin\Desktop\123.pfx", "vbhs456");
            listener = new TcpListener(Dns.GetHostEntry(host).AddressList[0], 816);
        }
        internal void RunServer()
        {
            listener.Start();
            System.Windows.MessageBox.Show("waiting");
            tcpClientsList.Add(listener.AcceptTcpClient());
            //client = listener.AcceptTcpClient();
            TcpClient lastClient = tcpClientsList[tcpClientsList.Count];
            if (lastClient.Connected)
            {
                System.Windows.MessageBox.Show("Client connected");
                sslStream = new SslStream(lastClient.GetStream(), false);
                try
                {
                    //sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls12, true);
                }
                catch (AuthenticationException e)
                {
                    System.Windows.MessageBox.Show("Exception: {0}", e.Message);
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        System.Windows.MessageBox.Show("Inner exception: {0}", e.InnerException.Message);
                    }
                    System.Windows.MessageBox.Show("Authentication failed - closing the connection.");
                    sslStream.Close();
                    lastClient.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("still empty");
            }
        }

        internal void StopServer()
        {
            sslStream.Close();
            foreach (var client in tcpClientsList)
            {
                client.Client.Dispose();
            }
            
            listener.Stop();
        }
    }
}
                /*
                switch (FinalWindow.iterationFlag)
                {
                    case 0:
                        AcceptFile(@"C:\Users\Admin\Desktop\Hashes\");
                        FinalWindow.iterationFlag++;
                        break;

                    case 2:
                        SendFile(FinalWindow.HashHolderDirectory + "ST" + @".txt");
                        sslStream.WriteTimeout = 4000;
                        for (int i = 0; i < FinalWindow.uplFilesList.Count; i++)
                        {
                            SendFile(FinalWindow.syncDirPath + @"\" + FinalWindow.uplFilesList[i]);
                        }

                        for (int i = 0; i < FinalWindow.download_count; i++)
                        {
                            AcceptFile(FinalWindow.syncDirPath + @"\");                                    
                        }
                        break;
                }
                */
