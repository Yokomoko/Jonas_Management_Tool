using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Jonas_Sage_Importer;
using Jonas_Sage_Importer.EditorControls;
using Jonas_Sage_Importer.Generate_Excel_Reports;
using Jonas_Sage_Importer.Properties;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SageImporterLibrary;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using PositionChangedEventArgs = Telerik.WinControls.UI.Data.PositionChangedEventArgs;

namespace Jonas_Sage_Importer {
    public partial class RadForm1 : RadForm {

        #region Enums

        enum ImportStages { Grid = 0, Temp = 1 }

        #endregion

        #region public and private properties

        protected OleDbDataAdapter DataAdapter = new OleDbDataAdapter();
        protected BindingSource TableBindingSource = new BindingSource();
        protected DataTable Table = new DataTable();

        public static RadForm1 _radForm1 = new RadForm1();
        #endregion

        #region Constructor
        public RadForm1() {
            InitializeComponent();
            _radForm1 = this;
            LoadImportSourceCmbo();
        }
        #endregion

        #region Event Handlers
        private void RadForm1Load(object sender, EventArgs e) {
            var screen = Screen.PrimaryScreen.WorkingArea;
            Top = (screen.Height / 2) - (Height / 2);
            Left = (screen.Width / 2) - (Width / 2);
            Text = Application.ProductName;
            uxRemoveNewerRecordsDt.Value = DateTime.Today;
            uxImportSourceCmbo.SelectedIndex = 1;
            radLabelElement1.Text = "OK";
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

            //Set Theme
            switch (Settings.Default.Theme) {
                case 0:
                SetLightTheme();
                break;
                case 1:
                SetDarkTheme();
                break;
                case 2:
                SetBreezeTheme();
                break;
                default:
                SetLightTheme();
                break;
            }
        }

        private string GetGridProcedureName(ImportStages importStage) {
            var name = "";
            if (uxImportSourceCmbo.SelectedIndex == 0) {
                switch (uxImportTypeCmbo.SelectedIndex) {
                    case -1:
                    UtilityMethods.ShowMessageBox("Please select an Import Type", "");
                    return "";
                    case 0: //AR
                    name = $"GP_{importStage}_ImportInvoices";
                    break;
                    case 1: //Posted to P+L
                    name = $"GP_{importStage}_ImportPostedInvoices";
                    break;
                    case 2: //Outstanding Invoices
                    Jonas.DeleteHistoricalLedger(uxImportTypeCmbo.SelectedIndex, new DateTime(1900, 01, 01), uxImportSourceCmbo.Text);
                    name = $"GP_{importStage}_ImportOutstandingInvoices";
                    break;
                }

            }
            else if (uxImportSourceCmbo.SelectedIndex == 1) {
                switch (Table.Columns.Count) {
                    case 14:
                    name = "CRM_Temp_ImportOrders";
                    break;
                    case 21:
                    name = "CRM_Temp_ImportOrders_Adv";
                    break;
                    case 22:
                    name = "CRM_Temp_ImportOrders_Adv2";
                    break;
                    case 23:
                    name = "CRM_Temp_ImportOrders_Adv3";
                    break;
                    default:
                    UtilityMethods.ShowMessageBox("The number of columns should be either 14,21,22 or 23");
                    return "";
                }
            }
            return "";
        }

        private void ImportGreatPlains() {
            if (uxExcelSheetViewerGv.Rows.Count == 0) {
                UtilityMethods.ShowMessageBox("Please select an Excel sheet first so that there is information in the table.", "");
                return;
            }
            if (uxRemoveNewerRecordsChk.Checked) {
                Jonas.DeleteHistoricalCheck(uxImportSourceCmbo, uxImportTypeCmbo, true, uxRemoveNewerRecordsDt.Value);
            }
            string gridProcedureName = null;
            string tempProcedureName = null;
            switch (uxImportTypeCmbo.SelectedIndex) {
                case 0: //AR
                gridProcedureName = "GP_Grid_ImportInvoices";
                tempProcedureName = "GP_Temp_ImportInvoices";
                break;
                case 1: //Posted to P+L
                gridProcedureName = "GP_Grid_ImportPostedInvoices";
                tempProcedureName = "GP_Temp_ImportPostedInvoices";
                break;
                case 2: //Outstanding Invoices
                Jonas.DeleteHistoricalLedger(uxImportTypeCmbo.SelectedIndex, new DateTime(1900, 01, 01), uxImportSourceCmbo.Text);
                gridProcedureName = "GP_Grid_ImportOutstandingInvoices";
                tempProcedureName = "GP_Temp_ImportOutstandingInvoices";
                break;
                default:
                UtilityMethods.ShowMessageBox("Please select an Import Type");
                return;
            }
            try {
                radLabelElement1.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application.");
                Jonas.ImportInvoices(gridProcedureName, Table, uxImportSourceCmbo.Text);
            }
            catch (Exception exception) {
                radLabelElement1.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message}", "Failed");
                return;
            }
            try {
                radLabelElement1.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Temporary Table.");
                Jonas.CommitImport(tempProcedureName, uxImportSourceCmbo.Text);
                radLabelElement1.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Temporary Table.");
            }
            catch (Exception exception) {
                radLabelElement1.Text = $"Failed to Import {uxImportTypeCmbo.Text} from Temporary Table.";
                UtilityMethods.ShowMessageBox(
                    $"Failed to import {uxImportTypeCmbo.Text} from Temporary Table.\n\n{exception.Message}",
                    "Failed");
            }
        }

        private void ImportCRM() {
            var gridProcedureName = "";
            var tempProcedureName = "";
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
                    UtilityMethods.ShowMessageBox("The number of columns should be either 14,21,22 or 23");
                    return;
                }
                try {
                    radLabelElement1.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application");
                    Jonas.ImportInvoices(gridProcedureName, Table, uxImportSourceCmbo.Text);
                    radLabelElement1.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Application");
                    if (gridProcedureName == "CRM_ImportCogs") {
                        return;
                    }
                }
                catch (Exception exception) {
                    radLabelElement1.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                    UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message} \n \n {exception.InnerException}", "Failed");
                    return;
                }
                try {
                    radLabelElement1.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Temporary Table.");
                    Jonas.CommitImport(tempProcedureName, uxImportSourceCmbo.Text);
                    radLabelElement1.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Temporary Table.");
                }
                catch (Exception exception) {
                    radLabelElement1.Text = $"Failed to Import {uxImportTypeCmbo.Text} from Temporary Table.";
                    UtilityMethods.ShowMessageBox(
                        $"Failed to import {uxImportTypeCmbo.Text} from Temporary Table.\n\n{exception.Message}",
                        "Failed");
                    return;
                }
            }
            else if (uxImportTypeCmbo.SelectedIndex == 1) {
                tempProcedureName = "CRM_ImportCogs";
            }
            else if (uxImportTypeCmbo.SelectedIndex == 2) {
                tempProcedureName = "SO_COGS";
            }
            try {
                radLabelElement1.Text = ($"Attempting to Import {uxImportTypeCmbo.Text} from Application");
                Jonas.ImportInvoices(tempProcedureName, Table, uxImportSourceCmbo.Text);
                radLabelElement1.Text = ($"Successfully imported {uxImportTypeCmbo.Text} from Application");
                DbConnectionsCs.LogImport(uxExcelSheetTxt.Text, "OpenCRM " + uxImportTypeCmbo.Text, uxExcelSheetViewerGv.RowCount);
            }
            catch (Exception exception) {
                radLabelElement1.Text = ($"Failed to Import {uxImportTypeCmbo.Text} from Application.");
                UtilityMethods.ShowMessageBox($"Failed to import {uxImportTypeCmbo.Text} from Application.\n\n{exception.Message}", "Failed");
            }
        }

        private void ImportFromGridView(RadDropDownList importSource) {
            if (importSource == null) throw new ArgumentNullException(nameof(importSource));
            string gridProcedureName;
            string tempProcedureName;

            #region GreatPlains
            if (importSource.SelectedIndex == 0) {
                ImportGreatPlains();
            }
            #endregion
            #region CRM

            if (importSource.SelectedIndex == 1) {
                ImportCRM();
            }
            #endregion

            DbConnectionsCs.LogImport(uxExcelSheetTxt.Text, uxImportTypeCmbo.Text == "Sales Order" ? "OpenCRM " + uxImportTypeCmbo.Text : uxImportTypeCmbo.Text, uxExcelSheetViewerGv.RowCount);
        }

        private void uxImportBtn_Click(object sender, EventArgs e) {

            if (uxExcelSheetTxt.Text == "") {
                UtilityMethods.ShowMessageBox("Please select an Excel sheet.", "Error");
                return;
            }
            ImportFromGridView(uxImportSourceCmbo);

        }

        private void uxAboutRmi_Click(object sender, EventArgs e) {
            AboutBox box = new AboutBox { TopMost = true };
            box.ShowDialog();
        }

        private void uxConnCfgRmi_Click(object sender, EventArgs e) {
            DatabaseConnection dbConnection = new DatabaseConnection {
                StartPosition = FormStartPosition.CenterScreen
            };
            dbConnection.Activate();
            dbConnection.ShowDialog();
        }

        private void uxNomCodeEditorRmi_Click(object sender, EventArgs e) {
            NominalCodeEditor_Rad codeEditor = new NominalCodeEditor_Rad {
                TopMost = true,
                StartPosition = FormStartPosition.CenterScreen
            };
            codeEditor.ShowDialog();
        }

        private void GpExcelFileFindFileOk(object sender, CancelEventArgs e) {
            Stream strm = null;
            try {
                strm = gpExcelFileFind.OpenFile();
            }
            catch (IOException ioex) {
                UtilityMethods.ShowMessageBox($"File is being used elsewhere. Please close the file and try again. \n {ioex.Message}");
            }
            catch (Exception ex) {
                UtilityMethods.ShowMessageBox($"Error opening file: \n {ex.Message}");
            }
            uxExcelSheetTxt.Text = gpExcelFileFind.FileName;
            Type officeType = Type.GetTypeFromProgID("Excel.Application");

            if (officeType == null) {
                UtilityMethods.ShowMessageBox("Excel is not installed. Please install Excel and try again.");
            }
            else {
                try {
                    strm?.Close();
                    var oXL = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                    var oWb = oXL.Workbooks.Open(uxExcelSheetTxt.Text, ReadOnly: true);
                    uxExcelWorksheetCmbo.Items.Clear();
                    foreach (Worksheet oSheet in oWb.Sheets) {
                        uxExcelWorksheetCmbo.Items.Add(oSheet.Name);
                    }
                    uxExcelWorksheetCmbo.SelectedIndex = 0;
                    oWb.Close(0);
                    oXL.Application.Quit();
                }
                catch (ApplicationException noExcel) {
                    UtilityMethods.ShowMessageBox($"Unable to open Excel document. Excel may not be installed. Please install Microsoft Excel Viewer or Microsoft Office and try again. \n \n {noExcel.Message}");
                    return;
                }
                catch (Exception ex) {
                    UtilityMethods.ShowMessageBox($"Unable to open Excel document. \n \n + {ex.Message}", "Error");
                    return;
                }
            }
            uxExcelSheetViewerGv.DataSource = TableBindingSource;
        }

        public void UpdateStripText(string message) {
            radLabelElement1.Text = message;
        }

        private void uxImportSourceCmbo_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
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
                UtilityMethods.ShowMessageBox("Please select an import type first.");
            }
            else {
                gpExcelFileFind.Title = "Please Select a File";
                gpExcelFileFind.FileName = "";
                gpExcelFileFind.ValidateNames = true;
                gpExcelFileFind.Filter = "Excel Worksheets|*.xls;*.xlsx;";
                gpExcelFileFind.FilterIndex = 1;
                gpExcelFileFind.ShowReadOnly = true;
                gpExcelFileFind.ReadOnlyChecked = true;

                if (gpExcelFileFind.ShowDialog() != DialogResult.OK) {
                    return;
                }
                try {
                    FillDataTable();
                }
                catch (ArgumentException iaex) {
                    LogToText.WriteToLog($"Invalid Argument (Might have pressed close on the directory box - {iaex}");
                    return;
                }
                TableBindingSource.DataSource = Table;

                for (int i = 0; i < uxExcelSheetViewerGv.Columns.Count(); i++) {
                    uxExcelSheetViewerGv.Columns[i].BestFit();
                }
            }
        }

        private void uxWorksheetUpdateBtn_Click(object sender, EventArgs e) {
            if (uxExcelSheetTxt.Text == "") {
                radLabelElement1.Text = "Nothing to Update!";
                return;
            }
            try {
                FillDataTable();
            }
            catch (ArgumentException iaex) {
                LogToText.WriteToLog($"Invalid Argument (Might have pressed close on the directory box - {iaex}");
                return;
            }
            TableBindingSource.DataSource = Table;
            for (int i = 0; i < uxExcelSheetViewerGv.Columns.Count(); i++) {
                uxExcelSheetViewerGv.Columns[i].BestFit();
            }
        }

        private void uxRemoveNewerRecordsChk_ToggleStateChanged(object sender, StateChangedEventArgs args) {
            uxRemoveNewerRecordsDt.Enabled = uxRemoveNewerRecordsChk.Checked;
        }

        private void uxUpdateChkRmi_Click(object sender, EventArgs e) {
            if (CheckForUpdates(false)) {
                CloseApplication();
            }
        }

        private void uxExitRmi_Click(object sender, EventArgs e) {
            CloseApplication();
        }

        #region Themes
        private void uxLightThemeRmi_Click(object sender, EventArgs e) {
            SetLightTheme();
        }

        private void uxDarkThemeRmi_Click(object sender, EventArgs e) {
            SetDarkTheme();
        }

        private void uxBreezeThmRmi_Click(object sender, EventArgs e) {
            SetBreezeTheme();
        }
        private static void SetLightTheme() {
            ThemeResolutionService.ApplicationThemeName = "Office2013Light";
            Settings.Default.Theme = 0;
            Settings.Default.Save();
        }

        private static void SetDarkTheme() {
            ThemeResolutionService.ApplicationThemeName = "Office2013Dark";
            Settings.Default.Theme = 1;
            Settings.Default.Save();
        }

        private static void SetBreezeTheme() {
            ThemeResolutionService.ApplicationThemeName = "Breeze";
            Settings.Default.Theme = 2;
            Settings.Default.Save();
        }
        #endregion
        #region Report Strip Bar
        private void uxInvoicePlRmi_Click(object sender, EventArgs e) {
            CreateNewReportWindow("/Invoices Posted to P and L");
        }

        private void uxStatementRmi_Click(object sender, EventArgs e) {
            CreateNewReportWindow("/Customer Statement");
        }

        private void uxBacklogRmi_Click(object sender, EventArgs e) {
            CreateNewReportWindow("/Sales Backlog");
        }

        private void uxRaisedInvoicesRmi_Click(object sender, EventArgs e) {
            CreateNewReportWindow("/Raised Invoices");
        }

        private void uxCogsRmi_Click(object sender, EventArgs e) {
            CreateNewReportWindow("/Cost of Goods Sold");
        }

        private void uxGbuRmi_Click(object sender, EventArgs e) {
            GenerateGBUCombinedTargets.GenerateReport();
        }
        #endregion
        #endregion

        #region private methods
        private void CreateNewReportWindow(string path) {
            bool stopTimer = false;

            Loading lS = new Loading { TopMost = true };
            lS.UpdateText("Loading Report, please wait...\nThis may take up to a minute the first time the report is generated.");

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
                            "Microsoft Report Viewer is not installed.\n\n"
                           + "Do you want to install this now?", "Report Viewer is not installed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        UtilityMethods.ShowMessageBox("This application will now be minimised.");
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
                    "Microsoft CLR Types are not installed.\n\n" + "Do you want to install this now?",
                    "Microsoft CLR Types are not installed",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes) {
                    UtilityMethods.ShowMessageBox("This application will now be minimised");
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
        private bool IsReportViewerInstalled() {
            RegistryKey registryBase = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            // check the two possible reportviewer v10 registry keys
            return registryBase.OpenSubKey(@"Software\Microsoft\ReportViewer\v2.0.50727") != null
                   || registryBase.OpenSubKey(@"Software\Microsoft\ReportViewer\v9.0") != null
                   || registryBase.OpenSubKey(@"Software\Wow6432Node\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\ReportViewer v10") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\ReportViewer\v10.0") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Classes\Installer\Products\2443504FAD987B24B9C51B984CC4CB42") != null
                ;
        }
        private bool IsSqlClrTypesInstalled() {
            RegistryKey registryBase = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty);
            return registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\RefCount\SQLSysClrTypes") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\RefCount\SQLSysClrTypes11") != null
                   || registryBase.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server 2014 Redist\SQL Server System CLR Types") != null;
        }

        private static void CloseApplication() {
            Application.ExitThread();
            Application.Exit();
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

        private static void DeleteUpdateFile() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete("UpdateFile.xml");
        }

        private static bool CheckForUpdates(bool suppressUpToDateMsgBox) {
            try {
                using (var client = new WebClient()) {
                    DeleteUpdateFile();
                    client.DownloadFile("https://drive.google.com/uc?export=download&id=0B0omVYO3nyCiUW0yT1JtbDdlRHc", "UpdateFile.xml");
                }
                var fs = new FileStream("UpdateFile.xml", FileMode.Open, FileAccess.Read);
                var doc = new XmlDataDocument();
                doc.Load(fs);
                XmlNode node1 = doc.DocumentElement?.SelectSingleNode("/item/version");
                XmlNode node2 = doc.DocumentElement?.SelectSingleNode("/item/url");

                if (node1 != null && node2 != null) {
                    var updateVersion = new Version(node1.InnerText);
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    var versionComparionResult = updateVersion.CompareTo(currentVersion);

                    if (versionComparionResult > 0) {
                        var dialogResult = UtilityMethods.ShowMessageBox($"A newer version is available. Would you like to go to download this now?\n \n Current Version: {currentVersion} \n Latest version: {updateVersion} \n \n Download Link:\n {node2.InnerText}", "New version available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult != DialogResult.Yes) {
                            DeleteUpdateFile();
                            return false;
                        }
                        Process.Start(node2.InnerText);
                        UtilityMethods.ShowMessageBox(
                            "The application will now exit. Please download and install the latest version");
                        fs.Dispose();
                        DeleteUpdateFile();
                        return true;
                    }
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
            catch (Exception) {
                DeleteUpdateFile();
                return false;
            }
        }

        private void uxRelNotesRmi_Click(object sender, EventArgs e) {
            PopReleaseNotes();
        }

        private static Form GetOpenedForm<T>() where T : Form {
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
                    Owner = _radForm1,
                    StartPosition = FormStartPosition.CenterParent
                };
                rn.ShowDialog();
            }
            else {
                rnForm.Select();
            }
        }

        private void FillDataTable() {
            string connectionString = ($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source ={uxExcelSheetTxt.Text};Extended Properties= \"Excel 8.0;HDR=Yes;IMED=1\"");
            OleDbConnection connectionBuilder = new OleDbConnection(connectionString);
            string command = $"Select * from [{uxExcelWorksheetCmbo.Text}$]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(command, connectionBuilder);
            try {
                Table.Rows.Clear();
                adapter.Fill(Table);
            }
            catch (InvalidOperationException ioexception) {
                UtilityMethods.ShowMessageBox(
                   @"64-Bit OLEDB Provider for ODBC Not Installed. Please go to the Resources folder in your install directory (Default C:\Program Files (x86)\Eposgroup\Jonas Ledger Management Tool\Resources) and run Ace.exe" + $"\n \n {ioexception.Message}");
                if (UtilityMethods.ShowMessageBox("64-Bit OLEDB Provider for ODBC Not Installed.\n\nDo you want to install this now?", "64-Bit OLEDB Provider for ODBC pnot installed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    UtilityMethods.ShowMessageBox("This application will now be minimised.");
                    Process.Start($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Resources\Ace.exe");
                }
                else {
                    return;
                }
            }
            catch (ArgumentException argex) {
                LogToText.WriteToLog($"This is an informational message. Argument Exception. {argex.Message}");
                return;
            }
            catch (Exception ex) {
                UtilityMethods.ShowMessageBox($"Something went wrong. \n \n {ex.Message} \n \n {ex.InnerException}");
                return;
            }

            //Remove Excess Columns
            if (Table != null) {
                int originalSize = Table.Columns.Count;
                int columnSize = 17;
                //If the excel sheet has 41 columns, trim 8 columns from the beginning and 16 from the end so that it complies.
                if (originalSize == 41) {
                    UtilityMethods.ShowMessageBox($"The application has detected {originalSize} in this spreadsheet.\n8 Columns will automatically be trimmed from the beginning and 16 from the end in order to comply with import standards.");
                    for (var i = 0; i < 8; i++) {
                        Table.Columns.RemoveAt(0);
                    }
                    while (Table.Columns.Count > columnSize) {
                        Table.Columns.RemoveAt(columnSize);
                    }
                    var TableClone = Table.Clone();
                    TableClone.Columns[0].DataType = typeof(string);
                    TableClone.Columns[4].DataType = typeof(string);
                    TableClone.Columns[6].DataType = typeof(string);
                    TableClone.Columns[7].DataType = typeof(string);
                    foreach (DataRow row in Table.Rows) {
                        TableClone.ImportRow(row);
                    }
                    Table = TableClone;
                }
            }
        }
        #endregion

    }
}
