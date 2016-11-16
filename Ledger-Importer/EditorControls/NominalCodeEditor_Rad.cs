using BL_JonasSageImporter;
using SageImporterLibrary;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using Jonas_Sage_Importer.Properties;
using Telerik.WinControls;

namespace Jonas_Sage_Importer.EditorControls {
    public partial class NominalCodeEditor_Rad : Telerik.WinControls.UI.RadForm {

        #region Public and Private Properties

        private readonly DataSet ds = new DataSet();

        private DataSet changes;

        readonly DbConnectionsCs dbConnections = new DbConnectionsCs();
        private readonly Purchase_SaleLedgerEntities ef = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
        #endregion

        #region Constructors
        public NominalCodeEditor_Rad() {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers

        private void NominalCodeEditor_Rad_Load(object sender, EventArgs e) {
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
            BindGrid();
            try {
                uxNomCodeListGv.Columns[1].Width = uxNomCodeListGv.Width - uxNomCodeListGv.Columns[0].Width - 21;
            }
            catch {
                // ignored
            } //Do Nothing
        }

        private void uxNomFilterTxt_TextChanged(object sender, EventArgs e) {
            // this.bindingSource.Filter = $"convert(NominalCode,'System.String') like '%{this.textBox1.Text.ToString()}%' OR convert(Description,'System.String') like '%{this.textBox1.Text.ToString()}%'";
            string rowFilter = $"Convert([NominalCode],\'System.String\') like \'%{uxNomFilterTxt.Text}%\'";
            rowFilter += $"OR [Description] like \'%{uxNomFilterTxt.Text}%\'";

            DataTable dataTable = uxNomCodeListGv.DataSource as DataTable;
            if (dataTable != null) {
                dataTable.DefaultView.RowFilter = rowFilter;
            }
        }

        private void uxCloseBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void uxAddBtn_Click(object sender, EventArgs e) {
            var nominalCode = !string.IsNullOrEmpty(uxNomCodeTxt.Text) ? int.Parse(uxNomCodeTxt.Text) : (int?)null;

            if (nominalCode == null) UtilityMethods.ShowMessageBox("Please Enter Nominal Code and Description. The Nominal Code can not be blank.");
            else
                try {
                    var glType = new GLType {
                        GLNo = (int)nominalCode,
                        GLDescription = uxNomDescTxt.Text
                    };
                    ef.GLTypes.Add(glType);
                    ef.SaveChanges();
                    Populate();
                }
                catch (Exception ex) {
                    UtilityMethods.ShowMessageBox($"Unable to complete Update Command: \n \n {ex.Message}");
                }
        }

        private void uxNomCodeTxt_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void uxDeleteBtn_Click(object sender, EventArgs e) {
            if (uxNomCodeListGv.SelectedRows.Count > 0) {
                DialogResult dResult = UtilityMethods.ShowMessageBox(
                "Are you sure you want to delete this nominal code? \nOnce it is removed you will not be able to recover this.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dResult != DialogResult.Yes) return;
                var rowToBeDeleted = (int)uxNomCodeListGv.SelectedRows[0].Cells[0].Value;

                var gltype = (from o in ef.GLTypes where o.GLNo == rowToBeDeleted select o).First();
                ef.GLTypes.Attach(gltype);
                ef.GLTypes.Remove(gltype);
                ef.SaveChanges();
                BindGrid();
            }
        }

        private void uxSaveBtn_Click(object sender, EventArgs e) {
            try {

                changes = ds.GetChanges();


                if (changes != null) {
                    ds.AcceptChanges();
                    dbConnections.GetNominalCodeAdapter().AcceptChangesDuringUpdate = true;
                    dbConnections.GetNominalCodeAdapter().Update(changes);
                    UtilityMethods.ShowMessageBox("The Nominal Codes have been Updated.");
                }
            }
            catch (Exception ex) {
                UtilityMethods.ShowMessageBox($"Error updating Nominal Codes \n \n {ex.Message}");
            }
        }

        private void uxRefreshBtn_Click(object sender, EventArgs e) {
            BindGrid();
        }

        #endregion

        #region Private Methods

        private void Populate() {
            BindGrid();
            uxNomCodeTxt.Text = "";
            uxNomDescTxt.Text = "";
        }

        private void BindGrid() {
            try {
                uxNomCodeListGv.DataSource = ef.GLTypes.Local.ToBindingList();
                uxNomCodeListGv.Refresh();
            }
            catch (Exception ex) {
                MessageBox.Show("An Exception has Occurred. Please check you have access to the database and try again. \n\n" + ex.Message);
            }
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
    }
}
