using System.Windows;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Логика взаимодействия для NetSettingsWindows.xaml
    /// </summary>
    public partial class NetSettingsWindows
    {
        public NetSettingsWindows()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            Top = (screenHeight - Height) / 2 + 150;
            Left = (screenWidth - Width) / 2 + 150;
            ip.IsReadOnly = true;
            hostname.IsReadOnly = true;
        }
    }
}
