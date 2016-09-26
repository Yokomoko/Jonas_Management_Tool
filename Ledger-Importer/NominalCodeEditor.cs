using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SageImporterLibrary;

namespace Jonas_Sage_Importer
{
    public partial class NominalCodeEditor : Form
    {
        private readonly string connectionString = DbConnectionsCs.ConnectionString();

        private DataSet ds = new DataSet();

        private DataSet changes;

        DbConnectionsCs dbConnections = new DbConnectionsCs();

        /*
        DataTable dt = new DataTable();
        BindingSource bindingSource = new BindingSource();
        SqlDataAdapter da = new SqlDataAdapter();
        DbConnectionsCs dbConnectionsCs = new DbConnectionsCs();
        */

        private string nCode = string.Empty;

        private string nDesc = string.Empty;



        public NominalCodeEditor()
        {
            InitializeComponent();
        }

        private void NominalCodeEditor_Load(object sender, EventArgs e)
        {

            dbConnections.GetNominalCodeAdapter().Fill(ds);
            nominalCodesGridView.DataSource = ds.Tables[0];
            

            nominalCodesGridView.Columns[1].Width = nominalCodesGridView.Width
                                                       - nominalCodesGridView.Columns[0].Width
                                                     - 21;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // this.bindingSource.Filter = $"convert(NominalCode,'System.String') like '%{this.textBox1.Text.ToString()}%' OR convert(Description,'System.String') like '%{this.textBox1.Text.ToString()}%'";
            string rowFilter = $"Convert([{"NominalCode"}],'System.String') like '%{textBox1.Text}%'";
            rowFilter += $"OR [{"Description"}] like '%{textBox1.Text}%'";

            DataTable dataTable = nominalCodesGridView.DataSource as DataTable;
            if (dataTable != null)
            {
                dataTable.DefaultView.RowFilter = rowFilter;
            }
        }

         private void ExitBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void AddNominalCodeBtnClick(object sender, EventArgs e)
        {
            nCode = nominalCodeTxtBox.Text;
            nDesc = nominalDescriptionTxtBox.Text;


            int nominalCode = nCode !=string.Empty ? int.Parse(nCode) : 0;

            string nominalDescription = nDesc;

            if (nominalCode != 0 && nominalDescription !=string.Empty)
            {


                try
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["NominalCode"] = Int32.Parse(nCode);
                    newRow["Description"] = nDesc;

                    ds.Tables[0].Rows.Add(newRow);
                    nominalCodeTxtBox.Text = "";
                    nominalDescriptionTxtBox.Text = "";

                }
                catch (SqlException sqlex)
                {
                    MessageBox.Show($"Unable to complete Update Command: \n \n {sqlex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to complete Update Command: \n \n {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show(@"Please Enter Nominal Code and Description. The Nominal Code can not be blank.");
            }


        }

        private void nominalCodeTxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (nominalCodesGridView.SelectedRows.Count > 0)
            {
                DialogResult dResult = MessageBox.Show(
                $"Are you sure you want to delete this nominal code? \nOnce it is removed you will not be able to recover this.",
                @"Are you sure?",
                MessageBoxButtons.YesNo);

                if (dResult == DialogResult.Yes)
                {
                    dbConnections.GetNominalCodeAdapter().AcceptChangesDuringUpdate = true;
                    nominalCodesGridView.Rows.RemoveAt(nominalCodesGridView.SelectedRows[0].Index);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                changes = ds.GetChanges();

                if (changes != null)
                {

                    ds.AcceptChanges();
                    dbConnections.GetNominalCodeAdapter().AcceptChangesDuringUpdate = true;
                    dbConnections.GetNominalCodeAdapter().Update(changes);
                    

                    MessageBox.Show(@"The Nominal Codes have been Updated.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Nominal Codes \n \n {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            nominalCodesGridView.Refresh();
        }

    }
}
