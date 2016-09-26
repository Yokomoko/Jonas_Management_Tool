using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SageImporterLibrary

{
    public class DbConnectionsCs : IDisposable {
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();

        private LogToText logToText = new LogToText();


        private static string _dbLocationTxt = "192.168.15.48";


        private static string _dbNameTxt = "Purchase_SaleLedger";

        private static string _usernameTxt = "Sage";

        private static string _passwordTxt = "SageImport";

        private static string _reportServerUrl = @"http://192.168.15.48/reportserver";

        public static string DbLocationTxt()
        {
            return _dbLocationTxt;
        }


        public static string DbNameTxt()
        {
            return _dbNameTxt;
        }

        public static string UserNameTxt()
        {
            return _usernameTxt;
        }

        public static string PasswordTxt()
        {
            return _passwordTxt;
        }

        public static string ReportServerUrl()
        {
            return _reportServerUrl;
        }

        public static string ConnectionString()
        {
            return
                $"Persist Security Info=False;Integrated Security=False;Initial Catalog={DbNameTxt()};Data Source={DbLocationTxt()};User ID={UserNameTxt()};Password={PasswordTxt()};";
        }

        internal OleDbDataAdapter DataAdapter = new OleDbDataAdapter();

        internal BindingSource TableBindingSource = new BindingSource();

        internal DataTable Table = new DataTable();



        public static void TestConnection(
            string dbLocation,
            string dbName,
            string userName,
            string password,
            Label connLabel)
        {
            string testConString =
                $"Persist Security Info=False;Integrated Security=false;Initial Catalog={dbName};Data Source={dbLocation};User ID={userName};Password={password}";
            LogToText.WriteToLog($"Testing connection - {testConString}");
            try
            {
                using (SqlConnection conn = new SqlConnection(testConString))
                {
                    conn.Open();
                    connLabel.Text = @"Success";
                    LogToText.WriteToLog($"Connection OK with the connection string - '{testConString}'");
                    connLabel.ForeColor = Color.Green;
                    connLabel.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            catch (SqlException ex)
            {
                LogToText.WriteToLog($"Connection Failed with the connection string - '{testConString}'");
                //MessageBox.Show($"Connection Failed \n {ex.Message}");
                MessageBox.Show(new Form { TopMost = true }, $"Connection Failed \n {ex.Message}");
                connLabel.Text = @"Failed..";
                connLabel.ForeColor = Color.Red;
                connLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        public static void UpdateConnection(string dbLocation, string dbName, string userName, string password)
        {
            bool counter = false;

            while (counter == false)
            {
                _dbLocationTxt = dbLocation;
                _dbNameTxt = dbName;
                _usernameTxt = userName;
                _passwordTxt = password;
                LogToText.WriteToLog(
                    $"Connection String Updated. dbLocation = {dbLocation} dbName = {dbName} userName = {userName} password = {password}");
                MessageBox.Show(@"Connection String Updated Successfully", @"Success");
                counter = true;
            }
        }

        public static void updateReportServerUri(string reportServerUrl)
            {
                _reportServerUrl = reportServerUrl;
            }


        public SqlDataAdapter GetData(string selectCommand)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
               dataAdapter = new SqlDataAdapter(selectCommand, conn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable();
                //BindingSource bSource = new BindingSource();

                //dataAdapter.Fill(table);
                //bSource.DataSource = table;

                return dataAdapter;
            }
        }

        public static void LogImport(string excelPath, string importType, int rowCount)
        {
            string sqlQuery =
                "Insert into log (LogDate, ExcelPath, ImportType, NumberOfRowsImported) Values (GetDate(), @ExcelPath, @ImportType, @RowCount)";
            try
            {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString()))
                {
                    using (SqlCommand sqcomm = new SqlCommand(sqlQuery, sqconn))
                    {
                        LogToText.WriteToLog("Logging information to the log table.");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.Text;
                        sqcomm.CommandText = sqlQuery;
                        sqcomm.Parameters.Add(new SqlParameter("@ExcelPath", excelPath));
                        sqcomm.Parameters.Add(new SqlParameter("@ImportType", importType));
                        sqcomm.Parameters.Add(new SqlParameter("@RowCount", rowCount));
                        sqconn.Open();
                        sqcomm.ExecuteNonQuery();
                    }
                    LogToText.WriteToLog("Successfully logged information to the log table.");
                }
            }
            catch (Exception e)
            {
                LogToText.WriteToLog($"Failed to write to log in the database\n{e.Message}");
                throw;
            }
        }


        public SqlDataAdapter GetNominalCodeAdapter()
        {
            string sql = "Select GLNo as NominalCode, GLDescription as Description from GlTypes order by GLNo";
            string sqlconn = ConnectionString();

            SqlCommand sqlComm;
            SqlConnection sqlConn = new SqlConnection(ConnectionString());
            

            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlconn);



            //SelectCommand
            sqlComm = new SqlCommand(sql,sqlConn);
            adapter.SelectCommand = sqlComm;
            //UpdateCommand
            sqlComm = new SqlCommand("Update GLTypes Set GLNo = @GlNo, GlDescription = @GlDescription where GlNo = @oldGlNo",sqlConn);
            sqlComm.Parameters.Add("@GlNo", SqlDbType.Int, 5, "NominalCode");
            sqlComm.Parameters.Add("@GlDescription", SqlDbType.NVarChar, 255, "Description");
            SqlParameter parameter = sqlComm.Parameters.Add("@oldGlNo", SqlDbType.Int, 5, "NominalCode");
            parameter.SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand = sqlComm;
            //DeleteCommand
            sqlComm = new SqlCommand("Delete from GLTypes where GlNo = @GlNo",sqlConn);
            parameter = sqlComm.Parameters.Add("GlNo", SqlDbType.Int, 5, "NominalCode");
            parameter.SourceVersion = DataRowVersion.Original;
            adapter.DeleteCommand = sqlComm;
            //InsertCommand
            sqlComm = new SqlCommand("Insert into GLTypes (GLNo, GlDescription) values (@GlNo, @GlDescription)",sqlConn);
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
