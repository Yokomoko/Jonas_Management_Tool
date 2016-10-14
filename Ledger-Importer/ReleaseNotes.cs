using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Jonas_Sage_Importer {
    public partial class ReleaseNotes : Form {
        public ReleaseNotes() {
            InitializeComponent();
            titleLbl.Text += " to " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {

        }
    }
}
