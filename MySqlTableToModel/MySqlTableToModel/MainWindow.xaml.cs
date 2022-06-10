using System.Windows;

namespace MySqlTableToModel
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var iniFile = new IniFile("../../../../Config/db.ini");
            var ip = iniFile.Read("ip", "database");
            var user = iniFile.Read("user", "database");
            var password = iniFile.Read("password", "database");
            var table = iniFile.Read("table", "database");

            Tables sqlConnectBase = new Tables(ip, user, password, table);
            sqlConnectBase.GetTableInformation("user_nc_data", out var informations);
        }
    }
}
