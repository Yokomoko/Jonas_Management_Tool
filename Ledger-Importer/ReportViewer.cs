using System;
using System.Windows.Forms;
using Jonas_Sage_Importer.Properties;

namespace Jonas_Sage_Importer
{
    public partial class ReportViewer : Form
    {
        readonly Uri reportServerPath = new Uri($"{Settings.Default.DBReportServerUrl}");
        string reportName = String.Empty;
        

        public ReportViewer()
        {
            InitializeComponent();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            Text = $"{reportName.Replace("/","")}";
           
             reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            reportViewer1.ShowCredentialPrompts = true;
            reportViewer1.ServerReport.ReportServerUrl = reportServerPath;
            reportViewer1.ServerReport.ReportPath = reportName;
            reportViewer1.Refresh();
        }

        public void ReportServerPathName(string reportPath)
        {
            reportName = reportPath;
        }


    }
}
