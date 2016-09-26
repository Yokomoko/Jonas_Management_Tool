using System;
using System.Windows.Forms;
using SageImporterLibrary;

namespace Jonas_Sage_Importer
{
    public partial class DatabaseConnection : Form
    {



        public DatabaseConnection()
        {
            InitializeComponent();
        }

        private void DatabaseConnection_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            DbLocationTxtBox.Text = DbConnectionsCs.DbLocationTxt();
            DbNameTxtBox.Text = DbConnectionsCs.DbNameTxt();
            UsernameTxtBox.Text = DbConnectionsCs.UserNameTxt();
            PasswordTxtBox.Text = DbConnectionsCs.PasswordTxt();
            ConnectionStringTxtBox.Text = DbConnectionsCs.ConnectionString();
            txtBoxReportServerUrl.Text = @"http://192.168.15.48/reportserver";
        }

        private void ConnTestBtn_Click(object sender, EventArgs e)
        {
            bool trialTest = false;

            Loading lS = new Loading { TopMost = true };
            lS.UpdateText($"Testing Connection\n... Please Wait... This May Take up to a Minute to Complete. ");
            lS.Show();
            lS.Update();

            while (trialTest == false)
            {
                DbConnectionsCs.TestConnection(
                    DbLocationTxtBox.Text,
                    DbNameTxtBox.Text,
                    UsernameTxtBox.Text,
                    PasswordTxtBox.Text,
                     ConnectionStatus);
                trialTest = true;
            }
            lS.Hide();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DbConnectionsCs.UpdateConnection(
                DbLocationTxtBox.Text,
                DbNameTxtBox.Text,
                UsernameTxtBox.Text,
                PasswordTxtBox.Text);
            ConnectionStringTxtBox.Text = DbConnectionsCs.ConnectionString();
        }

        private void dbConnectionExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReturnPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                ConnTestBtn.PerformClick();
            }
        }
        private void DbLocationTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            ReturnPress(sender, e);
        }

        private void UserNameTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            ReturnPress(sender, e);
        }

        private void PasswordTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            ReturnPress(sender, e);
        }

        private void DbLocationTxtBox_Validated(object sender, EventArgs e)
        {
            Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateRptServerUrl_Click(object sender, EventArgs e)
        {
            DbConnectionsCs.updateReportServerUri(txtBoxReportServerUrl.Text);
        }
    }
}
