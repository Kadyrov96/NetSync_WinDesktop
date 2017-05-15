using System.Net.Security;

namespace NetSync_WinDesktop
{
    public class ClientHandler
    {
        IStreamHandler streamHandler;
        Synchroniser syncService;

        public ClientHandler(SslStream sslStream)
        {
            streamHandler = new IOStreamHandler(sslStream);
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
