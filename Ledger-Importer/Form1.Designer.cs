using System.ComponentModel;
using System.DirectoryServices;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerLedgerByGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoicesPostedToPAndLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerStatementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesBacklogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raisedInvoicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nominalCodeEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jonasGroupEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gpExcelFileFind = new System.Windows.Forms.OpenFileDialog();
            this.uxImportSourceLbl = new System.Windows.Forms.Label();
            this.uxInclusiveLbl = new System.Windows.Forms.Label();
            this.uxImportTypeLbl = new System.Windows.Forms.Label();
            this.uxExcelSheetLbl = new System.Windows.Forms.Label();
            this.uxWorksheetLbl = new System.Windows.Forms.Label();
            this.uxExcelSheetViewerGv = new Telerik.WinControls.UI.RadGridView();
            this.uxRemoveNewerRecordsDt = new Telerik.WinControls.UI.RadDateTimePicker();
            this.uxRemoveNewerRecordsChk = new Telerik.WinControls.UI.RadCheckBox();
            this.uxExcelSheetTxt = new Telerik.WinControls.UI.RadTextBox();
            this.uxWorksheetUpdateBtn = new Telerik.WinControls.UI.RadButton();
            this.uxExcelBrowseBtn = new Telerik.WinControls.UI.RadButton();
            this.uxImportBtn = new Telerik.WinControls.UI.RadButton();
            this.uxImportTypeCmbo = new Telerik.WinControls.UI.RadDropDownList();
            this.uxImportSourceCmbo = new Telerik.WinControls.UI.RadDropDownList();
            this.uxExcelWorksheetCmbo = new Telerik.WinControls.UI.RadDropDownList();
            this.office2010SilverTheme1 = new Telerik.WinControls.Themes.Office2010SilverTheme();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetViewerGv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetViewerGv.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxRemoveNewerRecordsDt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxRemoveNewerRecordsChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxWorksheetUpdateBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelBrowseBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportTypeCmbo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportSourceCmbo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelWorksheetCmbo)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 463);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StatusStrip1.Size = new System.Drawing.Size(661, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 28;
            this.StatusStrip1.Text = "StatusStrip1";
            this.StatusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StatusStrip1_ItemClicked);
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.StatusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(661, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuStrip1ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customerLedgerByGroupToolStripMenuItem,
            this.invoicesPostedToPAndLToolStripMenuItem,
            this.customerStatementToolStripMenuItem,
            this.salesBacklogToolStripMenuItem,
            this.raisedInvoicesToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // customerLedgerByGroupToolStripMenuItem
            // 
            this.customerLedgerByGroupToolStripMenuItem.Enabled = false;
            this.customerLedgerByGroupToolStripMenuItem.Name = "customerLedgerByGroupToolStripMenuItem";
            this.customerLedgerByGroupToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.customerLedgerByGroupToolStripMenuItem.Text = "Sales Backlog and Raised Invoices (Old)";
            this.customerLedgerByGroupToolStripMenuItem.ToolTipText = "Please use Sales Backlog or Raised Invoices";
            this.customerLedgerByGroupToolStripMenuItem.Click += new System.EventHandler(this.customerLedgerByGroupToolStripMenuItem_Click);
            // 
            // invoicesPostedToPAndLToolStripMenuItem
            // 
            this.invoicesPostedToPAndLToolStripMenuItem.Name = "invoicesPostedToPAndLToolStripMenuItem";
            this.invoicesPostedToPAndLToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.invoicesPostedToPAndLToolStripMenuItem.Text = "Invoices Posted to P and L";
            this.invoicesPostedToPAndLToolStripMenuItem.Click += new System.EventHandler(this.invoicesPostedToPAndLToolStripMenuItem_Click);
            // 
            // customerStatementToolStripMenuItem
            // 
            this.customerStatementToolStripMenuItem.Name = "customerStatementToolStripMenuItem";
            this.customerStatementToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.customerStatementToolStripMenuItem.Text = "Customer Statement";
            this.customerStatementToolStripMenuItem.Click += new System.EventHandler(this.customerStatementToolStripMenuItem_Click);
            // 
            // salesBacklogToolStripMenuItem
            // 
            this.salesBacklogToolStripMenuItem.Name = "salesBacklogToolStripMenuItem";
            this.salesBacklogToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.salesBacklogToolStripMenuItem.Text = "Sales Backlog";
            this.salesBacklogToolStripMenuItem.Click += new System.EventHandler(this.salesBacklogToolStripMenuItem_Click);
            // 
            // raisedInvoicesToolStripMenuItem
            // 
            this.raisedInvoicesToolStripMenuItem.Name = "raisedInvoicesToolStripMenuItem";
            this.raisedInvoicesToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.raisedInvoicesToolStripMenuItem.Text = "Raised Invoices";
            this.raisedInvoicesToolStripMenuItem.Click += new System.EventHandler(this.raisedInvoicesToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nominalCodeEditorToolStripMenuItem,
            this.jonasGroupEditorToolStripMenuItem,
            this.connectionSettingsToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // nominalCodeEditorToolStripMenuItem
            // 
            this.nominalCodeEditorToolStripMenuItem.Name = "nominalCodeEditorToolStripMenuItem";
            this.nominalCodeEditorToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.nominalCodeEditorToolStripMenuItem.Text = "Nominal Code Editor";
            this.nominalCodeEditorToolStripMenuItem.Click += new System.EventHandler(this.NominalCodeEditorToolStripMenuItemClick);
            // 
            // jonasGroupEditorToolStripMenuItem
            // 
            this.jonasGroupEditorToolStripMenuItem.Enabled = false;
            this.jonasGroupEditorToolStripMenuItem.Name = "jonasGroupEditorToolStripMenuItem";
            this.jonasGroupEditorToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.jonasGroupEditorToolStripMenuItem.Text = "Jonas Group Editor";
            // 
            // connectionSettingsToolStripMenuItem
            // 
            this.connectionSettingsToolStripMenuItem.Name = "connectionSettingsToolStripMenuItem";
            this.connectionSettingsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.connectionSettingsToolStripMenuItem.Text = "Connection Settings";
            this.connectionSettingsToolStripMenuItem.Click += new System.EventHandler(this.ConnectionSettingsToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeLogToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // changeLogToolStripMenuItem
            // 
            this.changeLogToolStripMenuItem.Enabled = false;
            this.changeLogToolStripMenuItem.Name = "changeLogToolStripMenuItem";
            this.changeLogToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.changeLogToolStripMenuItem.Text = "Change Log";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
            // 
            // gpExcelFileFind
            // 
            this.gpExcelFileFind.FileName = "OpenFileDialog1";
            this.gpExcelFileFind.FileOk += new System.ComponentModel.CancelEventHandler(this.GpExcelFileFindFileOk);
            // 
            // uxImportSourceLbl
            // 
            this.uxImportSourceLbl.AutoSize = true;
            this.uxImportSourceLbl.Location = new System.Drawing.Point(57, 38);
            this.uxImportSourceLbl.Name = "uxImportSourceLbl";
            this.uxImportSourceLbl.Size = new System.Drawing.Size(73, 13);
            this.uxImportSourceLbl.TabIndex = 57;
            this.uxImportSourceLbl.Text = "Import Source";
            // 
            // uxInclusiveLbl
            // 
            this.uxInclusiveLbl.AutoSize = true;
            this.uxInclusiveLbl.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.uxInclusiveLbl.Location = new System.Drawing.Point(379, 156);
            this.uxInclusiveLbl.Name = "uxInclusiveLbl";
            this.uxInclusiveLbl.Size = new System.Drawing.Size(137, 13);
            this.uxInclusiveLbl.TabIndex = 56;
            this.uxInclusiveLbl.Text = "Note: This date is inclusive.";
            // 
            // uxImportTypeLbl
            // 
            this.uxImportTypeLbl.AutoSize = true;
            this.uxImportTypeLbl.Location = new System.Drawing.Point(67, 67);
            this.uxImportTypeLbl.Name = "uxImportTypeLbl";
            this.uxImportTypeLbl.Size = new System.Drawing.Size(63, 13);
            this.uxImportTypeLbl.TabIndex = 52;
            this.uxImportTypeLbl.Text = "Import Type";
            // 
            // uxExcelSheetLbl
            // 
            this.uxExcelSheetLbl.AutoSize = true;
            this.uxExcelSheetLbl.Location = new System.Drawing.Point(66, 98);
            this.uxExcelSheetLbl.Name = "uxExcelSheetLbl";
            this.uxExcelSheetLbl.Size = new System.Drawing.Size(64, 13);
            this.uxExcelSheetLbl.TabIndex = 48;
            this.uxExcelSheetLbl.Text = "Excel Sheet";
            // 
            // uxWorksheetLbl
            // 
            this.uxWorksheetLbl.AutoSize = true;
            this.uxWorksheetLbl.Location = new System.Drawing.Point(71, 128);
            this.uxWorksheetLbl.Name = "uxWorksheetLbl";
            this.uxWorksheetLbl.Size = new System.Drawing.Size(59, 13);
            this.uxWorksheetLbl.TabIndex = 49;
            this.uxWorksheetLbl.Text = "Worksheet";
            // 
            // uxExcelSheetViewerGv
            // 
            this.uxExcelSheetViewerGv.AutoScroll = true;
            this.uxExcelSheetViewerGv.Location = new System.Drawing.Point(9, 184);
            // 
            // 
            // 
            this.uxExcelSheetViewerGv.MasterTemplate.AllowAddNewRow = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowCellContextMenu = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowColumnChooser = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowColumnReorder = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowDeleteRow = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowDragToGroup = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowEditRow = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AllowRowResize = false;
            this.uxExcelSheetViewerGv.MasterTemplate.AutoExpandGroups = true;
            this.uxExcelSheetViewerGv.MasterTemplate.ShowRowHeaderColumn = false;
            this.uxExcelSheetViewerGv.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.uxExcelSheetViewerGv.Name = "uxExcelSheetViewerGv";
            this.uxExcelSheetViewerGv.ReadOnly = true;
            this.uxExcelSheetViewerGv.ShowGroupPanel = false;
            this.uxExcelSheetViewerGv.ShowNoDataText = false;
            this.uxExcelSheetViewerGv.Size = new System.Drawing.Size(643, 230);
            this.uxExcelSheetViewerGv.TabIndex = 59;
            this.uxExcelSheetViewerGv.Text = "radGridView1";
            this.uxExcelSheetViewerGv.UseScrollbarsInHierarchy = true;
            // 
            // uxRemoveNewerRecordsDt
            // 
            this.uxRemoveNewerRecordsDt.Location = new System.Drawing.Point(134, 152);
            this.uxRemoveNewerRecordsDt.Name = "uxRemoveNewerRecordsDt";
            this.uxRemoveNewerRecordsDt.Size = new System.Drawing.Size(240, 20);
            this.uxRemoveNewerRecordsDt.TabIndex = 60;
            this.uxRemoveNewerRecordsDt.TabStop = false;
            this.uxRemoveNewerRecordsDt.Text = "29 June 2016";
            this.uxRemoveNewerRecordsDt.Value = new System.DateTime(2016, 6, 29, 19, 47, 5, 950);
            // 
            // uxRemoveNewerRecordsChk
            // 
            this.uxRemoveNewerRecordsChk.Location = new System.Drawing.Point(6, 153);
            this.uxRemoveNewerRecordsChk.Name = "uxRemoveNewerRecordsChk";
            this.uxRemoveNewerRecordsChk.Size = new System.Drawing.Size(124, 18);
            this.uxRemoveNewerRecordsChk.TabIndex = 61;
            this.uxRemoveNewerRecordsChk.Text = "Remove Newer Than";
            this.uxRemoveNewerRecordsChk.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.uxRemoveNewerRecordsChk_ToggleStateChanged);
            // 
            // uxExcelSheetTxt
            // 
            this.uxExcelSheetTxt.BackColor = System.Drawing.SystemColors.ControlDark;
            this.uxExcelSheetTxt.Enabled = false;
            this.uxExcelSheetTxt.Location = new System.Drawing.Point(134, 94);
            this.uxExcelSheetTxt.Name = "uxExcelSheetTxt";
            this.uxExcelSheetTxt.ReadOnly = true;
            this.uxExcelSheetTxt.Size = new System.Drawing.Size(240, 20);
            this.uxExcelSheetTxt.TabIndex = 62;
            // 
            // uxWorksheetUpdateBtn
            // 
            this.uxWorksheetUpdateBtn.Location = new System.Drawing.Point(376, 123);
            this.uxWorksheetUpdateBtn.Name = "uxWorksheetUpdateBtn";
            this.uxWorksheetUpdateBtn.Size = new System.Drawing.Size(70, 21);
            this.uxWorksheetUpdateBtn.TabIndex = 63;
            this.uxWorksheetUpdateBtn.Text = "Update";
            this.uxWorksheetUpdateBtn.Click += new System.EventHandler(this.uxWorksheetUpdateBtn_Click);
            // 
            // uxExcelBrowseBtn
            // 
            this.uxExcelBrowseBtn.Location = new System.Drawing.Point(376, 94);
            this.uxExcelBrowseBtn.Name = "uxExcelBrowseBtn";
            this.uxExcelBrowseBtn.Size = new System.Drawing.Size(45, 20);
            this.uxExcelBrowseBtn.TabIndex = 64;
            this.uxExcelBrowseBtn.Text = "...";
            this.uxExcelBrowseBtn.Click += new System.EventHandler(this.uxExcelBrowseBtn_Click);
            // 
            // uxImportBtn
            // 
            this.uxImportBtn.Location = new System.Drawing.Point(284, 418);
            this.uxImportBtn.Name = "uxImportBtn";
            this.uxImportBtn.Size = new System.Drawing.Size(93, 40);
            this.uxImportBtn.TabIndex = 65;
            this.uxImportBtn.Text = "Import File";
            this.uxImportBtn.Click += new System.EventHandler(this.uxImportBtn_Click);
            // 
            // uxImportTypeCmbo
            // 
            this.uxImportTypeCmbo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.uxImportTypeCmbo.Location = new System.Drawing.Point(134, 66);
            this.uxImportTypeCmbo.Name = "uxImportTypeCmbo";
            this.uxImportTypeCmbo.Size = new System.Drawing.Size(240, 20);
            this.uxImportTypeCmbo.TabIndex = 66;
            // 
            // uxImportSourceCmbo
            // 
            this.uxImportSourceCmbo.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuart;
            this.uxImportSourceCmbo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.uxImportSourceCmbo.Location = new System.Drawing.Point(134, 37);
            this.uxImportSourceCmbo.Name = "uxImportSourceCmbo";
            this.uxImportSourceCmbo.Size = new System.Drawing.Size(240, 20);
            this.uxImportSourceCmbo.TabIndex = 67;
            this.uxImportSourceCmbo.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.uxImportSourceCmbo_SelectedIndexChanged);
            // 
            // uxExcelWorksheetCmbo
            // 
            this.uxExcelWorksheetCmbo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.uxExcelWorksheetCmbo.Location = new System.Drawing.Point(134, 123);
            this.uxExcelWorksheetCmbo.Name = "uxExcelWorksheetCmbo";
            this.uxExcelWorksheetCmbo.Size = new System.Drawing.Size(240, 20);
            this.uxExcelWorksheetCmbo.TabIndex = 68;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(661, 485);
            this.Controls.Add(this.uxExcelWorksheetCmbo);
            this.Controls.Add(this.uxImportSourceCmbo);
            this.Controls.Add(this.uxImportTypeCmbo);
            this.Controls.Add(this.uxImportBtn);
            this.Controls.Add(this.uxExcelBrowseBtn);
            this.Controls.Add(this.uxWorksheetUpdateBtn);
            this.Controls.Add(this.uxExcelSheetTxt);
            this.Controls.Add(this.uxRemoveNewerRecordsChk);
            this.Controls.Add(this.uxRemoveNewerRecordsDt);
            this.Controls.Add(this.uxExcelSheetViewerGv);
            this.Controls.Add(this.uxImportSourceLbl);
            this.Controls.Add(this.uxInclusiveLbl);
            this.Controls.Add(this.uxImportTypeLbl);
            this.Controls.Add(this.uxExcelSheetLbl);
            this.Controls.Add(this.uxWorksheetLbl);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Jonas Importer Tool";
            this.Load += new System.EventHandler(this.Form1Load);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetViewerGv.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetViewerGv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxRemoveNewerRecordsDt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxRemoveNewerRecordsChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelSheetTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxWorksheetUpdateBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelBrowseBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportTypeCmbo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxImportSourceCmbo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxExcelWorksheetCmbo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal ToolStripStatusLabel StatusStripLabel;
        private DirectoryEntry directoryEntry1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem nominalCodeEditorToolStripMenuItem;
        private ToolStripMenuItem jonasGroupEditorToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem changeLogToolStripMenuItem;
        private ToolStripMenuItem connectionSettingsToolStripMenuItem;
        internal OpenFileDialog gpExcelFileFind;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem customerLedgerByGroupToolStripMenuItem;
        private ToolStripMenuItem invoicesPostedToPAndLToolStripMenuItem;
        public StatusStrip StatusStrip1;
        private ToolStripMenuItem customerStatementToolStripMenuItem;
        internal Label uxImportSourceLbl;
        private Label uxInclusiveLbl;
        internal Label uxImportTypeLbl;
        internal Label uxExcelSheetLbl;
        internal Label uxWorksheetLbl;
        private Telerik.WinControls.UI.RadGridView uxExcelSheetViewerGv;
        private Telerik.WinControls.UI.RadDateTimePicker uxRemoveNewerRecordsDt;
        private Telerik.WinControls.UI.RadCheckBox uxRemoveNewerRecordsChk;
        private Telerik.WinControls.UI.RadTextBox uxExcelSheetTxt;
        private Telerik.WinControls.UI.RadButton uxWorksheetUpdateBtn;
        private Telerik.WinControls.UI.RadButton uxExcelBrowseBtn;
        private Telerik.WinControls.UI.RadButton uxImportBtn;
        private Telerik.WinControls.UI.RadDropDownList uxImportTypeCmbo;
        private Telerik.WinControls.UI.RadDropDownList uxImportSourceCmbo;
        private Telerik.WinControls.UI.RadDropDownList uxExcelWorksheetCmbo;
        private Telerik.WinControls.Themes.Office2010SilverTheme office2010SilverTheme1;
        private ToolStripMenuItem salesBacklogToolStripMenuItem;
        private ToolStripMenuItem raisedInvoicesToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
    }
}

