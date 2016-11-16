using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;
using BL_JonasSageImporter;
using Jonas_Sage_Importer;
using Jonas_Sage_Importer.Properties;

namespace SageImporterLibrary
{
    public class DbConnectionsCs : IDisposable
    {
        public static string ConnectionString = new SqlConnectionStringBuilder
        {
            PersistSecurityInfo = false,
            DataSource = Settings.Default.DBLocation,
            IntegratedSecurity = false,
            InitialCatalog = Settings.Default.DBName,
            UserID = Settings.Default.DBUsername,
            Password = BL_JonasSageImporter.Business_Layer_Classes.DataEncryptor.DecryptStringAES(Settings.Default.DBPassword, "DBPassword")
        }.ConnectionString;

        public static string EncryptedConnectionString = new SqlConnectionStringBuilder
        {
            PersistSecurityInfo = false,
            DataSource = Settings.Default.DBLocation,
            IntegratedSecurity = false,
            InitialCatalog = Settings.Default.DBName,
            UserID = Settings.Default.DBUsername,
            Password = Settings.Default.DBPassword + " (ENCRYPTED)"
        }.ConnectionString;


        internal OleDbDataAdapter DataAdapter = new OleDbDataAdapter();
        internal BindingSource TableBindingSource = new BindingSource();
        internal DataTable Table = new DataTable();

        public static bool TestConnection(string TestConnectionString)
        {
            LogToText.WriteToLog($"Testing connection - {TestConnectionString}");
            try
            {
                using (SqlConnection conn = new SqlConnection(TestConnectionString))
                {
                    conn.Open();
                    LogToText.WriteToLog($"Connection OK with the connection string - '{TestConnectionString}'");
                }
                return true;
            }
            catch (SqlException ex)
            {
                LogToText.WriteToLog($"Connection Failed with the connection string - '{TestConnectionString}'\n\n {ex.Message}");
                return false;
            }
        }

        private static void UpdateConnectionString()
        {
            ConnectionString = new SqlConnectionStringBuilder
            {
                PersistSecurityInfo = false,
                DataSource = Settings.Default.DBLocation,
                IntegratedSecurity = false,
                InitialCatalog = Settings.Default.DBName,
                UserID = Settings.Default.DBUsername,
                Password = BL_JonasSageImporter.Business_Layer_Classes.DataEncryptor.DecryptStringAES(Settings.Default.DBPassword, "DBPassword")
            }.ConnectionString;
            EncryptedConnectionString = new SqlConnectionStringBuilder
            {
                PersistSecurityInfo = false,
                DataSource = Settings.Default.DBLocation,
                IntegratedSecurity = false,
                InitialCatalog = Settings.Default.DBName,
                UserID = Settings.Default.DBUsername,
                Password = Settings.Default.DBPassword + " (ENCRYPTED)"
            }.ConnectionString;
        }

        public static void UpdateConnection(string dbLocation, string dbName, string userName, string password)
        {
            password = BL_JonasSageImporter.Business_Layer_Classes.DataEncryptor.EncryptStringAES(password, "DBPassword");
            Settings.Default.DBLocation = dbLocation;
            Settings.Default.DBName = dbName;
            Settings.Default.DBUsername = userName;
            Settings.Default.DBPassword = password;
            Settings.Default.Save();
            UpdateConnectionString();

            LogToText.WriteToLog(
                $"Connection String Updated. dbLocation = {dbLocation} dbName = {dbName} userName = {userName} password = {password}");
            UtilityMethods.ShowMessageBox("Connection String Updated Successfully", "Success");
        }

        public static void updateReportServerUri(string reportServerUrl)
        {
            Settings.Default.DBReportServerUrl = reportServerUrl;
            Settings.Default.Save();
        }


        public static void LogImport(string excelPath, string importType, int rowCount)
        {
            var ef = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
            try { 
                var log = new Log {
                    LogDate = DateTime.Now,
                    ExcelPath = excelPath,
                    ImportType = importType,
                    NumberOfRowsImported = rowCount
                };
                ef.Logs.Add(log);
                ef.SaveChanges();
            }
            catch (Exception e)
            {
                LogToText.WriteToLog($"Failed to write to log in the database\n{e.Message}");
            }
        }


        public SqlDataAdapter GetNominalCodeAdapter()
        {
            string sql = "Select GLNo as NominalCode, GLDescription as Description from GlTypes order by GLNo";
            string sqlconn = ConnectionString;

            SqlConnection sqlConn = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlconn);

            //SelectCommand
            var sqlComm = new SqlCommand(sql, sqlConn);
            adapter.SelectCommand = sqlComm;
            //UpdateCommand
            sqlComm = new SqlCommand("Update GLTypes Set GLNo = @GlNo, GlDescription = @GlDescription where GlNo = @oldGlNo", sqlConn);
            sqlComm.Parameters.Add("@GlNo", SqlDbType.Int, 5, "NominalCode");
            sqlComm.Parameters.Add("@GlDescription", SqlDbType.NVarChar, 255, "Description");
            SqlParameter parameter = sqlComm.Parameters.Add("@oldGlNo", SqlDbType.Int, 5, "NominalCode");
            parameter.SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand = sqlComm;
            //DeleteCommand
            sqlComm = new SqlCommand("Delete from GLTypes where GlNo = @GlNo", sqlConn);
            parameter = sqlComm.Parameters.Add("GlNo", SqlDbType.Int, 5, "NominalCode");
            parameter.SourceVersion = DataRowVersion.Original;
            adapter.DeleteCommand = sqlComm;
            //InsertCommand
            sqlComm = new SqlCommand("Insert into GLTypes (GLNo, GlDescription) values (@GlNo, @GlDescription)", sqlConn);
            sqlComm.Parameters.Add("@GlNo", SqlDbType.Int, 5, "NominalCode");
            sqlComm.Parameters.Add("@GlDescription", SqlDbType.NVarChar, 255, "Description");
            adapter.InsertCommand = sqlComm;

            return adapter;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            // dispose managed resources
            DataAdapter.Dispose();
            TableBindingSource.Dispose();
            Table.Dispose();
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
