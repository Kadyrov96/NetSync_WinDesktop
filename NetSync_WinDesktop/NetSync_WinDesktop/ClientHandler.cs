using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace NetSync_WinDesktop
{
    public class ClientHandler
    {
        TcpClient client;
        IStreamHandler streamHandler;
        Synchroniser syncService;
        X509Certificate2 serverCertificate;

        public ClientHandler(TcpClient _tcpClient)
        {
            client = _tcpClient;
            SslStream sslStream = new SslStream(client.GetStream(), false);
            //serverCertificate = new X509Certificate2(@"C:\Users\Admin\Desktop\123.pfx", "vbhs456");
            try
            {
                //sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls12, true);
                streamHandler = new IOStreamHandler(sslStream);
            }
            catch (AuthenticationException e)
            {
                System.Windows.MessageBox.Show("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    System.Windows.MessageBox.Show("Inner exception: {0}", e.InnerException.Message);
                }
                System.Windows.MessageBox.Show("Authentication failed - closing the connection.");
                sslStream.Close();
            }
        }

        public void ServeClient()
        {
            string profileName = streamHandler.ReceiveString();
            SyncProfile clientProfile = SyncProfilesHandler.SelectedProfilesList.Find(
                profile => profile.ProfileName == profileName);

            if (clientProfile !=null)
            {
                syncService = new Synchroniser(new FolderHandler(clientProfile.ProfileSyncFolderPath));
                syncService.Synchronise(streamHandler);
                SyncProfilesHandler.UpdateProfile(clientProfile);
            }
            else
                streamHandler.SendString();
        }
    }
}
