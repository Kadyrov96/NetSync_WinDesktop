using System.Net.Sockets;
using System.Net.Security;

namespace NetSync_WinDesktop
{
    class ClientHandler
    {
        TcpClient client;
        IStreamHandler streamHandler;
        Synchroniser syncService;

        ClientHandler(TcpClient _tcpClient)
        {
            client = _tcpClient;
            streamHandler = new IOStreamHandler(new SslStream(client.GetStream(), false));
        }

        void ServeClient()
        {
            string profileName = streamHandler.ReceiveString();
            SyncProfile clientProfile = SyncProfilesHandler.SelectedProfilesList.Find(
                profile => profile.ProfileName == profileName);

            if (clientProfile !=null)
            {
                syncService = new Synchroniser(new FolderHandler(clientProfile.ProfileSyncFolderPath));
                syncService.Synchronise(streamHandler);
            }
            else
                streamHandler.SendString();
        }
    }
}
