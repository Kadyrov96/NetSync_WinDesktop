using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static List<string> syncProfilesList;
        private Thread thread;
        private SSL_Server sslServer;
        public MainWindow()
        {
            InitializeComponent();
            HandleTray();

            sslServer = new SSL_Server();
            thread = new Thread(sslServer.RunServer);
            thread.Start();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }

        void HandleTray()
        {
            MenuItem menuItem1 = new MenuItem("&Show up main menu");
            MenuItem menuItem2 = new MenuItem("&Close the app");

            menuItem1.Click +=
            delegate (object sender, EventArgs args)
            {
                Show();
                WindowState = WindowState.Normal;
            };

            menuItem2.Click +=
            delegate (object sender, EventArgs args)
            {
                Environment.Exit(1);
            };

            NotifyIcon ni = new NotifyIcon();
            ni.Icon = new Icon(@"C:\Users\kadyr\Source\Repos\NetSync_WinDesktop\NetSync_WinDesktop\Drawable\logo.ico");
            ni.ContextMenu = new ContextMenu();

            ni.ContextMenu.MenuItems.Add(menuItem1);
            ni.ContextMenu.MenuItems.Add(menuItem2);
            ni.Visible = true;
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            base.OnStateChanged(e);
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
