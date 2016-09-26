using System.ComponentModel;
using System.Windows.Forms;

namespace Jonas_Sage_Importer
{
    partial class NominalCodeEditor
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.nominalCodesGridView = new System.Windows.Forms.DataGridView();
            this.nominalCodeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblFilter = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.exitBtn = new System.Windows.Forms.Button();
            this.nominalCodeLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nominalCodeTxtBox = new System.Windows.Forms.TextBox();
            this.nominalDescriptionTxtBox = new System.Windows.Forms.TextBox();
            this.addNominalCodeBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nominalCodesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nominalCodeBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // nominalCodesGridView
            // 
            this.nominalCodesGridView.AllowUserToAddRows = false;
            this.nominalCodesGridView.AllowUserToResizeColumns = false;
            this.nominalCodesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.nominalCodesGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.nominalCodesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.nominalCodesGridView.Location = new System.Drawing.Point(3, 24);
            this.nominalCodesGridView.Name = "nominalCodesGridView";
            this.nominalCodesGridView.RowHeadersVisible = false;
            this.nominalCodesGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.nominalCodesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.nominalCodesGridView.Size = new System.Drawing.Size(271, 282);
            this.nominalCodesGridView.TabIndex = 0;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(6, 315);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(35, 13);
            this.lblFilter.TabIndex = 2;
            this.lblFilter.Text = "Filter: ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(41, 312);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(232, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(470, 413);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 5;
            this.exitBtn.Text = "Close";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.ExitBtnClick);
            // 
            // nominalCodeLbl
            // 
            this.nominalCodeLbl.AutoSize = true;
            this.nominalCodeLbl.Location = new System.Drawing.Point(6, 25);
            this.nominalCodeLbl.Name = "nominalCodeLbl";
            this.nominalCodeLbl.Size = new System.Drawing.Size(79, 13);
            this.nominalCodeLbl.TabIndex = 6;
            this.nominalCodeLbl.Text = "Nominal Code: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nominal Code Description:";
            // 
            // nominalCodeTxtBox
            // 
            this.nominalCodeTxtBox.Location = new System.Drawing.Point(9, 43);
            this.nominalCodeTxtBox.Name = "nominalCodeTxtBox";
            this.nominalCodeTxtBox.Size = new System.Drawing.Size(211, 20);
            this.nominalCodeTxtBox.TabIndex = 8;
            this.nominalCodeTxtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nominalCodeTxtBox_KeyPress);
            // 
            // nominalDescriptionTxtBox
            // 
            this.nominalDescriptionTxtBox.Location = new System.Drawing.Point(9, 91);
            this.nominalDescriptionTxtBox.Name = "nominalDescriptionTxtBox";
            this.nominalDescriptionTxtBox.Size = new System.Drawing.Size(211, 20);
            this.nominalDescriptionTxtBox.TabIndex = 9;
            // 
            // addNominalCodeBtn
            // 
            this.addNominalCodeBtn.Location = new System.Drawing.Point(9, 121);
            this.addNominalCodeBtn.Name = "addNominalCodeBtn";
            this.addNominalCodeBtn.Size = new System.Drawing.Size(211, 23);
            this.addNominalCodeBtn.TabIndex = 10;
            this.addNominalCodeBtn.Text = "Add";
            this.addNominalCodeBtn.UseVisualStyleBackColor = true;
            this.addNominalCodeBtn.Click += new System.EventHandler(this.AddNominalCodeBtnClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nominalCodeLbl);
            this.groupBox1.Controls.Add(this.addNominalCodeBtn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nominalDescriptionTxtBox);
            this.groupBox1.Controls.Add(this.nominalCodeTxtBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 161);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Nominal Code";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.nominalCodesGridView);
            this.groupBox2.Controls.Add(this.lblFilter);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(266, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 398);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Nominal Codes";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(198, 352);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 36);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(9, 351);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 37);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(103, 352);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 37);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete Selected";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // NominalCodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 444);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.exitBtn);
            this.Name = "NominalCodeEditor";
            this.Text = "Nominal Code Editor";
            this.Load += new System.EventHandler(this.NominalCodeEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nominalCodesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nominalCodeBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
             
        }

        #endregion

        private DataGridView nominalCodesGridView;
        private BindingSource nominalCodeBindingSource;
        private Label lblFilter;
        private TextBox textBox1;
        private Button exitBtn;
        private Label nominalCodeLbl;
        private Label label1;
        private TextBox nominalCodeTxtBox;
        private TextBox nominalDescriptionTxtBox;
        private Button addNominalCodeBtn;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnSave;
        private Button btnDelete;
        private Button btnRefresh;
    }
}