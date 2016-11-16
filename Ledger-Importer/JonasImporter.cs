using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SageImporterLibrary;
using Telerik.WinControls.UI;
using BL_JonasSageImporter;
using Jonas_Sage_Importer.Properties;

namespace Jonas_Sage_Importer {
    class JonasImporterEnums {
        #region Public Enums

        //All Enums need a description
        public enum ConvertColumns {
            MiniPack = 0,
            DirectDebit = 1,
            TillType = 2,
            AdminStatus = 3,
            Status = 4
        }

        public enum ImportSources {
            [Description("Great Plains")]
            Great_Plains = 1,
            [Description("OpenCRM")]
            OpenCrm = 2
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
            COGS_Report = 1,
            [Description("Sales Orders and COGS")]
            SO_COGS = 2
        }

        #endregion
    }

    internal class Jonas {

        private static readonly string ConnectionString = DbConnectionsCs.ConnectionString;
        private static readonly string DbName = Settings.Default.DBName;
        private static readonly Purchase_SaleLedgerEntities ef = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());

        [Obsolete("Do not use for additional code - Legacy GP only. Use Entity Framework and consider updating existing methods using this class")]
        public static void ImportGpToTemporaryTable(string command, DataTable tbl, string ImpName) {
            try {
                using (SqlConnection sqconnother = new SqlConnection(ConnectionString)) {
                    using (SqlCommand sqcomm = new SqlCommand(command, sqconnother)) {
                        sqcomm.Connection = sqconnother;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.Parameters.AddWithValue("@tblLedger", tbl);
                        sqcomm.CommandText = command;
                        sqconnother.Open();
                        //statusStripBar.Text = "Attempting to import to temporary table.";
                        sqcomm.ExecuteNonQuery();
                    }
                    string tempSuccess =
                        $"{ImpName}: Successfully imported to temporary table in the {DbName} database";
                    LogToText.WriteToLog(tempSuccess);
                }
            }
            catch (Exception ex) {
                UtilityMethods.ShowMessageBox($"Failed to Import to Temporary Database.\n\n" + ex.Message);
            }
        }

        [Obsolete("Do not use for additional code - Legacy GP only. Use Entity Framework and consider updating existing methods using this class")]
        public static void ImportGpToMainTable(string command, string impName) {
            try {
                using (SqlConnection sqconn = new SqlConnection(ConnectionString)) {
                    using (SqlCommand sqcomm = new SqlCommand(command, sqconn)) {
                        LogToText.WriteToLog($"{impName}: Attempting to commit new data to database.");
                        sqcomm.Connection = sqconn;
                        sqcomm.CommandType = CommandType.StoredProcedure;
                        sqcomm.CommandText = command;
                        sqconn.Open();
                        sqcomm.ExecuteNonQuery();
                    }
                }
                string commitSuccess = $"{impName}: Successfully committed new data to the {DbName} database";
                LogToText.WriteToLog(commitSuccess);
            }
            catch (Exception ex) {
                string commitFailure = $"{impName}: Error committing data to the database:\n\n{ex.Message}";
                LogToText.WriteToLog(commitFailure);
                UtilityMethods.ShowMessageBox(commitFailure, "Failed");
            }
        }


        private static void CrmImportOrders(DataTable tbl) {
            try {
                ef.Database.ExecuteSqlCommand(
                    "Delete from SaleLedger where [Type] like '%OpenCRM%' and ImportType like '%OpenCRM Sales Order%'");
                foreach (DataRow dr in tbl.Rows) {
                    var salesLedger = new SaleLedger {
                        Date = DateTime.Parse(dr[0].ToString()),
                        CustName = dr[1].ToString(),
                        SiteName = dr[2].ToString(),
                        CustRef = dr[3].ToString(),
                        DueDate = DateTime.Parse(dr[4].ToString()),
                        Category = dr[5].ToString(),
                        ItemDescription = dr[6].ToString(),
                        Qty = decimal.Parse(dr[7].ToString()),
                        Net = decimal.Parse(dr[8].ToString()),
                        Tax = decimal.Parse(dr[9].ToString()),
                        Gross = decimal.Parse(dr[10].ToString()),
                        Profit = decimal.Parse(dr[11].ToString()),
                        Type = "OpenCRM",
                        Currency = dr[12].ToString(),
                        CustOrderNo = dr[13].ToString(),
                        ImportType = "OpenCRM Sales Order",
                        MiniPack = tbl.Columns.Count >= 14 ? (short?)ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.MiniPack, dr[14].ToString()) : (short?)null,
                        SiteSurveyDate = dr[15]?.ToString(),
                        BacklogComments = dr[16]?.ToString(),
                        Deposit = dr[17]?.ToString(),
                        AssignedTo = dr[18]?.ToString(),
                        MegJobNo = dr[19]?.ToString(),
                        DirectDebit = tbl.Columns.Count >= 14 ? short.Parse(ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.DirectDebit, dr[20].ToString()).ToString()) : (short?)null,
                        Spare1 = tbl.Columns.Count >= 21 ? ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.TillType, dr[21].ToString()).ToString() : null,
                        Spare2 = tbl.Columns.Count >= 22 ? ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.AdminStatus, dr[22].ToString()).ToString() : null
                    };
                    ef.SaleLedgers.Add(salesLedger);
                }
                ef.SaveChanges();
                LogToText.WriteToLog($"OpenCRM Sales Order: Successfully committed new data to the {DbName} database");
            }
            catch (Exception ex) {
                LogToText.WriteToLog($"Error Importing Orders To Database. \n\n {ex.Message}");
                UtilityMethods.ShowMessageBox($"Error Importing Orders To Database. \n\n {ex.Message}");
            }
        }

        private static void CrmImportCogs(DataTable tbl) {
            ef.Database.ExecuteSqlCommand("Delete [CostOfGoodsSold]");
            foreach (DataRow row in tbl.Rows) {
                try {
                    row[2] = ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.AdminStatus, row[2].ToString().ToUpper()).ToString();
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
                    string commitSuccess = $"OpenCRM Cogs: Successfully committed new data to the {DbName} database";
                    LogToText.WriteToLog(commitSuccess);
                }
                catch (Exception ex) {
                    UtilityMethods.ShowMessageBox(
                        "Error importing Cogs {Environment.NewLine} {Environment.NewLine} {ex.Message}",
                        "Error Importing COGS");
                    UtilityMethods.ShowMessageBox($"Error importing Cogs {Environment.NewLine} {Environment.NewLine} {ex.Message}", "Error Importing COGS");
                    string commitFailure = $"OpenCRM Cogs: Error committing data to the database: \n\n{ex.Message}";
                    LogToText.WriteToLog(commitFailure);
                    return;
                }
            }
            ef.SaveChanges();
        }

        private static void CrmImportOrderCogs(DataTable tbl) {
            var lId = 1;
            try {
                ef.Database.ExecuteSqlCommand("Delete [CostOfGoodsSold]");
                ef.Database.ExecuteSqlCommand(
                    @"Delete from Purchase_SaleLedger.dbo.SaleLedger where [Type] = 'OpenCRM' and ImportType = 'OpenCRM Sales Order'");

                foreach (DataRow dr in tbl.Rows) {
                    var cog = new CostOfGoodsSold() {
                        CogsCompanyName = dr[1].ToString(),
                        CogsSiteName = dr[2].ToString(),
                        CogsGPCode = dr[3].ToString(),
                        CogsDueDate = DateTime.Parse(dr[4].ToString().Trim()),
                        CogsGPCategory = dr[5].ToString(),
                        CogsDescription = dr[6].ToString(),
                        CogsItemQuantity = decimal.Parse(dr[7].ToString()),
                        CogsItemListPrice = decimal.Parse(dr[8].ToString()),
                        CogsSalesOrderId = int.Parse(dr[13].ToString()),
                        CogsItemBuyPrice = decimal.Parse(dr[24].ToString()),
                        // ReSharper disable once PossibleInvalidOperationException - Never evaulates to null
                        CogsStatus = (int)ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.AdminStatus, dr[22].ToString().Trim()),
                        CogsLedgerId = lId
                    };
                    var sLedger = new SaleLedger() {
                        Date = DateTime.Parse(dr[0].ToString()),
                        CustRef = dr[4].ToString(),
                        CustName = dr[1].ToString(),
                        SiteName = dr[2].ToString(),
                        DueDate = DateTime.Parse(dr[4].ToString()),
                        Category = dr[5].ToString(),
                        ItemDescription = dr[6].ToString(),
                        Qty = decimal.Parse(dr[7].ToString()),
                        Net = decimal.Parse(dr[8].ToString()),
                        Tax = decimal.Parse(dr[9].ToString()),
                        Gross = decimal.Parse(dr[10].ToString()),
                        Profit = decimal.Parse(dr[11].ToString()),
                        Type = "OpenCRM",
                        Currency = "£",
                        CustOrderNo = dr[13].ToString(),
                        ImportType = "OpenCRM Sales Order",
                        SaleLedgerLedgerId = lId,
                        MiniPack = (short?)ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.MiniPack, dr[14].ToString()),
                        SiteSurveyDate = dr[15].ToString(),
                        BacklogComments = dr[16].ToString(),
                        Deposit = dr[17].ToString(),
                        AssignedTo = dr[18].ToString(),
                        MegJobNo = dr[19].ToString(),
                        DirectDebit = (short?)ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.DirectDebit, dr[20].ToString()),
                        Spare1 = ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.TillType, dr[21].ToString()).ToString(),
                        Spare2 = ConvertImportTextToInt(JonasImporterEnums.ConvertColumns.AdminStatus, dr[22].ToString()).ToString()
                    };
                    ef.SaleLedgers.Add(sLedger);
                    ef.CostOfGoodsSolds.Add(cog);
                    lId = lId + 1;
                }
                ef.SaveChanges();
                LogToText.WriteToLog($"OpenCRM Combined Sales Orders & Cogs: Successfully committed new data to the {DbName} database");
            }
            catch (Exception ex) {
                LogToText.WriteToLog($"Error committing to the Database. \n\n {ex.Message}");
                UtilityMethods.ShowMessageBox("Error committing to the database \n\n" + ex.Message);
            }
        }

        public static void ImportInvoices(string command, DataTable tbl, string ImpName) {
            switch (command) {
                case "CRM_Grid_ImportOrders":
                CrmImportOrders(tbl);
                break;
                case "CRM_ImportCogs": {
                    CrmImportCogs(tbl);
                }
                break;
                case "SO_COGS":
                CrmImportOrderCogs(tbl);
                break;
            }
        }


        private static int? ConvertImportTextToInt(JonasImporterEnums.ConvertColumns convertColumns, string input) {
            int? output = null;

            switch (convertColumns) {
                #region MiniPack and DirectDebit (14 & 20)
                case JonasImporterEnums.ConvertColumns.MiniPack:
                case JonasImporterEnums.ConvertColumns.DirectDebit:
                input = input.Trim().ToLower().Replace(" ", "");

                //MiniPack or DirectDebit Columns
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
                break;
                #endregion
                #region TillType
                case JonasImporterEnums.ConvertColumns.TillType:
                switch (input) {
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
                break;
                #endregion
                #region Status
                case JonasImporterEnums.ConvertColumns.Status:
                switch (input) {
                    case "created":
                    output = 1;
                    break;
                    case "approved":
                    output = 2;
                    break;
                    case "sent":
                    output = 3;
                    break;
                    case "esigned":
                    output = 4;
                    break;
                    case "cancelled":
                    output = 5;
                    break;
                    case "pendingcancelled":
                    output = 6;
                    break;
                    case "pendinginvoice":
                    output = 7;
                    break;
                    case "completed":
                    output = 8;
                    break;
                    case "installed":
                    output = 9;
                    break;
                    case "sage":
                    output = 10;
                    break;
                    case "stuck":
                    output = 11;
                    break;
                    case "invoiced":
                    output = 12;
                    break;
                    case "recurring":
                    output = 13;
                    break;
                    default:
                    output = -1;
                    break;
                }
                break;
                #endregion
                #region Admin Status (22)
                case JonasImporterEnums.ConvertColumns.AdminStatus: {
                    if (input == "created" || input == "0") {
                        output = 0;
                    }
                    else if (input == "pending" || input == "1") {
                        output = 1;
                    }
                    else if (input == "approved" || input == "2") {
                        output = 2;
                    }
                    else if (input == "pendingcancelled" || input == "pendingcancel" || input == "pendingcancellation" || input == "3") {
                        output = 3;
                    }
                    else if (input == "pendinginvoice" || input == "pendinginvoiced" || input == "4") {
                        output = 4;
                    }
                    else if (input == "invoiced" || input == "5") {
                        output = 5;
                    }
                    else if (input == "pendingapproved" || input == "pendingapproval" || input == "6") {
                        output = 6;
                    }
                    else if (input == "stuck" || input == "7") {
                        output = 7;
                    }
                    else {
                        output = -1;
                    }
                    break;
                }

                #endregion
            }
            return output;
        }


        public static void DeleteHistoricalCheck(RadDropDownList sourceComboBox, RadDropDownList typeComboBox, bool removeNewer, DateTime removeNewerDt) {
            if (sourceComboBox.SelectedIndex == 1) {
                if (removeNewer) {
                    DialogResult dialogResult =
                         UtilityMethods.ShowMessageBox(
                            $"Are you sure you would like to delete {typeComboBox.SelectedText} newer than {removeNewerDt} (inclusive)?\n\nYou will not be able to recover this information.",
                            "Confirm Delete?",
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
            var command = string.Empty;
            var tableName = string.Empty;

            switch (commandType) {
                case 0:
                tableName = "SaleLedger";
                command = $"Delete from {tableName} where Date >= @removeDateTime and ([Type] = 'Invoice' or [Type] = 'Return') and ImportType = 'Great Plains'";
                break;
                case 1:
                tableName = "PostedInvoices";
                command = $"Delete from {tableName} where TrxDate >= @removeDateTime";
                break;
                case 2:
                tableName = "OutstandingInvoices";
                command = $"Delete from {tableName} where Date >= @removeDateTime";
                break;
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
