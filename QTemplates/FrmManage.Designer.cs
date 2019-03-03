namespace QTemplates
{
    partial class FrmManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTemplates = new System.Windows.Forms.ListBox();
            this.cmbCategoryVersions = new System.Windows.Forms.ComboBox();
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnNewVersion = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLangVersions = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnDeleteVersion = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.btnSaveVersionChanges = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbLangFilter = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(272, 24);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(760, 376);
            this.txtMessage.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Templates:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Category:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Language:";
            // 
            // lstTemplates
            // 
            this.lstTemplates.FormattingEnabled = true;
            this.lstTemplates.Location = new System.Drawing.Point(8, 120);
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.Size = new System.Drawing.Size(256, 264);
            this.lstTemplates.TabIndex = 13;
            this.lstTemplates.Click += new System.EventHandler(this.lstTemplates_Click);
            // 
            // cmbCategoryVersions
            // 
            this.cmbCategoryVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryVersions.FormattingEnabled = true;
            this.cmbCategoryVersions.Items.AddRange(new object[] {
            "All"});
            this.cmbCategoryVersions.Location = new System.Drawing.Point(8, 72);
            this.cmbCategoryVersions.Name = "cmbCategoryVersions";
            this.cmbCategoryVersions.Size = new System.Drawing.Size(256, 21);
            this.cmbCategoryVersions.TabIndex = 12;
            this.cmbCategoryVersions.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryVersions_SelectedIndexChanged);
            // 
            // cmbLang
            // 
            this.cmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(8, 40);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(240, 21);
            this.cmbLang.TabIndex = 11;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(360, 32);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(115, 32);
            this.btnCreate.TabIndex = 17;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(8, 496);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 32);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Location = new System.Drawing.Point(12, 20);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(264, 20);
            this.txtTitle.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "Template title:";
            // 
            // btnNewVersion
            // 
            this.btnNewVersion.Location = new System.Drawing.Point(464, 32);
            this.btnNewVersion.Name = "btnNewVersion";
            this.btnNewVersion.Size = new System.Drawing.Size(88, 32);
            this.btnNewVersion.TabIndex = 21;
            this.btnNewVersion.Text = "New Version";
            this.btnNewVersion.UseVisualStyleBackColor = true;
            this.btnNewVersion.Click += new System.EventHandler(this.btnNewVersion_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 392);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(256, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Languages available:";
            // 
            // cmbLangVersions
            // 
            this.cmbLangVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLangVersions.FormattingEnabled = true;
            this.cmbLangVersions.Location = new System.Drawing.Point(8, 408);
            this.cmbLangVersions.Name = "cmbLangVersions";
            this.cmbLangVersions.Size = new System.Drawing.Size(256, 21);
            this.cmbLangVersions.TabIndex = 22;
            this.cmbLangVersions.SelectedIndexChanged += new System.EventHandler(this.cmbLangVersions_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 16);
            this.label6.TabIndex = 25;
            this.label6.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(16, 64);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(256, 21);
            this.cmbCategory.TabIndex = 24;
            // 
            // btnDeleteVersion
            // 
            this.btnDeleteVersion.Location = new System.Drawing.Point(656, 32);
            this.btnDeleteVersion.Name = "btnDeleteVersion";
            this.btnDeleteVersion.Size = new System.Drawing.Size(91, 32);
            this.btnDeleteVersion.TabIndex = 26;
            this.btnDeleteVersion.Text = "Delete Version";
            this.btnDeleteVersion.UseVisualStyleBackColor = true;
            this.btnDeleteVersion.Click += new System.EventHandler(this.btnDeleteVersion_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(616, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 32);
            this.btnDelete.TabIndex = 27;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(488, 32);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(115, 32);
            this.btnSaveChanges.TabIndex = 28;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // btnSaveVersionChanges
            // 
            this.btnSaveVersionChanges.Location = new System.Drawing.Point(560, 32);
            this.btnSaveVersionChanges.Name = "btnSaveVersionChanges";
            this.btnSaveVersionChanges.Size = new System.Drawing.Size(88, 32);
            this.btnSaveVersionChanges.TabIndex = 29;
            this.btnSaveVersionChanges.Text = "Save Version";
            this.btnSaveVersionChanges.UseVisualStyleBackColor = true;
            this.btnSaveVersionChanges.Click += new System.EventHandler(this.btnSaveVersionChanges_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(200, 18);
            this.tabControl1.Location = new System.Drawing.Point(272, 408);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(768, 124);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Honeydew;
            this.tabPage1.Controls.Add(this.btnSaveChanges);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.btnCreate);
            this.tabPage1.Controls.Add(this.cmbCategory);
            this.tabPage1.Controls.Add(this.btnDelete);
            this.tabPage1.Controls.Add(this.txtTitle);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(760, 98);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Template";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnNewVersion);
            this.tabPage2.Controls.Add(this.btnSaveVersionChanges);
            this.tabPage2.Controls.Add(this.btnDeleteVersion);
            this.tabPage2.Controls.Add(this.cmbLang);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(760, 98);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Versions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(272, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Message:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(256, 16);
            this.label8.TabIndex = 33;
            this.label8.Text = "Language:";
            // 
            // cmbLangFilter
            // 
            this.cmbLangFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLangFilter.FormattingEnabled = true;
            this.cmbLangFilter.Items.AddRange(new object[] {
            "All"});
            this.cmbLangFilter.Location = new System.Drawing.Point(8, 24);
            this.cmbLangFilter.Name = "cmbLangFilter";
            this.cmbLangFilter.Size = new System.Drawing.Size(256, 21);
            this.cmbLangFilter.TabIndex = 32;
            // 
            // FrmManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(1034, 531);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbLangFilter);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbLangVersions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstTemplates);
            this.Controls.Add(this.cmbCategoryVersions);
            this.Controls.Add(this.txtMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Templates";
            this.Load += new System.EventHandler(this.FrmManage_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstTemplates;
        private System.Windows.Forms.ComboBox cmbCategoryVersions;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnNewVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLangVersions;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnDeleteVersion;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.Button btnSaveVersionChanges;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbLangFilter;
    }
}