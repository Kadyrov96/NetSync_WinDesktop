using System;
using System.Windows;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для ProfileAddingWindow.xaml
    /// </summary>
    public partial class ProfileAddingWindow
    {
        //Delegates for updating the ListView in the SyncProfilesWindow
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;

        Synchroniser syncService;
        FolderHandler folderHandler;

        public ProfileAddingWindow()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;

            folderHandler = new FolderHandler();
        }

        private void Ok_Btn_Click(object sender, RoutedEventArgs e)
        {
            DataChangedEventHandler handler = DataChanged;
            bool isAdded = SyncProfilesHandler.AddNewProfile(syncProfileName.Text, syncFolder.Text);
            if (isAdded)
            {
                syncService = new Synchroniser(folderHandler);
                syncService.CreateSyncDataStore();
                Close();
                handler?.Invoke(this, new EventArgs());
            }
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelectFolder_Btn_Click(object sender, RoutedEventArgs e)
        {
            folderHandler.SelectFolder();
            syncFolder.Text = folderHandler.FolderPath;
        }
    }
}
