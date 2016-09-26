using System;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
    public partial class Loading : Form
    {

        public Loading()
        {
            InitializeComponent();
            Shown += Loading_Shown;
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            Shown += Loading_Shown;
        }

        void Loading_Shown(object sender, EventArgs e)
        {
            Update();
        }

        public void UpdateText(string TextString)
        {
            lblConnectionTest.Text = TextString;
        }

    }
}
