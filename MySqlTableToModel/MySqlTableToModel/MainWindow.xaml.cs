using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MySqlTableToModel
{
    class RelayCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, (i) => true)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Tables _sqlConnectBase;
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            Init();
        }

        public ICommand TableSelect => new RelayCommand((parameter) =>
        {
            TableName = parameter.ToString();
        });

        public ObservableCollection<string> TableNames { get; set; }

        private void Init()
        {
            var iniFile = new IniFile("../../../../Config/db.ini");
            var ip = iniFile.Read("ip", "database");
            var user = iniFile.Read("user", "database");
            var password = iniFile.Read("password", "database");
            var database = iniFile.Read("database", "database");

            _sqlConnectBase = new Tables(ip, user, password, database);

            GetTables(database);
        }

        private void GetTables(string database)
        {
            _sqlConnectBase.GetTableNames(database, out var tables);

            TableNames = new ObservableCollection<string>(tables);
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
            _sqlConnectBase.GetTableInformation(TableName, out var informations);
            ConvertToCode(informations);
        }


        private void ConvertToCode(List<InformationSchema> informations)
        {
            string className = TextFormat.SnakeCaseToPascalCase(TableName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System");
            sb.AppendLine();
            sb.AppendLine($"namespace NAMESPACE");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {className}");
            sb.AppendLine("    {");
            foreach (var information in informations)
            {
                string columnType = "object";
                var type = information.COLUMN_TYPE;

                if (type.Contains("int"))
                    columnType = "int";
                else if (type.Contains("var(1)") || type.Contains("char(1)"))
                    columnType = "char";
                else if (type.Contains("var") || type.Contains("text"))
                    columnType = "string";
                else if (type.Contains("datetime"))
                    columnType = "DateTime";
                else if (type.Contains("double"))
                    columnType = "double";
                else if (type.Contains("float"))
                    columnType = "float";

                sb.AppendLine($"      public {columnType} {information.COLUMN_NAME} {{ get; set;}}");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            Code = sb.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string fileName = TextFormat.SnakeCaseToPascalCase(TableName);
            System.IO.File.WriteAllText($"{fileName}.cs", Code);
        }

        private void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Code);
        }
    }
}
