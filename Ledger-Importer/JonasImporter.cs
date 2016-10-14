using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SageImporterLibrary;
using Telerik.WinControls.UI;
using System.Data.Sql;
using BL_JonasSageImporter;

namespace Jonas_Sage_Importer {
    class JonasImporterEnums {
        #region Public Enums

        //All Enums need a description

        public enum ImportSources {
            [Description("Great Plains")]
            Great_Plains = 1,
            [Description("OpenCRM")]
            OpenCrm = 2
        }

        public enum SageImportTypes {
            [Description("Invoice")]
            Invoice = 0,
            [Description("Sales Order")]
            Sales_Order = 1
        }

        public enum GreatPlainsImportTypes {
            [Description("Invoice (EPOS AR)")]
            Invoice_EposAR = 0,
            [Description("Invoices Posted to P+L (CSS DOWNLOAD)")]
            Invoices_Posted_to_P_L = 1,
            [Description("Outstanding Invoices")]
            Outstanding_Invoices = 2
        }

        public enum CrmImportTypes {
            [Description("Sales Order")]
            Sales_Order = 0,
            [Description("COGS Report")]
            COGS_Report = 1
        }

        #endregion
    }

    class Jonas {
        private static string ConnectionString() {
            return DbConnectionsCs.ConnectionString();
        }

        /// <summary>
        /// The DbName from DbConnectionsCs class in order to reference what the database name it is importing into is.
        /// </summary>
        /// <returns></returns>
        private static string DbNameTxt() {
            return DbConnectionsCs.DbNameTxt();
        }

        public static void ImportInvoices(string command, DataTable tbl, string ImpName) {

            var comm = command;

            if (command == "CRM_Grid_ImportOrders") {
                var ef = new Purchase_SaleLedgerEntities("Purchase_SaleLedgerEntities_Live");
                ef.Database.ExecuteSqlCommand(
                    "Delete from SaleLedger where [Type] like '%OpenCRM%' and ImportType like '%OpenCRM Sales Order%'");
                foreach (DataRow dr in tbl.Rows) {
                    using (SqlConnection sqconn = new SqlConnection(ConnectionString())) {
                        using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                            LogToText.WriteToLog(
                                $"{ImpName}: Attempting to import row {tbl.Rows.IndexOf(dr)} to temporary table in the database");

                            const int minicol = 14;
                            const int ddcol = 20;
                            const int tillTypeCol = 21;
                            const int adminStatusCol = 22;

                            if (tbl.Columns.Count >= 21) {
                                //MiniPack coll from text to int
                                dr[minicol] = ConvertImportTextToInt(minicol, dr[ddcol].ToString().Trim().ToLower());
                                //DirectDebit from text to int
                                dr[ddcol] = ConvertImportTextToInt(ddcol, dr[ddcol].ToString().Trim().ToLower());
                            }

                            //Till Type from text to int
                            if (tbl.Columns.Count >= 22) {
                                dr[tillTypeCol] = ConvertImportTextToInt(tillTypeCol,
                                    dr[tillTypeCol].ToString().Trim().ToLower());
                            }
                            //Admin Status from text to int
                            if (tbl.Columns.Count >= 23) {
                                dr[adminStatusCol] = ConvertImportTextToInt(adminStatusCol,
                                    dr[adminStatusCol].ToString().Trim().ToLower());
                            }
                            //start importing
                            sqcomm.CommandType = CommandType.StoredProcedure;
                            sqcomm.Parameters.AddWithValue("@Date", dr[0]);
                            sqcomm.Parameters.AddWithValue("@CustName", dr[1]);
                            sqcomm.Parameters.AddWithValue("@SiteName", dr[2]);
                            sqcomm.Parameters.AddWithValue("@CustRef", dr[3]);
                            sqcomm.Parameters.AddWithValue("@DueDate", dr[4]);
                            sqcomm.Parameters.AddWithValue("@Category", dr[5]);
                            sqcomm.Parameters.AddWithValue("@ItemDescription", dr[6]);
                            sqcomm.Parameters.AddWithValue("@Qty", dr[7]);
                            sqcomm.Parameters.AddWithValue("@Net", dr[8]);
                            sqcomm.Parameters.AddWithValue("@Tax", dr[9]);
                            sqcomm.Parameters.AddWithValue("@Gross", dr[10]);
                            sqcomm.Parameters.AddWithValue("@Profit", dr[11]);
                            sqcomm.Parameters.AddWithValue("@Currency", dr[12]);
                            sqcomm.Parameters.AddWithValue("@CustOrderNo", dr[13]);
                            if (tbl.Columns.Count >= 21) {
                                comm = "CRM_Grid_ImportOrders_Adv";
                                sqcomm.Parameters.AddWithValue("@MiniPack", dr[14]);
                                sqcomm.Parameters.AddWithValue("@SiteSurveyDate", dr[15]);
                                sqcomm.Parameters.AddWithValue("@BacklogComments", dr[16]);
                                sqcomm.Parameters.AddWithValue("@Deposit", dr[17]);
                                sqcomm.Parameters.AddWithValue("@AssignedTo", dr[18]);
                                sqcomm.Parameters.AddWithValue("@MegJobNo", dr[19]);
                                sqcomm.Parameters.AddWithValue("@DirectDebit", dr[20]);
                            }
                            if (tbl.Columns.Count == 22) {
                                comm = "CRM_Grid_ImportOrders_Adv2";
                                sqcomm.Parameters.AddWithValue("@Spare1", dr[21]);
                            }
                            if (tbl.Columns.Count == 23) {
                                comm = "CRM_Grid_ImportOrders_Adv3";
                                sqcomm.Parameters.AddWithValue("@Spare1", dr[21]); //Till Type
                                sqcomm.Parameters.AddWithValue("@Spare2", dr[22]); //Admin Status
                            }

                            sqcomm.CommandText = comm;
                             sqconn.Open();
                            sqcomm.ExecuteNonQuery();
                        }
                    }//select * from saleledgerextended where entrytype like '%OpenCRM%' and Importtype like '%OpenCRM Sales Order%' order by Custref
                }
            }
            else if (command == "CRM_ImportCogs") {
                var ef = new Purchase_SaleLedgerEntities("Purchase_SaleLedgerEntities_Live");
                ef.Database.ExecuteSqlCommand("Delete [CostOfGoodsSold]");
                foreach (DataRow row in tbl.Rows) {
                    try {
                        #region Case To Alter Statuses to Int Values
                        switch (row[2].ToString().ToUpper()) {
                            case "CREATED":
                                row[2] = 1.ToString();
                                break;
                            case "APPROVED":
                                row[2] = 2.ToString();
                                break;
                            case "SENT":
                                row[2] = 3.ToString();
                                break;
                            case "ESIGNED":
                                row[2] = 4.ToString();
                                break;
                            case "CANCELLED":
                                row[2] = 5.ToString();
                                break;
                            case "PENDING CANCELLED":
                                row[2] = 6.ToString();
                                break;
                            case "PENDING INVOICE":
                                row[2] = 7.ToString();
                                break;
                            case "COMPLETED":
                                row[2] = 8.ToString();
                                break;
                            case "INSTALLED":
                                row[2] = 9.ToString();
                                break;
                            case "SAGE":
                                row[2] = 10.ToString();
                                break;
                            case "STUCK":
                                row[2] = 11.ToString();
                                break;
                            case "INVOICED":
                                row[2] = 12.ToString();
                                break;
                            case "RECURRING":
                                row[2] = 13.ToString();
                                break;
                            default:
                                row[2] = "-1";
                                break;
                        }
                        #endregion

                        var cog = new CostOfGoodsSold() {
                            CogsCompanyName = row[0].ToString(),
                            CogsSiteName = row[1].ToString(),
                            CogsStatus = int.Parse(row[2].ToString()),
                            CogsGPCode = row[3].ToString(),
                            CogsDueDate = DateTime.Parse(row[4].ToString()),
                            CogsGPCategory = row[5].ToString(),
                            CogsDescription = row[6].ToString(),
                            CogsSalesOrderId = int.Parse(row[7].ToString()),
                            CogsItemQuantity = decimal.Parse(row[8].ToString()),
                            CogsItemListPrice = decimal.Parse(row[9].ToString()),
                            CogsItemBuyPrice = decimal.Parse(row[10].ToString())
                        };
                        ef.CostOfGoodsSolds.Add(cog);
                        ef.SaveChanges();
                        string commitSuccess = $"{ImpName}: Successfully committed new data to the {DbNameTxt()} database";
                        LogToText.WriteToLog(commitSuccess);
                    }
                    catch (Exception ex) {
                        UtilityMethods.ShowMessageBox(
                            "Error importing Cogs {Environment.NewLine} {Environment.NewLine} {ex.Message}",
                            "Error Importing COGS");
                        UtilityMethods.ShowMessageBox($"Error importing Cogs {Environment.NewLine} {Environment.NewLine} {ex.Message}", "Error Importing COGS");
                        string commitFailure = $"{ImpName}: Error committing data to the database: \n{ex.Message}";
                        LogToText.WriteToLog(commitFailure);
                        return;
                    }
                }
            }
            else {
                using (SqlConnection sqconnother = new SqlConnection(ConnectionString())) {
                    using (SqlCommand sqcomm = new SqlCommand(comm, sqconnother)) {
                        sqcomm.Connection = sqconnother;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.Parameters.AddWithValue("@tblLedger", tbl);
                        sqcomm.CommandText = command;
                        sqconnother.Open();
                        //statusStripBar.Text = "Attempting to import to temporary table.";
                        sqcomm.ExecuteNonQuery();
                    }
                    string tempSuccess =
                        $"{ImpName}: Successfully imported to temporary table in the {DbNameTxt()} database";
                    LogToText.WriteToLog(tempSuccess);
                }
            }
            //return returnMessage;
        }

        public static void CommitImport(string command, string impName) {
            try {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString())) {
                    using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                        LogToText.WriteToLog($"{impName}: Attempting to commit new data to database.");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = command;
                        sqconn.Open();
                        sqcomm.ExecuteNonQuery();
                    }
                }
                string commitSuccess = $"{impName}: Successfully committed new data to the {DbNameTxt()} database";
                LogToText.WriteToLog(commitSuccess);
            }
            catch (Exception ex) {
                string commitFailure = $"{impName}: Error committing data to the database: \n{ex.Message}";
                LogToText.WriteToLog(commitFailure);
                UtilityMethods.ShowMessageBox(commitFailure, "Failed");
            }
        }

        private static int? ConvertImportTextToInt(int columnNumber, string input) {
            int? output = null;

            #region MiniPack and DirectDebit (14 & 20)
            //MiniPack or DirectDebit Columns
            if (columnNumber == 14 || columnNumber == 20) {
                switch (input) {
                    case "-":
                    case "0":
                    case ".":
                    case "n/a":
                        output = 0;
                        break;
                    case "pending":
                    case "1":
                        output = 1;
                        break;
                    case "chasing":
                    case "2":
                        output = 2;
                        break;
                    case "yes":
                    case "3":
                        output = 3;
                        break;
                    case "no":
                    case "4":
                        output = 4;
                        break;
                    default:
                        output = -1;
                        break;
                }
            }
            #endregion
            #region Till Type (21)

            if (columnNumber == 21) {
                switch (input.Replace(" ", "")) {
                    case "quantum":
                    case "0":
                        output = 0;
                        break;
                    case "pixel":
                    case "1":
                        output = 1;
                        break;
                    case "absolute":
                    case "2":
                        output = 2;
                        break;
                    case "fashionmaster":
                    case "3":
                        output = 3;
                        break;
                    default:
                        output = -1;
                        break;
                }
            }

            #endregion
            #region Admin Status (22)

            if (columnNumber == 22) {
                switch (input) {
                    case "created":
                    case "0":
                        output = 0;
                        break;
                    case "pending":
                    case "1":
                        output = 1;
                        break;
                    case "approved":
                    case "2":
                        output = 2;
                        break;
                    case "pending cancelled":
                    case "pending cancel":
                    case "pending cancellation":
                    case "3":
                        output = 3;
                        break;
                    case "pending invoice":
                    case "pending invoiced":
                    case "4":
                        output = 4;
                        break;
                    case "invoiced":
                    case "5":
                        output = 5;
                        break;
                    case "pending approved":
                    case "pending approval":
                    case "6":
                        output = 6;
                        break;
                    case "stuck":
                    case "7":
                        output = 7;
                        break;
                    default:
                        output = -1;
                        break;
                }
            }

            #endregion 

            return output;
        }

        public static void DeletePreviousOrders(string ImpName) {
            var dialogResult = UtilityMethods.ShowMessageBox(
                @"Would you like to remove all previously entered sales orders?", @"Sales Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes) {
                LogToText.WriteToLog($"{ImpName}: Previously entered sales orders were not deleted from the database.");
                return;
            }
            try {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString())) {
                    using (SqlCommand sqcomm = new SqlCommand("Sage_DeletePreviousOrders", sqconn)) {
                        LogToText.WriteToLog($"{ImpName}: Deleting Previous Sales Orders");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = "Sage_DeletePreviousOrders";
                        sqconn.Open();
                        sqcomm.ExecuteNonQuery();
                    }
                }
                string deleteSuccess = $"{ImpName}: Deleted previous orders from the {DbNameTxt()} database.";
                LogToText.WriteToLog(deleteSuccess);
            }
            catch (Exception ex) {
                string deleteFailure = $"{ImpName}: Error deleting previous orders from database: \n {ex.Message}";
                LogToText.WriteToLog(deleteFailure);
                UtilityMethods.ShowMessageBox(deleteFailure, "Failed");
            }
        }

        public static void DeleteHistoricalCheck(RadDropDownList sourceComboBox, RadDropDownList typeComboBox, bool removeNewer, DateTime removeNewerDt) {
            if (sourceComboBox.SelectedIndex == 1) {
                if (removeNewer) {
                    DialogResult dialogResult =
                         UtilityMethods.ShowMessageBox(
                            $"Are you sure you would like to delete {typeComboBox.SelectedText} newer than {removeNewerDt} (inclusive)?\n\nYou will not be able to recover this information.",
                            @"Confirm Delete?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.Yes) {
                    }
                    else {
                        DeleteHistoricalLedger(typeComboBox.SelectedIndex, removeNewerDt, sourceComboBox.Text);
                    }
                }
            }
        }

        public static void DeleteHistoricalLedger(int commandType, DateTime removeDateTime, string ImpName) {
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

            try {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString())) {
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
            catch (Exception) {
                throw;
            }
        }
    }

}
