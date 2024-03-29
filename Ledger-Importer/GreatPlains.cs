﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SageImporterLibrary;
using Jonas_Sage_Importer.Properties;

namespace Jonas_Sage_Importer {
    class GreatPlains {
        private const string ImpName = "GREAT PLAINS";
        private static readonly string ConnectionString = DbConnectionsCs.ConnectionString;


        public static void ImportInvoices(ComboBox comboBox, bool removeNewer, DateTime removeNewerDt, DataTable dTable) {

            if (dTable.Rows.Count == 0) {
                UtilityMethods.ShowMessageBox("Please select an Excel sheet first so that there is information in the table.");
                return;
            }

            if (removeNewer) {
                DeleteHistoricalCheck(comboBox, true, removeNewerDt);
            }

            string gridProcedureName = String.Empty;
            string tempProcedureName = String.Empty;

            switch (comboBox.SelectedIndex) {
                case -1:
                UtilityMethods.ShowMessageBox("Please select an Import Type");
                return;
                case 0:
                gridProcedureName = "GP_Grid_ImportInvoices";
                tempProcedureName = "GP_Temp_ImportInvoices";
                break;
                case 1:
                gridProcedureName = "GP_Grid_ImportPostedInvoices";
                tempProcedureName = "GP_Temp_ImportPostedInvoices";
                break;
                case 2:
                DeleteHistoricalLedger(comboBox.SelectedIndex, new DateTime(1900, 01, 01));
                gridProcedureName = "GP_Grid_ImportOutstandingInvoices";
                tempProcedureName = "GP_Temp_ImportOutstandingInvoices";
                break;
            }
            try {
                RadForm1._radForm1.UpdateStripText($"Attempting to Import {comboBox.Text} from Application.");
                ImportInvoices(gridProcedureName, dTable);
            }
            catch (Exception exception) {
                RadForm1._radForm1.UpdateStripText($"Failed to Import {comboBox.Text} from Application.");
                UtilityMethods.ShowMessageBox($"Failed to import {comboBox.Text} from Application.\n\n{exception.Message}", "Failed");
                return;
            }
            try {
                RadForm1._radForm1.UpdateStripText($"Attempting to Import {comboBox.Text} from Temporary Table.");
                CommitImport(tempProcedureName);
                RadForm1._radForm1.UpdateStripText($"Successfully imported {comboBox.Text} from Temporary Table.");
            }
            catch (Exception exception) {
                RadForm1._radForm1.UpdateStripText($"Failed to Import {comboBox.Text} from Temporary Table.");
                UtilityMethods.ShowMessageBox(
                   $"Failed to import {comboBox.Text} from Temporary Table.\n\n{exception.Message}",
                   "Failed");
            }
        }

        private static void DeleteHistoricalCheck(ComboBox comboBox, bool removeNewer, DateTime removeNewerDt) {
            if (removeNewer) {
                DialogResult dialogResult =
                     UtilityMethods.ShowMessageBox(
                        $"Are you sure you would like to delete {comboBox.SelectedText} newer than {removeNewerDt} (inclusive)?\n\nYou will not be able to recover this information.",
                        "Confirm Delete?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes) {
                }
                else {
                    DeleteHistoricalLedger(comboBox.SelectedIndex, removeNewerDt);
                }
            }
        }

        private static void ImportInvoices(string command, DataTable tbl) {
            using (SqlConnection sqconn = new SqlConnection(ConnectionString)) {
                using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                    LogToText.WriteToLog($"{ImpName}: Attempting to import to temporary table in the database");
                    sqcomm.Connection = sqconn;
                    sqcomm.CommandType = CommandType.StoredProcedure;
                    sqcomm.Parameters.AddWithValue("@tblLedger", tbl);
                    sqcomm.CommandText = command;
                    sqconn.Open();

                    // statusStripBar.Text = "Attempting to import to temporary table.";
                    sqcomm.ExecuteNonQuery();
                }

                string tempSuccess =
                    $"{ImpName}:  Successfully imported to temporary table in the {Settings.Default.DBName} database";
                LogToText.WriteToLog(tempSuccess);

                // statusStripBar.Text = tempSuccess;
            }
        }

        private static void CommitImport(string command) {
            try {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString)) {
                    using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                        LogToText.WriteToLog($"{ImpName}: Attempting to commit new data to database.");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = command;
                        sqconn.Open();

                        // statusStripBar.Text = "Attempting to commit to Database.";
                        sqcomm.ExecuteNonQuery();
                    }
                }

                string commitSuccess = $"{ImpName}: Successfully committed new data to the {Settings.Default.DBName} database";
                LogToText.WriteToLog(commitSuccess);
            }
            catch (Exception ex) {
                string commitFailure = $"{ImpName}: Error committing data to the database: \n{ex.Message}";
                LogToText.WriteToLog(commitFailure);
                UtilityMethods.ShowMessageBox(commitFailure, "Failed");
            }
        }

        private static void DeleteHistoricalLedger(int commandType, DateTime removeDateTime) {
            string command = string.Empty;
            string tableName = string.Empty;

            if (commandType == 0) {
                tableName = "SaleLedger";
                command = $"Delete from {tableName} where Date >= @removeDateTime and ([Type] = 'Invoice' or [Type] = 'Return') and ImportType = 'Great Plains'";
            }
            else if (commandType == 1) {
                tableName = "PostedInvoices";
                command = $"Delete from {tableName} where TrxDate >= @removeDateTime";
            }
            else if (commandType == 2) {
                tableName = "OutstandingInvoices";
                command = $"Delete from {tableName} where Date >= @removeDateTime";
            }

            using (SqlConnection sqconn = new SqlConnection(ConnectionString)) {
                using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                    LogToText.WriteToLog($"{ImpName}: Attempting to delete {tableName} newer than {removeDateTime}.");
                    sqcomm.Connection = sqconn;
                    sqcomm.CommandType = CommandType.Text;
                    sqcomm.CommandText = command;
                    sqcomm.Parameters.Add(new SqlParameter("@removeDateTime", removeDateTime));
                    sqconn.Open();
                    sqcomm.ExecuteNonQuery();
                }
            }
        }





    }
}
