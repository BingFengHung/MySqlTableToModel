using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;

namespace MySqlTableToModel
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _tableName;
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                NotifyPropertyChange(nameof(TableName));
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                NotifyPropertyChange(nameof(Code));
            }
        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var iniFile = new IniFile("../../../../Config/db.ini");
            var ip = iniFile.Read("ip", "database");
            var user = iniFile.Read("user", "database");
            var password = iniFile.Read("password", "database");
            var table = iniFile.Read("table", "database");

            Tables sqlConnectBase = new Tables(ip, user, password, table);
            sqlConnectBase.GetTableInformation(TableName, out var informations);
            ConvertToCode(informations);
        }

        private void ConvertToCode(List<InformationSchema> informations)
        {
            string className = TableName.Replace("_", " ");
            className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(className);
            className = className.Replace(" ", "");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System");
            sb.AppendLine();
            sb.AppendLine($"namespace NAMESPACE");
            sb.AppendLine("{");
            sb.AppendLine($"    class {TableName}");
            sb.AppendLine("    {");
            foreach (var information in informations)
            {
                string columnType = "object";
                var type = information.COLUMN_TYPE;

                if (type.Contains("int"))
                    columnType = "int";
                else if (type.Contains("var") || type.Contains("text"))
                    columnType = "string";
                else if (type.Contains("datetime"))
                    columnType = "DateTime";

                sb.AppendLine($"      public {columnType} {information.COLUMN_NAME} {{ get; set;}}");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            Code = sb.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string fileName = TableName.Replace("_", " ");
            fileName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fileName);
            fileName = fileName.Replace(" ", "");
            System.IO.File.WriteAllText($"{fileName}.cs", Code);
        }

        private void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
