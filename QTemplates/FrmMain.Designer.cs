namespace QTemplates
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.btnUse = new System.Windows.Forms.Button();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lstTemplates = new System.Windows.Forms.ListBox();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "QTemplates";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.NotifyIcon1_BalloonTipClicked);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAbout,
            this.mnuUpdateCheck,
            this.toolStripSeparator1,
            this.mnuTools,
            this.toolStripSeparator2,
            this.cmnuExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(172, 104);
            // 
            // cmnuAbout
            // 
            this.cmnuAbout.Name = "cmnuAbout";
            this.cmnuAbout.Size = new System.Drawing.Size(171, 22);
            this.cmnuAbout.Text = "About";
            this.cmnuAbout.Click += new System.EventHandler(this.CmnuAbout_Click);
            // 
            // mnuUpdateCheck
            // 
            this.mnuUpdateCheck.Name = "mnuUpdateCheck";
            this.mnuUpdateCheck.Size = new System.Drawing.Size(171, 22);
            this.mnuUpdateCheck.Text = "Check for Updates";
            this.mnuUpdateCheck.Click += new System.EventHandler(this.MnuUpdateCheck_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // mnuTools
            // 
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(171, 22);
            this.mnuTools.Text = "Tools";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // cmnuExit
            // 
            this.cmnuExit.Name = "cmnuExit";
            this.cmnuExit.Size = new System.Drawing.Size(171, 22);
            this.cmnuExit.Text = "Exit";
            this.cmnuExit.Click += new System.EventHandler(this.CmnuExit_Click);
            // 
            // cmbLang
            // 
            this.cmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(8, 24);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(256, 21);
            this.cmbLang.TabIndex = 2;
            this.cmbLang.DropDownClosed += new System.EventHandler(this.CmbLang_DropDownClosed);
            // 
            // btnUse
            // 
            this.btnUse.Location = new System.Drawing.Point(88, 392);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(96, 32);
            this.btnUse.TabIndex = 1;
            this.btnUse.Text = "Use";
            this.btnUse.UseVisualStyleBackColor = true;
            this.btnUse.Click += new System.EventHandler(this.BtnUse_Click);
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "All"});
            this.cmbCategory.Location = new System.Drawing.Point(8, 72);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(256, 21);
            this.cmbCategory.TabIndex = 3;
            this.cmbCategory.DropDownClosed += new System.EventHandler(this.CmbCategory_DropDownClosed);
            // 
            // lstTemplates
            // 
            this.lstTemplates.FormattingEnabled = true;
            this.lstTemplates.Location = new System.Drawing.Point(8, 120);
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.Size = new System.Drawing.Size(256, 264);
            this.lstTemplates.Sorted = true;
            this.lstTemplates.TabIndex = 4;
            this.lstTemplates.DoubleClick += new System.EventHandler(this.LstTemplates_DoubleClick);
            // 
            // btnHide
            // 
            this.btnHide.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHide.Location = new System.Drawing.Point(8, 392);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(72, 32);
            this.btnHide.TabIndex = 6;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.BtnHide_Click);
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(192, 392);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(72, 32);
            this.btnManage.TabIndex = 5;
            this.btnManage.Text = "Manage";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.BtnManage_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Language:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Category:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Templates:";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnHide;
            this.ClientSize = new System.Drawing.Size(274, 434);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.lstTemplates);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.cmbLang);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QTemplates";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.Button btnUse;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ListBox lstTemplates;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.ToolStripMenuItem cmnuAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmnuExit;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateCheck;
    }
}

