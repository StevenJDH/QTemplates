namespace QTemplates
{
    partial class FrmSpellChecker
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
            this.txtOriginal = new System.Windows.Forms.TextBox();
            this.txtReplaceWith = new System.Windows.Forms.TextBox();
            this.lstSuggestions = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDictionaries = new System.Windows.Forms.ComboBox();
            this.sbSpellStatus = new System.Windows.Forms.StatusBar();
            this.statusPaneWord = new System.Windows.Forms.StatusBarPanel();
            this.statusPaneCount = new System.Windows.Forms.StatusBarPanel();
            this.statusPaneIndex = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOriginal
            // 
            this.txtOriginal.BackColor = System.Drawing.SystemColors.Window;
            this.txtOriginal.HideSelection = false;
            this.txtOriginal.Location = new System.Drawing.Point(8, 24);
            this.txtOriginal.Multiline = true;
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.ReadOnly = true;
            this.txtOriginal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOriginal.Size = new System.Drawing.Size(376, 144);
            this.txtOriginal.TabIndex = 6;
            // 
            // txtReplaceWith
            // 
            this.txtReplaceWith.Location = new System.Drawing.Point(8, 192);
            this.txtReplaceWith.Name = "txtReplaceWith";
            this.txtReplaceWith.Size = new System.Drawing.Size(376, 20);
            this.txtReplaceWith.TabIndex = 3;
            // 
            // lstSuggestions
            // 
            this.lstSuggestions.FormattingEnabled = true;
            this.lstSuggestions.Location = new System.Drawing.Point(8, 240);
            this.lstSuggestions.Name = "lstSuggestions";
            this.lstSuggestions.Size = new System.Drawing.Size(376, 108);
            this.lstSuggestions.TabIndex = 4;
            this.lstSuggestions.SelectedIndexChanged += new System.EventHandler(this.LstSuggestions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Not in dictionary:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Replace with:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Suggestions:";
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(392, 24);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(96, 32);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Location = new System.Drawing.Point(392, 64);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(96, 32);
            this.btnIgnore.TabIndex = 1;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.BtnIgnore_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(392, 352);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 364);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Dictionary language:";
            // 
            // cmbDictionaries
            // 
            this.cmbDictionaries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDictionaries.FormattingEnabled = true;
            this.cmbDictionaries.Location = new System.Drawing.Point(112, 360);
            this.cmbDictionaries.Name = "cmbDictionaries";
            this.cmbDictionaries.Size = new System.Drawing.Size(272, 21);
            this.cmbDictionaries.TabIndex = 5;
            this.cmbDictionaries.DropDownClosed += new System.EventHandler(this.CmbDictionaries_DropDownClosed);
            this.cmbDictionaries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmbDictionaries_KeyDown);
            // 
            // sbSpellStatus
            // 
            this.sbSpellStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sbSpellStatus.Location = new System.Drawing.Point(0, 401);
            this.sbSpellStatus.Name = "sbSpellStatus";
            this.sbSpellStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusPaneWord,
            this.statusPaneCount,
            this.statusPaneIndex});
            this.sbSpellStatus.ShowPanels = true;
            this.sbSpellStatus.Size = new System.Drawing.Size(495, 16);
            this.sbSpellStatus.SizingGrip = false;
            this.sbSpellStatus.TabIndex = 15;
            // 
            // statusPaneWord
            // 
            this.statusPaneWord.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusPaneWord.Name = "statusPaneWord";
            this.statusPaneWord.Width = 315;
            // 
            // statusPaneCount
            // 
            this.statusPaneCount.Name = "statusPaneCount";
            this.statusPaneCount.Text = "Word: 0 of 0";
            // 
            // statusPaneIndex
            // 
            this.statusPaneIndex.Name = "statusPaneIndex";
            this.statusPaneIndex.Text = "Index: 0";
            this.statusPaneIndex.Width = 80;
            // 
            // FrmSpellChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 417);
            this.Controls.Add(this.sbSpellStatus);
            this.Controls.Add(this.cmbDictionaries);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstSuggestions);
            this.Controls.Add(this.txtReplaceWith);
            this.Controls.Add(this.txtOriginal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSpellChecker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QSpell Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSpellChecker_FormClosing);
            this.Load += new System.EventHandler(this.FrmSpellChecker_Load);
            this.Shown += new System.EventHandler(this.FrmSpellChecker_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPaneIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOriginal;
        private System.Windows.Forms.TextBox txtReplaceWith;
        private System.Windows.Forms.ListBox lstSuggestions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDictionaries;
        private System.Windows.Forms.StatusBar sbSpellStatus;
        private System.Windows.Forms.StatusBarPanel statusPaneWord;
        private System.Windows.Forms.StatusBarPanel statusPaneCount;
        private System.Windows.Forms.StatusBarPanel statusPaneIndex;
    }
}