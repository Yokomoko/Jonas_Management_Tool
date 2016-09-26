using System.ComponentModel;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
    partial class DatabaseConnection
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
            this.Button1 = new System.Windows.Forms.Button();
            this.DBLocLbl = new System.Windows.Forms.Label();
            this.UsernameTxtBox = new System.Windows.Forms.TextBox();
            this.DbNameTxtBox = new System.Windows.Forms.TextBox();
            this.ConnectionStringTxtBox = new System.Windows.Forms.TextBox();
            this.DbLocationTxtBox = new System.Windows.Forms.TextBox();
            this.Passwordlbl = new System.Windows.Forms.Label();
            this.ConnectionStatus = new System.Windows.Forms.Label();
            this.DbNameLbl = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.PasswordTxtBox = new System.Windows.Forms.TextBox();
            this.UserNameLbl = new System.Windows.Forms.Label();
            this.ConnTestBtn = new System.Windows.Forms.Button();
            this.dbConnectionExitBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnUpdateRptServerUrl = new System.Windows.Forms.Button();
            this.txtBoxReportServerUrl = new System.Windows.Forms.TextBox();
            this.lblRptServerUrl = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(621, 103);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(67, 20);
            this.Button1.TabIndex = 36;
            this.Button1.Text = "Update";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // DBLocLbl
            // 
            this.DBLocLbl.AutoSize = true;
            this.DBLocLbl.Location = new System.Drawing.Point(6, 25);
            this.DBLocLbl.Name = "DBLocLbl";
            this.DBLocLbl.Size = new System.Drawing.Size(103, 13);
            this.DBLocLbl.TabIndex = 26;
            this.DBLocLbl.Text = "Database Location: ";
            // 
            // UsernameTxtBox
            // 
            this.UsernameTxtBox.Location = new System.Drawing.Point(352, 22);
            this.UsernameTxtBox.Name = "UsernameTxtBox";
            this.UsernameTxtBox.Size = new System.Drawing.Size(164, 20);
            this.UsernameTxtBox.TabIndex = 31;
            // 
            // DbNameTxtBox
            // 
            this.DbNameTxtBox.Location = new System.Drawing.Point(115, 77);
            this.DbNameTxtBox.Name = "DbNameTxtBox";
            this.DbNameTxtBox.Size = new System.Drawing.Size(164, 20);
            this.DbNameTxtBox.TabIndex = 29;
            // 
            // ConnectionStringTxtBox
            // 
            this.ConnectionStringTxtBox.Location = new System.Drawing.Point(115, 103);
            this.ConnectionStringTxtBox.Name = "ConnectionStringTxtBox";
            this.ConnectionStringTxtBox.ReadOnly = true;
            this.ConnectionStringTxtBox.Size = new System.Drawing.Size(500, 20);
            this.ConnectionStringTxtBox.TabIndex = 25;
            // 
            // DbLocationTxtBox
            // 
            this.DbLocationTxtBox.Location = new System.Drawing.Point(115, 22);
            this.DbLocationTxtBox.Name = "DbLocationTxtBox";
            this.DbLocationTxtBox.Size = new System.Drawing.Size(164, 20);
            this.DbLocationTxtBox.TabIndex = 27;
            // 
            // Passwordlbl
            // 
            this.Passwordlbl.AutoSize = true;
            this.Passwordlbl.Location = new System.Drawing.Point(288, 49);
            this.Passwordlbl.Name = "Passwordlbl";
            this.Passwordlbl.Size = new System.Drawing.Size(59, 13);
            this.Passwordlbl.TabIndex = 32;
            this.Passwordlbl.Text = "Password: ";
            // 
            // ConnectionStatus
            // 
            this.ConnectionStatus.Location = new System.Drawing.Point(558, 63);
            this.ConnectionStatus.Name = "ConnectionStatus";
            this.ConnectionStatus.Size = new System.Drawing.Size(89, 12);
            this.ConnectionStatus.TabIndex = 35;
            // 
            // DbNameLbl
            // 
            this.DbNameLbl.AutoSize = true;
            this.DbNameLbl.Location = new System.Drawing.Point(19, 80);
            this.DbNameLbl.Name = "DbNameLbl";
            this.DbNameLbl.Size = new System.Drawing.Size(90, 13);
            this.DbNameLbl.TabIndex = 28;
            this.DbNameLbl.Text = "Database Name: ";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(15, 106);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(94, 13);
            this.Label3.TabIndex = 24;
            this.Label3.Text = "Connection String:";
            // 
            // PasswordTxtBox
            // 
            this.PasswordTxtBox.Location = new System.Drawing.Point(352, 45);
            this.PasswordTxtBox.Name = "PasswordTxtBox";
            this.PasswordTxtBox.Size = new System.Drawing.Size(164, 20);
            this.PasswordTxtBox.TabIndex = 33;
            this.PasswordTxtBox.UseSystemPasswordChar = true;
            // 
            // UserNameLbl
            // 
            this.UserNameLbl.AutoSize = true;
            this.UserNameLbl.Location = new System.Drawing.Point(285, 22);
            this.UserNameLbl.Name = "UserNameLbl";
            this.UserNameLbl.Size = new System.Drawing.Size(61, 13);
            this.UserNameLbl.TabIndex = 30;
            this.UserNameLbl.Text = "Username: ";
            // 
            // ConnTestBtn
            // 
            this.ConnTestBtn.Location = new System.Drawing.Point(522, 24);
            this.ConnTestBtn.Name = "ConnTestBtn";
            this.ConnTestBtn.Size = new System.Drawing.Size(89, 38);
            this.ConnTestBtn.TabIndex = 34;
            this.ConnTestBtn.Text = "Test Connection";
            this.ConnTestBtn.UseVisualStyleBackColor = true;
            this.ConnTestBtn.Click += new System.EventHandler(this.ConnTestBtn_Click);
            // 
            // dbConnectionExitBtn
            // 
            this.dbConnectionExitBtn.Location = new System.Drawing.Point(630, 231);
            this.dbConnectionExitBtn.Name = "dbConnectionExitBtn";
            this.dbConnectionExitBtn.Size = new System.Drawing.Size(75, 23);
            this.dbConnectionExitBtn.TabIndex = 37;
            this.dbConnectionExitBtn.Text = "Close";
            this.dbConnectionExitBtn.UseVisualStyleBackColor = true;
            this.dbConnectionExitBtn.Click += new System.EventHandler(this.dbConnectionExitBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DBLocLbl);
            this.groupBox1.Controls.Add(this.ConnTestBtn);
            this.groupBox1.Controls.Add(this.Button1);
            this.groupBox1.Controls.Add(this.UserNameLbl);
            this.groupBox1.Controls.Add(this.PasswordTxtBox);
            this.groupBox1.Controls.Add(this.UsernameTxtBox);
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.DbNameTxtBox);
            this.groupBox1.Controls.Add(this.DbNameLbl);
            this.groupBox1.Controls.Add(this.ConnectionStringTxtBox);
            this.groupBox1.Controls.Add(this.ConnectionStatus);
            this.groupBox1.Controls.Add(this.DbLocationTxtBox);
            this.groupBox1.Controls.Add(this.Passwordlbl);
            this.groupBox1.Location = new System.Drawing.Point(10, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(693, 141);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Connection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdateRptServerUrl);
            this.groupBox2.Controls.Add(this.txtBoxReportServerUrl);
            this.groupBox2.Controls.Add(this.lblRptServerUrl);
            this.groupBox2.Location = new System.Drawing.Point(10, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(693, 71);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Credentials";
            // 
            // btnUpdateRptServerUrl
            // 
            this.btnUpdateRptServerUrl.Location = new System.Drawing.Point(622, 26);
            this.btnUpdateRptServerUrl.Name = "btnUpdateRptServerUrl";
            this.btnUpdateRptServerUrl.Size = new System.Drawing.Size(67, 20);
            this.btnUpdateRptServerUrl.TabIndex = 2;
            this.btnUpdateRptServerUrl.Text = "Update";
            this.btnUpdateRptServerUrl.UseVisualStyleBackColor = true;
            this.btnUpdateRptServerUrl.Click += new System.EventHandler(this.btnUpdateRptServerUrl_Click);
            // 
            // txtBoxReportServerUrl
            // 
            this.txtBoxReportServerUrl.Location = new System.Drawing.Point(115, 26);
            this.txtBoxReportServerUrl.Name = "txtBoxReportServerUrl";
            this.txtBoxReportServerUrl.Size = new System.Drawing.Size(500, 20);
            this.txtBoxReportServerUrl.TabIndex = 1;
            this.txtBoxReportServerUrl.Text = "http://192.168.15.48/reportserver";
            this.txtBoxReportServerUrl.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblRptServerUrl
            // 
            this.lblRptServerUrl.AutoSize = true;
            this.lblRptServerUrl.Location = new System.Drawing.Point(9, 29);
            this.lblRptServerUrl.Name = "lblRptServerUrl";
            this.lblRptServerUrl.Size = new System.Drawing.Size(98, 13);
            this.lblRptServerUrl.TabIndex = 0;
            this.lblRptServerUrl.Text = "Report Server URL";
            // 
            // DatabaseConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(712, 273);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dbConnectionExitBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseConnection";
            this.Text = "Connection Settings";
            this.Load += new System.EventHandler(this.DatabaseConnection_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Button Button1;
        internal Label DBLocLbl;
        internal TextBox UsernameTxtBox;
        internal TextBox DbNameTxtBox;
        internal TextBox ConnectionStringTxtBox;
        internal TextBox DbLocationTxtBox;
        internal Label Passwordlbl;
        internal Label ConnectionStatus;
        internal Label DbNameLbl;
        internal Label Label3;
        internal TextBox PasswordTxtBox;
        internal Label UserNameLbl;
        internal Button ConnTestBtn;
        private Button dbConnectionExitBtn;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox txtBoxReportServerUrl;
        private Label lblRptServerUrl;
        private Button btnUpdateRptServerUrl;
    }
}