using System.Windows;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }


        private void ConnectionSettings_Btn_Click(object sender, RoutedEventArgs e)
        {
            var form = new NetSettingsWindows();
            form.ShowDialog();
        }

        private void StartSyncService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopSyncService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SyncProfilesMenu_Btn_Click(object sender, RoutedEventArgs e)
        {
            var form = new SyncProfilesWindow();
            form.ShowDialog();
        }
    }
}
