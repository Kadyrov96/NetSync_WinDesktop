using System;
using System.Windows;
using System.Windows.Controls;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для ProfileAddingWindow.xaml
    /// </summary>
    public partial class ProfileAddingWindow
    {
        Synchroniser syncService;
        FolderHandler folderHandler;
        internal static ListView view1;

        public ProfileAddingWindow()
        {
            InitializeComponent();
            folderHandler = new FolderHandler();
            view1 = new ListView();
        }

        private void SelectFolder_Btn_Click(object sender, RoutedEventArgs e)
        {
            folderHandler.SelectFolder();
            syncFolder.Text = folderHandler.FolderPath;
        }

        private void StopSyncService_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SyncProfilesMenu_Btn_Click(object sender, RoutedEventArgs e)
        {
            SyncProfilesHandler.AddNewProfile(syncProfileName.Text, syncFolder.Text);

            syncService = new Synchroniser(folderHandler);
            syncService.CreateSyncDataStore();

            Close();
        }
    }
}
