using System.Net.Sockets;
using System.Net.Security;
using System.IO;

namespace NetSync_WinDesktop
{
    class ClientHandler
    {
        TcpClient client;
        IStreamHandler streamHandler;
        SyncProfile clientProfile;
        Synchroniser syncService;

        ClientHandler(TcpClient _tcpClient)
        {
            client = _tcpClient;
            streamHandler = new IOStreamHandler(new SslStream(client.GetStream(), false));
        }

        void ServeClient()
        {
            clientProfile.ProfileName = streamHandler.ReceiveString();

            if (MainWindow.syncProfilesList.Contains(clientProfile.ProfileName))
            {
                syncService = new Synchroniser(new FolderHandler(clientProfile.ProfileSyncFolderPath));
                syncService.CheckLocalChanges();

                streamHandler.ReceiveData(Directory.GetCurrentDirectory());
                streamHandler.SendData(syncService.CompareDevicesSyncData());

                //TO DO - отправка и прием файлов // 
            }
        }

    }
}
