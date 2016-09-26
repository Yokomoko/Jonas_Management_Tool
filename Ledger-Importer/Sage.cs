using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SageImporterLibrary;

namespace Jonas_Sage_Importer
{
    

    class Sage
    {
        private static String ImpName = "SAGE";
        /// <summary>
        /// The connection string from the DbConnectionsCs class in order to perform the imports.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ConnectionString()
        {
            return DbConnectionsCs.ConnectionString();
        }

        /// <summary>
        /// The DbName from DbConnectionsCs class in order to reference what the database name it is importing into is.
        /// </summary>
        /// <returns></returns>
        private static string DbNameTxt()
        {
            return DbConnectionsCs.DbNameTxt();
        }



        public static void ImportInvoices(string command, DataTable tbl)
        {
            using (SqlConnection sqconn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand sqcomm = new SqlCommand(command, sqconn))
                {
                    LogToText.WriteToLog($"{ImpName}: Attempting to import to temporary table in the database");
                    sqcomm.Connection = sqconn;
                    sqcomm.CommandType = CommandType.StoredProcedure;
                    sqcomm.Parameters.AddWithValue("@tblLedger", tbl);
                    sqcomm.CommandText = command;
                    sqconn.Open();
                    //statusStripBar.Text = "Attempting to import to temporary table.";
                    sqcomm.ExecuteNonQuery();
                }
                string tempSuccess = $"{ImpName}: Successfully imported to temporary table in the {DbNameTxt()} database";
                LogToText.WriteToLog(tempSuccess);
                // statusStripBar.Text = tempSuccess;
            }
        }

        public static void CommitImport(string command)
        {
            try
            {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString()))
                {
                    using (SqlCommand sqcomm = new SqlCommand(command, sqconn))
                    {
                        LogToText.WriteToLog($"{ImpName}: Attempting to commit new data to database.");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = command;
                        sqconn.Open();
                        // statusStripBar.Text = "Attempting to commit to Database.";
                        sqcomm.ExecuteNonQuery();
                    }
                }
                string commitSuccess = $"{ImpName}: Successfully committed new data to the {DbNameTxt()} database";
                LogToText.WriteToLog(commitSuccess);
            }
            catch (Exception ex)
            {
                string commitFailure = $"{ImpName}: Error committing data to the database: \n{ex.Message}";
                LogToText.WriteToLog(commitFailure);
                MessageBox.Show(commitFailure, "Failed");
            }
        }

        public static void DeletePreviousOrders()
        {

            //if (!successCounter) return;
            DialogResult dialogResult = MessageBox.Show(
                "Would you like to remove all previously entered sales orders?", "Sales Orders", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
            {
                LogToText.WriteToLog($"{ImpName}: Previously entered sales orders were not deleted from the database.");
                return;
            }
            try
            {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString()))
                {
                    using (SqlCommand sqcomm = new SqlCommand("Sage_DeletePreviousOrders", sqconn))
                    {
                        LogToText.WriteToLog($"{ImpName}: Deleting Previous Sales Orders");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = "Sage_DeletePreviousOrders";
                        sqconn.Open();
                        //statusStripBar.Text = "Deleting Previous Orders from Database.";
                        sqcomm.ExecuteNonQuery();
                    }
                }
                string deleteSuccess = $"{ImpName}: Deleted previous orders from the {DbNameTxt()} database.";
                LogToText.WriteToLog(deleteSuccess);
            }
            catch (Exception ex)
            {
                string deleteFailure = $"{ImpName}: Error deleting previous orders from database: \n {ex.Message}";
                LogToText.WriteToLog(deleteFailure);
                MessageBox.Show(deleteFailure, "Failed");
            }
        }
    }
}
