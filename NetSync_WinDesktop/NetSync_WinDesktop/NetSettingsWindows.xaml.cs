using System.Windows;
using System.Net;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для NetSettingsWindows.xaml
    /// </summary>
    public partial class NetSettingsWindows
    {
        string hostName;
        IPAddress ipAddr;
        public NetSettingsWindows()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2 + 150;
            Left = (screenWidth - Width) / 2 + 150;

            hostName = Dns.GetHostName();
            hostname.Text = hostName;
            hostname.IsReadOnly = true;

            ipAddr = Dns.GetHostByName(hostName).AddressList[0];
            ip.Text = ipAddr.ToString();
            ip.IsReadOnly = true;
        }
    }
}
