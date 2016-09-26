using System.ComponentModel;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
    partial class Loading
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
            this.lblConnectionTest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblConnectionTest
            // 
            this.lblConnectionTest.AutoSize = true;
            this.lblConnectionTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblConnectionTest.Location = new System.Drawing.Point(-1, 0);
            this.lblConnectionTest.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnectionTest.Name = "lblConnectionTest";
            this.lblConnectionTest.Size = new System.Drawing.Size(211, 26);
            this.lblConnectionTest.TabIndex = 0;
            this.lblConnectionTest.Text = "Testing Connection ... Please Wait ...\r\nThis May Take up to a Minute to Complete." +
    "\r\n";
            this.lblConnectionTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(222, 35);
            this.ControlBox = false;
            this.Controls.Add(this.lblConnectionTest);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Loading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            this.Load += new System.EventHandler(this.Loading_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblConnectionTest;
    }
}