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
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreateVersion = new System.Windows.Forms.Button();
            this.lblLangAvailable = new System.Windows.Forms.Label();
            this.cmbLangAvailable = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnDeleteVersion = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnUpdateVersion = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnManageCategories = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnManageLanguages = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(272, 24);
            this.txtMessage.MaxLength = 2048;
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(760, 376);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.TextChanged += new System.EventHandler(this.TxtMessage_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Templates:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Category:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Language:";
            // 
            // lstTemplates
            // 
            this.lstTemplates.FormattingEnabled = true;
            this.lstTemplates.Location = new System.Drawing.Point(8, 72);
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.Size = new System.Drawing.Size(256, 264);
            this.lstTemplates.Sorted = true;
            this.lstTemplates.TabIndex = 13;
            this.lstTemplates.SelectedIndexChanged += new System.EventHandler(this.LstTemplates_SelectedIndexChanged);
            // 
            // cmbCategoryFilter
            // 
            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.FormattingEnabled = true;
            this.cmbCategoryFilter.Items.AddRange(new object[] {
            "All"});
            this.cmbCategoryFilter.Location = new System.Drawing.Point(8, 24);
            this.cmbCategoryFilter.Name = "cmbCategoryFilter";
            this.cmbCategoryFilter.Size = new System.Drawing.Size(256, 21);
            this.cmbCategoryFilter.TabIndex = 12;
            this.cmbCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.CmbCategoryFilter_SelectedIndexChanged);
            // 
            // cmbLang
            // 
            this.cmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(16, 40);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(240, 21);
            this.cmbLang.TabIndex = 11;
            this.cmbLang.DropDownClosed += new System.EventHandler(this.CmbLang_DropDownClosed);
            // 
            // btnCreate
            // 
            this.btnCreate.Enabled = false;
            this.btnCreate.Location = new System.Drawing.Point(368, 32);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(115, 32);
            this.btnCreate.TabIndex = 17;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(8, 500);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 32);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Location = new System.Drawing.Point(16, 24);
            this.txtTitle.MaxLength = 40;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(256, 20);
            this.txtTitle.TabIndex = 19;
            this.txtTitle.TextChanged += new System.EventHandler(this.TxtTitle_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "Title:";
            // 
            // btnCreateVersion
            // 
            this.btnCreateVersion.Enabled = false;
            this.btnCreateVersion.Location = new System.Drawing.Point(368, 32);
            this.btnCreateVersion.Name = "btnCreateVersion";
            this.btnCreateVersion.Size = new System.Drawing.Size(115, 32);
            this.btnCreateVersion.TabIndex = 21;
            this.btnCreateVersion.Text = "Create Version";
            this.btnCreateVersion.UseVisualStyleBackColor = true;
            this.btnCreateVersion.Click += new System.EventHandler(this.BtnCreateVersion_Click);
            // 
            // lblLangAvailable
            // 
            this.lblLangAvailable.Location = new System.Drawing.Point(8, 344);
            this.lblLangAvailable.Name = "lblLangAvailable";
            this.lblLangAvailable.Size = new System.Drawing.Size(256, 16);
            this.lblLangAvailable.TabIndex = 23;
            this.lblLangAvailable.Text = "Languages available: 0";
            // 
            // cmbLangAvailable
            // 
            this.cmbLangAvailable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLangAvailable.FormattingEnabled = true;
            this.cmbLangAvailable.Location = new System.Drawing.Point(8, 360);
            this.cmbLangAvailable.Name = "cmbLangAvailable";
            this.cmbLangAvailable.Size = new System.Drawing.Size(256, 21);
            this.cmbLangAvailable.Sorted = true;
            this.cmbLangAvailable.TabIndex = 22;
            this.cmbLangAvailable.SelectedIndexChanged += new System.EventHandler(this.CmbLangAvailable_SelectedIndexChanged);
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
            this.btnDeleteVersion.Enabled = false;
            this.btnDeleteVersion.Location = new System.Drawing.Point(624, 32);
            this.btnDeleteVersion.Name = "btnDeleteVersion";
            this.btnDeleteVersion.Size = new System.Drawing.Size(115, 32);
            this.btnDeleteVersion.TabIndex = 26;
            this.btnDeleteVersion.Text = "Delete Version";
            this.btnDeleteVersion.UseVisualStyleBackColor = true;
            this.btnDeleteVersion.Click += new System.EventHandler(this.BtnDeleteVersion_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(624, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(115, 32);
            this.btnDelete.TabIndex = 27;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(496, 32);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(115, 32);
            this.btnUpdate.TabIndex = 28;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnUpdateVersion
            // 
            this.btnUpdateVersion.Enabled = false;
            this.btnUpdateVersion.Location = new System.Drawing.Point(496, 32);
            this.btnUpdateVersion.Name = "btnUpdateVersion";
            this.btnUpdateVersion.Size = new System.Drawing.Size(115, 32);
            this.btnUpdateVersion.TabIndex = 29;
            this.btnUpdateVersion.Text = "Update Version";
            this.btnUpdateVersion.UseVisualStyleBackColor = true;
            this.btnUpdateVersion.Click += new System.EventHandler(this.BtnUpdateVersion_Click);
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
            this.tabControl1.Size = new System.Drawing.Size(762, 124);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Honeydew;
            this.tabPage1.Controls.Add(this.btnManageCategories);
            this.tabPage1.Controls.Add(this.btnUpdate);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.btnCreate);
            this.tabPage1.Controls.Add(this.cmbCategory);
            this.tabPage1.Controls.Add(this.btnDelete);
            this.tabPage1.Controls.Add(this.txtTitle);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(754, 98);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Template";
            // 
            // btnManageCategories
            // 
            this.btnManageCategories.Location = new System.Drawing.Point(276, 62);
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Size = new System.Drawing.Size(68, 24);
            this.btnManageCategories.TabIndex = 32;
            this.btnManageCategories.Text = "Manage";
            this.btnManageCategories.UseVisualStyleBackColor = true;
            this.btnManageCategories.Click += new System.EventHandler(this.BtnManageCategories_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnManageLanguages);
            this.tabPage2.Controls.Add(this.btnCreateVersion);
            this.tabPage2.Controls.Add(this.btnUpdateVersion);
            this.tabPage2.Controls.Add(this.btnDeleteVersion);
            this.tabPage2.Controls.Add(this.cmbLang);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(754, 98);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Version";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnManageLanguages
            // 
            this.btnManageLanguages.Location = new System.Drawing.Point(264, 38);
            this.btnManageLanguages.Name = "btnManageLanguages";
            this.btnManageLanguages.Size = new System.Drawing.Size(68, 24);
            this.btnManageLanguages.TabIndex = 33;
            this.btnManageLanguages.Text = "Manage";
            this.btnManageLanguages.UseVisualStyleBackColor = true;
            this.btnManageLanguages.Click += new System.EventHandler(this.BtnManageLanguages_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(272, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Message:";
            // 
            // FrmManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(1043, 541);
            this.ControlBox = false;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblLangAvailable);
            this.Controls.Add(this.cmbLangAvailable);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstTemplates);
            this.Controls.Add(this.cmbCategoryFilter);
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
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCreateVersion;
        private System.Windows.Forms.Label lblLangAvailable;
        private System.Windows.Forms.ComboBox cmbLangAvailable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnDeleteVersion;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnUpdateVersion;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnManageCategories;
        private System.Windows.Forms.Button btnManageLanguages;
    }
}