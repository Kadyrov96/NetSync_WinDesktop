using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetSync_WinDesktop
{
    class ClientHandler
    {
        private TcpClient client;
        private IStreamHandler streamHandler;
        private string clientProfile;
        private Synchroniser syncService;

        ClientHandler(TcpClient _tcpClient)
        {
            client = _tcpClient;
            streamHandler = new IOStreamHandler(new SslStream(client.GetStream(), false));
        }

        void ServeClient()
        {
            clientProfile = streamHandler.ReceiveString();

            syncService = new Synchroniser(new FolderHandler(clientProfile));
            syncService.CheckLocalChanges();

            streamHandler.ReceiveData(Directory.GetCurrentDirectory());
            streamHandler.SendData(syncService.CompareDevicesSyncData());

            //TO DO - отправка и прием файлов // 
        }

    }
}
