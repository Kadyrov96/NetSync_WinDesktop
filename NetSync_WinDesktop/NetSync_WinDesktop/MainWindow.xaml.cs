using System.Threading;
using System.Windows;
using System.Collections.Generic;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Thread thread;
        private SSL_Server sslServer;
        public MainWindow()
        {
            InitializeComponent();

            sslServer = new SSL_Server();
            thread = new Thread(sslServer.RunServer);
            thread.Start();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }


        private void ConnectionSettings_Btn_Click(object sender, RoutedEventArgs e)
        {
            var connectSettingsWindow = new NetSettingsWindows();
            connectSettingsWindow.ShowDialog();
        }

        private void StartSyncService_Click(object sender, RoutedEventArgs e)
        {
            /*
            if(!thread.IsAlive)
            {
                thread.Start();
            }
            */
        }

        private void StopSyncService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SyncProfilesMenu_Btn_Click(object sender, RoutedEventArgs e)
        {
            var SyncProfilesWindow = new SyncProfilesWindow();
            SyncProfilesWindow.ShowDialog();
        }
    }
}
