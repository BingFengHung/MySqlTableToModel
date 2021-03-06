namespace MySqlTableToModel
{
    public partial class MainWindow
    {
        class InformationSchema
        {
            public string TABLE_CATALOG { get; set; }

            public string TABLE_SCHEMA { get; set; }

            public string TABLE_NAME { get; set; }

            public string COLUMN_NAME { get; set; }

            public string ORDINAL_POSITION { get; set; }

            public string COLUMN_DEFAULT { get; set; }

            public string IS_NULLABLE { get; set; }

            public string DATA_TYPE { get; set; }

            public string CHARACTER_MAXIMUM_LENGTH { get; set; }

            public string CHARACTER_OCTET_LENGTH { get; set; }

            public string NUMERIC_PRECISION { get; set; }

            public string NUMERIC_SCALE { get; set; }

            public string DATETIME_PRECISION { get; set; }

            public string CHARACTER_SET_NAME { get; set; }

            public string COLLATION_NAME { get; set; }

            public string COLUMN_TYPE { get; set; }

            public string COLUMN_KEY { get; set; }

            public string EXTRA { get; set; }

            public string PRIVILEGES { get; set; }

            public string COLUMN_COMMENT { get; set; }

            public string GENERATION_EXPRESSION { get; set; }
        }
    }
}
