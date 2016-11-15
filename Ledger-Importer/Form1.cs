using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SageImporterLibrary;
using Telerik.WinControls.UI;
using AutoUpdaterDotNET;
using Jonas_Sage_Importer.Generate_Excel_Reports;
using Jonas_Sage_Importer.Properties;

namespace Jonas_Sage_Importer {
    public partial class Form1 : Form {
        protected OleDbDataAdapter DataAdapter = new OleDbDataAdapter();
        protected BindingSource TableBindingSource = new BindingSource();
        protected DataTable Table = new DataTable();

        public static Form1 _form1 = new Form1();

        public Form1() {
            InitializeComponent();
            _form1 = this;
            LoadImportSourceCmbo();
        }


        private void Form1Load(object sender, EventArgs e) {
            var screen = Screen.PrimaryScreen.WorkingArea;
            Top = (screen.Height / 2) - (Height / 2);
            Left = (screen.Width / 2) - (Width / 2);
            Text = Application.ProductName;
            uxRemoveNewerRecordsDt.Value = DateTime.Today;
            uxImportSourceCmbo.SelectedIndex = 1;
            StatusStripLabel.Text = @"OK";
            //TopMost = true;
            uxRemoveNewerRecordsDt.Enabled = uxRemoveNewerRecordsChk.Checked;
            if (CheckForUpdates(true)) {
                CloseApplication();
            }
            //Check if first time this has been run
            if (Settings.Default.FirstRun) {
                //     UtilityMethods.ShowMessageBox("New stuff here");
                PopReleaseNotes();
                Settings.Default.FirstRun = false;
                Settings.Default.Save();
            }

        }


        private void ImportFromGridView(RadDropDownList importSource) {
            if (importSource == null) throw new ArgumentNullException(nameof(importSource));
            var gridProcedureName = string.Empty;
            var tempProcedureName = string.Empty;

            #region GreatPlains
            if (importSource.SelectedIndex == 0) {
                if (uxExcelSheetViewerGv.Rows.Count == 0) {
                    UtilityMethods.ShowMessageBox(@"Please select an Excel sheet first so that there is information in the table.", "");
                    return;
                }

                if (uxRemoveNewerRecordsChk.Checked) {
                    Jonas.DeleteHistoricalCheck(importSource, uxImportTypeCmbo, true, uxRemoveNewerRecordsDt.Value);
                }



                switch (uxImportTypeCmbo.SelectedIndex) {
                    case -1:

                        UtilityMethods.ShowMessageBox(@"Please select an Import Type", "");
                        return;
                    case 0: //Sage
                        gridProcedureName = "GP_Grid_ImportInvoices";
                        tempProcedureName = "GP_Temp_ImportInvoices";
                        break;
                    case 1: //Great Pains
                        gridProcedureName = "GP_Grid_ImportPostedInvoices";
                        tempProcedureName = "GP_Temp_ImportPostedInvoices";
                        break;
                    case 2: //OpenCRM
                        Jonas.DeleteHistoricalLedger(uxImportTypeCmbo.SelectedIndex, new DateTime(1900, 01, 01), importSource.Text); ;
                        gridProcedureName = "GP_Grid_ImportOutstandingInvoices";
                        tempProcedureName = "GP_Temp_ImportOutstandingInvoices";
                        break;
                }
                try {
                    StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application.");
                    Jonas.ImportInvoices(gridProcedureName, Table, importSource.Text);
                }
                catch (Exception exception) {
                    StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                    UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message}", @"Failed");
                    return;
                }
                try {
                    StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Temporary Table.");
                    Jonas.CommitImport(tempProcedureName, importSource.Text);
                    StatusStripLabel.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Temporary Table.");
                }
                catch (Exception exception) {
                    StatusStripLabel.Text = $"Failed to Import {uxImportTypeCmbo.Text} from Temporary Table.";
                    UtilityMethods.ShowMessageBox(
                        $"Failed to import {uxImportTypeCmbo.Text} from Temporary Table.\n\n{exception.Message}",
                        @"Failed");
                    return;
                }

            }
            #endregion
            #region CRM

            if (importSource.SelectedIndex == 1) {
                if (uxImportTypeCmbo.SelectedIndex == 0) {
                    gridProcedureName = "CRM_Grid_ImportOrders";
                    switch (Table.Columns.Count) {
                        case 14:
                            tempProcedureName = "CRM_Temp_ImportOrders";
                            break;
                        case 21:
                            tempProcedureName = "CRM_Temp_ImportOrders_Adv";
                            break;
                        case 22:
                            tempProcedureName = "CRM_Temp_ImportOrders_Adv2";
                            break;
                        case 23:
                            tempProcedureName = "CRM_Temp_ImportOrders_Adv3";
                            break;
                        default:
                            UtilityMethods.ShowMessageBox(@"The number of columns should be either 14,21,22 or 23");
                            return;
                    }
                    try {
                        StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application");
                        Jonas.ImportInvoices(gridProcedureName, Table, importSource.Text);
                        StatusStripLabel.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Application");
                        if (gridProcedureName == "CRM_ImportCogs") {
                            return;
                        }
                    }
                    catch (SqlException sqlException) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");

                        StringBuilder errorMessages = new StringBuilder();
                        for (int i = 0; i < sqlException.Errors.Count; i++) {
                            errorMessages.Append("Index #" + i + "\n" +
                                "Message: " + sqlException.Errors[i].Message + "\n" +
                                "LineNumber: " + sqlException.Errors[i].LineNumber + "\n" +
                                "Source: " + sqlException.Errors[i].Source + "\n" +
                                "Procedure: " + sqlException.Errors[i].Procedure + "\n");
                        }
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}", @"Failed");
                        LogToText.WriteToLog($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}");
                        return;
                    }
                    catch (Exception exception) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message} \n \n {exception.InnerException}", @"Failed");
                        return;
                    }

                    try {
                        StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Temporary Table.");
                        Jonas.CommitImport(tempProcedureName, importSource.Text);
                        StatusStripLabel.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Temporary Table.");
                    }
                    catch (Exception exception) {
                        StatusStripLabel.Text = $"Failed to Import {uxImportTypeCmbo.Text} from Temporary Table.";
                        UtilityMethods.ShowMessageBox(
                            $"Failed to import {uxImportTypeCmbo.Text} from Temporary Table.\n\n{exception.Message}",
                            @"Failed");
                        return;
                    }
                }
                if (uxImportTypeCmbo.SelectedIndex == 1) {
                    gridProcedureName = "CRM_ImportCogs";

                    try {
                        StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application");
                        Jonas.ImportInvoices(gridProcedureName, Table, importSource.Text);
                        StatusStripLabel.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Application");
                        DbConnectionsCs.LogImport(uxExcelSheetTxt.Text, "OpenCRM " + uxImportTypeCmbo.Text, uxExcelSheetViewerGv.RowCount);
                        return;

                    }
                    catch (SqlException sqlException) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");

                        StringBuilder errorMessages = new StringBuilder();
                        for (int i = 0; i < sqlException.Errors.Count; i++) {
                            errorMessages.Append("Index #" + i + "\n" +
                                "Message: " + sqlException.Errors[i].Message + "\n" +
                                "LineNumber: " + sqlException.Errors[i].LineNumber + "\n" +
                                "Source: " + sqlException.Errors[i].Source + "\n" +
                                "Procedure: " + sqlException.Errors[i].Procedure + "\n");
                        }
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}", @"Failed");
                        LogToText.WriteToLog($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}");
                        return;
                    }
                    catch (Exception exception) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message} \n \n {exception.InnerException}", @"Failed");
                        return;
                    }
                }
                if (uxImportTypeCmbo.SelectedIndex == 2) {
                    gridProcedureName = "SO_COGS";
                    StatusStripLabel.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application");
                    try {
                        Jonas.ImportInvoices(gridProcedureName, Table, importSource.Text);
                        StatusStripLabel.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Application");
                        DbConnectionsCs.LogImport(uxExcelSheetTxt.Text, "OpenCRM " + uxImportTypeCmbo.Text, uxExcelSheetViewerGv.RowCount);
                        return;
                    }
                    catch (SqlException sqlException) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");

                        StringBuilder errorMessages = new StringBuilder();
                        for (int i = 0; i < sqlException.Errors.Count; i++) {
                            errorMessages.Append("Index #" + i + "\n" +
                                "Message: " + sqlException.Errors[i].Message + "\n" +
                                "LineNumber: " + sqlException.Errors[i].LineNumber + "\n" +
                                "Source: " + sqlException.Errors[i].Source + "\n" +
                                "Procedure: " + sqlException.Errors[i].Procedure + "\n");
                        }
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}", @"Failed");
                        LogToText.WriteToLog($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{errorMessages.ToString()}");
                        return;
                    }
                    catch (Exception exception) {
                        StatusStripLabel.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                        UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message} \n \n {exception.InnerException}", @"Failed");
                        return;
                    }
                }
            }
            #endregion

            DbConnectionsCs.LogImport(uxExcelSheetTxt.Text, uxImportTypeCmbo.Text == @"Sales Order" ? "OpenCRM " + uxImportTypeCmbo.Text : uxImportTypeCmbo.Text, uxExcelSheetViewerGv.RowCount);
        }

        private void uxImportBtn_Click(object sender, EventArgs e) {

            if (uxExcelSheetTxt.Text == "") {
                UtilityMethods.ShowMessageBox(@"Please select an Excel sheet.", @"Error");
                return;
            }
            ImportFromGridView(uxImportSourceCmbo);

        }


        private void ExitToolStripMenuItemClick(object sender, EventArgs e) {
            CloseApplication();
        }

        private void CloseApplication() {
            Application.ExitThread();
            Application.Exit();
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e) {
            AboutBox box = new AboutBox { TopMost = true };
            box.ShowDialog();
        }

        private void ConnectionSettingsToolStripMenuItemClick(object sender, EventArgs e) {
            DatabaseConnection dbConnection = new DatabaseConnection {
                TopMost = true,
                StartPosition = FormStartPosition.CenterScreen
            };
            dbConnection.Activate();
            dbConnection.ShowDialog();
        }

        private void NominalCodeEditorToolStripMenuItemClick(object sender, EventArgs e) {
            NominalCodeEditor codeEditor = new NominalCodeEditor {
                TopMost = true,
                StartPosition = FormStartPosition.CenterScreen
            };
            codeEditor.Show();
        }

        private void MenuStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }


        private void GpExcelFileFindFileOk(object sender, CancelEventArgs e) {
            TableBindingSource = ExcelImport.ExcelDialogFind(
                gpExcelFileFind,
                uxExcelSheetTxt,
                uxExcelWorksheetCmbo,
                uxExcelSheetViewerGv,
                TableBindingSource);
        }


        private void customerLedgerByGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            //ReportViewer rViewer = new ReportViewer {TopMost = true};

            CreateNewReportWindow(@"/Sales Backlog and Raised Invoices");
        }


        private void invoicesPostedToPAndLToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewReportWindow(@"/Invoices Posted to P and L");
        }

        private void customerStatementToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewReportWindow(@"/Customer Statement");
        }

        private void salesBacklogToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewReportWindow(@"/Sales Backlog");
        }

        private void raisedInvoicesToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewReportWindow(@"/Raised Invoices");
        }
        private void costOfGoodsSoldToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateNewReportWindow(@"/Cost of Goods Sold");
        }

        private void CreateNewReportWindow(string path) {
            bool stopTimer = false;

            Loading lS = new Loading { TopMost = true };
            lS.UpdateText($"Loading Report, please wait...\nThis may take up to a minute the first time the report is generated.");

            if (IsSqlClrTypesInstalled()) {
                if (IsReportViewerInstalled()) {
                    try {
                        while (stopTimer == false) {
                            lS.Show();
                            lS.Update();
                            ReportViewer rViewer = new ReportViewer {
                                TopMost = true,
                                StartPosition = FormStartPosition.CenterScreen
                            };
                            rViewer.ReportServerPathName(path);
                            rViewer.Show();
                            stopTimer = true;
                        }
                    }
                    catch (FileNotFoundException fex) {
                        UtilityMethods.ShowMessageBox(
                            $"Pre-requisite files are not found. Please ensure Report Viewer 2012 is installed\n\n{fex.Message}", "");
                    }
                    catch (Exception ex) {
                        UtilityMethods.ShowMessageBox($"Error loading reports.\n\n{ex.Message}", "");
                    }
                }
                else {
                    if (UtilityMethods.ShowMessageBox(
                            $"Microsoft Report Viewer is not installed.\n\n"
                           + @"Do you want to install this now?", @"Report Viewer is not installed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        UtilityMethods.ShowMessageBox(@"This application will now be minimised.");
                        WindowState = FormWindowState.Minimized;
                        Process.Start($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Resources\ReportViewer2012.msi");
                    }
                    else {
                        return;
                    }

                }
            }
            else {
                if (UtilityMethods.ShowMessageBox(
                    $"Microsoft CLR Types are not installed.\n\n" + @"Do you want to install this now?",
                    @"Microsoft CLR Types are not installed",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes) {
                    UtilityMethods.ShowMessageBox(@"This application will now be minimised");
                    WindowState = FormWindowState.Minimized;
                    Process.Start(
                        $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Resources\SQLSysClrTypes2012.msi");
                }
                else {
                    return;
                }
            }
            lS.Hide();

            if (lS.Visible) {
                lS.Hide();
            }
        }
        /// <summary>
        /// returns true if ReportViewer OR ReportViewer Language Pack is installed
        /// </summary>
        /// <returns></returns>
        public bool IsReportViewerInstalled() {
            RegistryKey registryBase = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            // check the two possible reportviewer v10 registry keys
            return registryBase.OpenSubKey(@"Software\Microsoft\ReportViewer\v2.0.50727") != null
                   || registryBase.OpenSubKey(@"Software\Microsoft\ReportViewer\v9.0") != null
                   || registryBase.OpenSubKey(@"Software\Wow6432Node\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ReportViewer v10") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\ReportViewer\v10.0") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Classes\Installer\Products\2443504FAD987B24B9C51B984CC4CB42") != null
                ;
        }
        public bool IsSqlClrTypesInstalled() {
            RegistryKey registryBase = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            return registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\RefCount\SQLSysClrTypes") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\RefCount\SQLSysClrTypes11") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server 2014 Redist\SQL Server System CLR Types") != null;
        }
        private void StatusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }
        public void UpdateStripText(string message) {
            StatusStripLabel.Text = message;
        }

        private void LoadImportSourceCmbo() {
            uxImportSourceCmbo.DataSource = Enum.GetValues(typeof(JonasImporterEnums.ImportSources))
                .Cast<Enum>()
                .Select(value => new {
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))).Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
            uxImportSourceCmbo.DisplayMember = "Description";
            uxImportSourceCmbo.ValueMember = "value";


        }

        private void LoadImportTypeCmbo(int importSourceCmboSelectedIndex) {
            switch (importSourceCmboSelectedIndex) {
                case 0:
                    uxImportTypeCmbo.DataSource = Enum.GetValues(typeof(JonasImporterEnums.GreatPlainsImportTypes))
                        .Cast<Enum>()
                        .Select(value => new {
                            (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                                typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
                            value
                        })
                        .OrderBy(item => item.value)
                        .ToList();
                    uxImportTypeCmbo.DisplayMember = "Description";
                    uxImportTypeCmbo.ValueMember = "value";
                    break;
                case 1:
                    uxImportTypeCmbo.DataSource = Enum.GetValues(typeof(JonasImporterEnums.CrmImportTypes))
                        .Cast<Enum>()
                        .Select(value => new {
                            (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                                typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
                            value
                        })
                        .OrderBy(item => item.value)
                        .ToList();
                    uxImportTypeCmbo.DisplayMember = "Description";
                    uxImportTypeCmbo.ValueMember = "value";
                    break;
            }
        }


        private void uxImportSourceCmbo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) {
            LoadImportTypeCmbo(uxImportSourceCmbo.SelectedIndex);
            if (uxImportSourceCmbo.SelectedIndex == 0) {
                uxRemoveNewerRecordsChk.Visible = true;
                uxRemoveNewerRecordsDt.Visible = true;
                uxInclusiveLbl.Visible = true;
                //uxEndofWeekChk.Visible = false;
            }
            else if (uxImportSourceCmbo.SelectedIndex == 1) {
                uxRemoveNewerRecordsChk.Visible = false;
                uxRemoveNewerRecordsDt.Visible = false;
                uxInclusiveLbl.Visible = false;
                //uxEndofWeekChk.Visible = true;

            }
            else {
                uxRemoveNewerRecordsChk.Checked = false;
                uxRemoveNewerRecordsChk.Visible = false;
                uxRemoveNewerRecordsDt.Visible = false;
                uxInclusiveLbl.Visible = false;
                //uxEndofWeekChk.Visible = false;
            }
        }

        private void uxExcelBrowseBtn_Click(object sender, EventArgs e) {
            if (uxImportTypeCmbo.SelectedIndex == -1) {
                UtilityMethods.ShowMessageBox(@"Please select an import type first.");
            }
            else {
                if (ExcelImport.ExcelDialogBox(gpExcelFileFind).ShowDialog() != DialogResult.OK) {
                    return;
                }
                try {
                    Table = ExcelImport.GetData(uxExcelWorksheetCmbo.Text, uxExcelSheetTxt);
                }
                catch (ArgumentException iaex) {
                    LogToText.WriteToLog($"Invalid Argument (Might have pressed close on the directory box - {iaex}");
                    return;
                }
                TableBindingSource.DataSource = Table;
            }
        }

        private void uxWorksheetUpdateBtn_Click(object sender, EventArgs e) {
            if (uxExcelSheetTxt.Text == "") {
                StatusStripLabel.Text = @"Nothing to Update!";
                return;
            }
            try {
                Table = ExcelImport.GetData(uxExcelWorksheetCmbo.Text, uxExcelSheetTxt);
            }
            catch (ArgumentException iaex) {
                LogToText.WriteToLog($"Invalid Argument (Might have pressed close on the directory box - {iaex}");
                return;
            }
            TableBindingSource.DataSource = Table;
        }


        private void uxRemoveNewerRecordsChk_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args) {
            uxRemoveNewerRecordsDt.Enabled = uxRemoveNewerRecordsChk.Checked;
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CheckForUpdates(false)) {
                CloseApplication();
            }
        }

        private static void DeleteUpdateFile() {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            File.Delete("UpdateFile.xml");
        }

        private bool CheckForUpdates(bool suppressUpToDateMsgBox) {
            try {
                using (var client = new WebClient()) {
                    DeleteUpdateFile();
                    client.DownloadFile("https://drive.google.com/uc?export=download&id=0B0omVYO3nyCiUW0yT1JtbDdlRHc", "UpdateFile.xml");
                }
                var fs = new FileStream("UpdateFile.xml", FileMode.Open, FileAccess.Read);
                var doc = new XmlDataDocument();
                doc.Load(fs);
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/item/version");
                XmlNode node2 = doc.DocumentElement.SelectSingleNode("/item/url");

                if (node1 != null) {
                    var updateVersion = new Version(node1.InnerText.ToString());
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    var versionComparionResult = updateVersion.CompareTo(currentVersion);

                    if (versionComparionResult > 0) {
                        var dialogResult = UtilityMethods.ShowMessageBox(
                            $"A newer version is available. Would you like to go to download this now?\n \n Current Version: {currentVersion} \n Latest version: {updateVersion} \n \n Download Link:\n {node2.InnerText}",
                            "New version available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes) {
                            System.Diagnostics.Process.Start(node2.InnerText.ToString());
                            UtilityMethods.ShowMessageBox(
                                "The application will now exit. Please download and install the latest version");
                            fs.Dispose();
                            DeleteUpdateFile();
                            return true;
                        }
                    }
                    else {
                        if (!suppressUpToDateMsgBox) {
                            UtilityMethods.ShowMessageBox("You're already on the latest version.");
                        }
                        fs.Dispose();
                        DeleteUpdateFile();
                        return false;
                    }
                    DeleteUpdateFile();
                    return false;
                }
                DeleteUpdateFile();
                return false;
            }
            catch (UnauthorizedAccessException) {
                DeleteUpdateFile();
                return false;
            }
            catch (IOException) {
                DeleteUpdateFile();
                return false;
            }
            catch (Exception) {
                DeleteUpdateFile();
                return false;
            }
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e) {
            GenerateGBUCombinedTargets.GenerateReport();
        }

        private void changeLogToolStripMenuItem_Click(object sender, EventArgs e) {
            PopReleaseNotes();
        }

        public static Form GetOpenedForm<T>() where T : Form {
            foreach (Form openForm in Application.OpenForms) {
                if (openForm.GetType() == typeof(T)) {
                    return openForm;
                }
            }
            return null;
        }

        private void PopReleaseNotes() {
            var rnForm = (ReleaseNotes)GetOpenedForm<ReleaseNotes>();
            if (rnForm == null) {
                var rn = new ReleaseNotes {
                    TopMost = true,
                    Owner = _form1,
                    StartPosition = FormStartPosition.CenterParent
                };
                //rn.Location = new Point(Screen.PrimaryScreen.Bounds.X, //should be (0,0)
                //    Screen.PrimaryScreen.Bounds.Y);
                //rn.StartPosition = FormStartPosition.Manual;
                rn.ShowDialog();
            }
            else {
                rnForm.Select();
            }
        }

        private void radButton1_Click(object sender, EventArgs e) {
            UtilityMethods.ShowMessageBox("test", "test");
        }
    }
}

