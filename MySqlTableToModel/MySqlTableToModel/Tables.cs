using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MySqlTableToModel
{
    public partial class MainWindow
    {
        class Tables : SqlConnectBase
        {
            public Tables(string ip, string user, string password, string schema)
                : base(ip, user, password, schema)
            {
            }

            public void GetTableInformation(string tableName, out List<InformationSchema> informationSchemas)
            {
                string command = $@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='hartnet' AND TABLE_NAME='{tableName}';";
                bool isOk = GetData(command, out var dataTable);

                List<InformationSchema> informations = new List<InformationSchema>();

                InformationSchema information = null;

                if (isOk)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        information = new InformationSchema();
                        foreach (PropertyInfo property in information.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            property.SetValue(information, row[property.Name], null);
                        }
                        informations.Add(information);
                    }
                }

                informationSchemas = informations;
            }
        }
    }
}
