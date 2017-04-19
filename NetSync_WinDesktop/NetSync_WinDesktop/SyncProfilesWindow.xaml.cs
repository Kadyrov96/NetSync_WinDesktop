using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для SyncProfilesWindow.xaml
    /// </summary>
    public partial class SyncProfilesWindow
    {
        public SyncProfilesWindow()
        {
            InitializeComponent();
            if(File.Exists(Directory.GetCurrentDirectory() + @"\profiles.dat"))
            {
                SyncProfilesHandler.LoadProfiles();
                foreach (var item in SyncProfilesHandler.AvailableProfilesList)
                {
                    profilesListView.Items.Add(item);
                }
            }


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
            var addNewProfileWindow = new ProfileAddingWindow();
            addNewProfileWindow.ShowDialog();
            //Application.Current.Windows.Contain(ProfileAddingWindow);
            //this.profilesListView

            //listView1.Items.Add(new FileItem(syncDirPath + localNamesList[i], imageDel.Source, imageWait.Source, 140));
        }
    }
}
