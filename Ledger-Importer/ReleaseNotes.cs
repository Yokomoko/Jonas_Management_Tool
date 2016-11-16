using System;
using System.Reflection;
using System.Windows.Forms;

namespace Jonas_Sage_Importer {
    public partial class ReleaseNotes : Form {
        public ReleaseNotes() {
            InitializeComponent();
            titleLbl.Text += " to " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {

        }
    }
}
