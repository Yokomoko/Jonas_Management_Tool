using BL_JonasSageImporter;
using SageImporterLibrary;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using Jonas_Sage_Importer.Properties;
using Telerik.WinControls.Themes;
using Telerik.WinControls;

namespace Jonas_Sage_Importer.EditorControls {
    public partial class NominalCodeEditor_Rad : Telerik.WinControls.UI.RadForm {

        #region Public and Private Properties

        private readonly string connectionString = DbConnectionsCs.ConnectionString();

        private DataSet ds = new DataSet();

        private DataSet changes;

        DbConnectionsCs dbConnections = new DbConnectionsCs();

        private string nCode = string.Empty;
        private string nDesc = string.Empty;

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
            catch (Exception) {
                return;
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
            nCode = uxNomCodeTxt.Text;
            nDesc = uxNomDescTxt.Text;
            int nominalCode = nCode != string.Empty ? int.Parse(nCode) : 0;

            string nominalDescription = nDesc;

            if (nominalCode != 0 && nominalDescription != string.Empty) {
                try {
                    var glType = new GLType();
                    glType.GLNo = nominalCode;
                    glType.GLDescription = nominalDescription;

                    using (var dbCtx = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString())) {
                        dbCtx.Entry(glType).State = System.Data.Entity.EntityState.Added;
                        dbCtx.SaveChanges();
                    }

                    BindGrid();

                    uxNomCodeTxt.Text = "";
                    uxNomDescTxt.Text = "";
                }
                catch (SqlException sqlex) {
                    UtilityMethods.ShowMessageBox($"Unable to complete Update Command: \n \n {sqlex.Message}");
                }
                catch (Exception ex) {
                    UtilityMethods.ShowMessageBox($"Unable to complete Update Command: \n \n {ex.Message}");
                }
            }
            else {
                UtilityMethods.ShowMessageBox(@"Please Enter Nominal Code and Description. The Nominal Code can not be blank.");
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
                $"Are you sure you want to delete this nominal code? \nOnce it is removed you will not be able to recover this.",
                @"Are you sure?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (dResult != DialogResult.Yes) return;
                var rowToBeDeleted = (int)uxNomCodeListGv.SelectedRows[0].Cells[0].Value;
                var context = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
                var gltype = (from o in context.GLTypes where o.GLNo == rowToBeDeleted select o).First();
                context.GLTypes.Attach(gltype);
                context.GLTypes.Remove(gltype);
                context.SaveChanges();
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


                    UtilityMethods.ShowMessageBox(@"The Nominal Codes have been Updated.");
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

        private void BindGrid() {
            try {
                var context = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
                var query = from c in context.GLTypes select c;
                var nominalCodes = query.ToList();
                uxNomCodeListGv.DataSource = nominalCodes;
            }
            catch (Exception ex) {
                MessageBox.Show($"An Exception has Occurred. Please check you have access to the database and try again. \n\n" + ex.Message);
                return;
            }
        }

        private void SetLightTheme() {
            Office2013LightTheme lighttheme = new Office2013LightTheme();
            ThemeResolutionService.ApplicationThemeName = "Office2013Light";
            Settings.Default.Theme = 0;
            Settings.Default.Save();
        }

        private void SetDarkTheme() {
            Office2013DarkTheme darkTheme = new Office2013DarkTheme();
            ThemeResolutionService.ApplicationThemeName = "Office2013Dark";
            Settings.Default.Theme = 1;
            Settings.Default.Save();
        }

        private void SetBreezeTheme() {
            BreezeTheme breeze = new BreezeTheme();
            ThemeResolutionService.ApplicationThemeName = "Breeze";
            Settings.Default.Theme = 2;
            Settings.Default.Save();
        }

        #endregion

    }
}
