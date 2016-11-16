using System;
using System.Windows.Forms;
using Jonas_Sage_Importer.Properties;
using SageImporterLibrary;
using System.Data.SqlClient;
using System.ComponentModel;
using BL_JonasSageImporter.Business_Layer_Classes;

namespace Jonas_Sage_Importer
{
    public partial class DatabaseConnection : Form
    {

        Loading lS = new Loading { TopMost = true };

        public DatabaseConnection()
        {
            InitializeComponent();
        }

        private void DatabaseConnection_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            DbLocationTxtBox.Text = Settings.Default.DBLocation;
            DbNameTxtBox.Text = Settings.Default.DBName;
            UsernameTxtBox.Text = Settings.Default.DBUsername;
            PasswordTxtBox.Text = DataEncryptor.DecryptStringAES(Settings.Default.DBPassword, "DBPassword");
            ConnectionStringTxtBox.Text = DbConnectionsCs.EncryptedConnectionString;
            txtBoxReportServerUrl.Text = Settings.Default.DBReportServerUrl;
        }

        private void ConnTestBtn_Click(object sender, EventArgs e)
        {

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_TestConnection);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_TestConnectionComplete);

            bg.RunWorkerAsync();
            lS.UpdateText($"Testing Connection...\n Please Wait... This May Take up to a Minute to Complete. ");
            lS.Show();
        }

        private void bg_TestConnection(object sender, DoWorkEventArgs e)
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder
            {
                ["Persist Security Info"] = false,
                ["Data Source"] = DbLocationTxtBox.Text,
                ["integrated Security"] = false,
                ["Initial Catalog"] = DbNameTxtBox.Text,
                ["User ID"] = UsernameTxtBox.Text,
                ["Password"] = PasswordTxtBox.Text
            };

            var connString = cs.ConnectionString;


            var testResult = DbConnectionsCs.TestConnection(connString);
            e.Result = testResult;
        }

        private void bg_TestConnectionComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            bool result = (bool)e.Result;

            ConnectionStatus.Text = result ? "Connection Established" : "Connection Failed";
            ConnectionStatus.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            if (!result)
            {
                lS.UpdateText("Failed... Please see the log in the install directory for further details.");
            }
            else
            {
                lS.UpdateText("Connection Established Successfully");
            }
            System.Threading.Thread.Sleep(3500);
            lS.Hide();
        }

        private void uxUpdateBtn_Click(object sender, EventArgs e)
        {
            DbConnectionsCs.UpdateConnection(
                DbLocationTxtBox.Text,
                DbNameTxtBox.Text,
                UsernameTxtBox.Text,
                PasswordTxtBox.Text);
            ConnectionStringTxtBox.Text = DbConnectionsCs.EncryptedConnectionString;
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

        private void btnUpdateRptServerUrl_Click(object sender, EventArgs e)
        {
            DbConnectionsCs.updateReportServerUri(txtBoxReportServerUrl.Text);
        }
    }
}
