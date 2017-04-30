using System.IO;
using System.Windows;
using System;
using System.Collections.Generic;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для SyncProfilesWindow.xaml
    /// </summary>
    public partial class SyncProfilesWindow
    {
        private List<SyncProfile> profileList;
        public SyncProfilesWindow()
        {
            InitializeComponent();
            if (File.Exists(SyncProfilesHandler.syncProfilesStore))
            {
                SyncProfilesHandler.LoadProfiles();
                RefreshListView();
            }
            else
                SyncProfilesHandler.LoadEmptyProfiles();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }

        private void SyncProfilesMenu_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddNewProfile_Btn_Click(object sender, RoutedEventArgs e)
        {
            ProfileAddingWindow addingWindow = new ProfileAddingWindow();
            addingWindow.DataChanged += addingWindow_DataChanged;
            addingWindow.Show();
        }

        private void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in profilesListView.SelectedItems)
            {
                SyncProfile selectedProfile = (SyncProfile)item;
                SyncProfilesHandler.DeleteProfile(selectedProfile.ProfileName);
            }
            RefreshListView();
        }

        private void RefreshListView()
        {
            profileList = SyncProfilesHandler.AvailableProfilesList;
            profilesListView.Items.Clear();
            foreach (var item in profileList)
                profilesListView.Items.Add(item);
        }
        void addingWindow_DataChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void SyncSelectedProfiles_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in profilesListView.SelectedItems)
                SyncProfilesHandler.SelectedProfilesList.Add((SyncProfile)item);

            if (SyncProfilesHandler.SelectedProfilesList.Count != 0)
            {
                MessageBox.Show("Selected profiles are in queue now. Please, start sync service in main window.",
                    "Succesful choice of profiles", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("No profiles were selected. Please, select profile or create a new one.",
                    "Unsuccesful choice of profiles", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
