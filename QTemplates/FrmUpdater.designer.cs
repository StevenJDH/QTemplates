namespace QTemplates
{
    partial class FrmUpdater
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
            this.wbReleaseNotes = new System.Windows.Forms.WebBrowser();
            this.lblVersion = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnManualUpdate = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnLater = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // wbReleaseNotes
            // 
            this.wbReleaseNotes.AllowWebBrowserDrop = false;
            this.wbReleaseNotes.IsWebBrowserContextMenuEnabled = false;
            this.wbReleaseNotes.Location = new System.Drawing.Point(79, 121);
            this.wbReleaseNotes.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbReleaseNotes.Name = "wbReleaseNotes";
            this.wbReleaseNotes.ScriptErrorsSuppressed = true;
            this.wbReleaseNotes.Size = new System.Drawing.Size(480, 232);
            this.wbReleaseNotes.TabIndex = 14;
            this.wbReleaseNotes.WebBrowserShortcutsEnabled = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(79, 41);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(480, 40);
            this.lblVersion.TabIndex = 12;
            this.lblVersion.Text = "Version x.x.x.x is now available. You have version x.x.x.x installed. Would you l" +
    "ike to update it now?";
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(79, 97);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(480, 23);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "What\'s new:";
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(79, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(480, 23);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "A new version of QTemplates is available!";
            // 
            // pbDownload
            // 
            this.pbDownload.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pbDownload.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pbDownload.Location = new System.Drawing.Point(79, 361);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(480, 23);
            this.pbDownload.Step = 1;
            this.pbDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbDownload.TabIndex = 8;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Image = global::QTemplates.Properties.Resources.download;
            this.btnUpdate.Location = new System.Drawing.Point(448, 393);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(114, 31);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnManualUpdate
            // 
            this.btnManualUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualUpdate.Image = global::QTemplates.Properties.Resources.hand_point;
            this.btnManualUpdate.Location = new System.Drawing.Point(80, 392);
            this.btnManualUpdate.Name = "btnManualUpdate";
            this.btnManualUpdate.Size = new System.Drawing.Size(120, 31);
            this.btnManualUpdate.TabIndex = 16;
            this.btnManualUpdate.Text = "Manually Update";
            this.btnManualUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnManualUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManualUpdate.UseVisualStyleBackColor = true;
            this.btnManualUpdate.Click += new System.EventHandler(this.BtnManualUpdate_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::QTemplates.Properties.Resources.update_logo;
            this.pbLogo.Location = new System.Drawing.Point(7, 9);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(64, 64);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 15;
            this.pbLogo.TabStop = false;
            // 
            // btnLater
            // 
            this.btnLater.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLater.Image = global::QTemplates.Properties.Resources.clock_go;
            this.btnLater.Location = new System.Drawing.Point(328, 392);
            this.btnLater.Name = "btnLater";
            this.btnLater.Size = new System.Drawing.Size(115, 31);
            this.btnLater.TabIndex = 13;
            this.btnLater.Text = "Remind Me Later";
            this.btnLater.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLater.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLater.UseVisualStyleBackColor = true;
            this.btnLater.Click += new System.EventHandler(this.BtnLater_Click);
            // 
            // FrmUpdater
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 432);
            this.Controls.Add(this.btnManualUpdate);
            this.Controls.Add(this.wbReleaseNotes);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.btnLater);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.pbDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QTemplates Updater";
            this.Load += new System.EventHandler(this.FrmUpdater_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.WebBrowser wbReleaseNotes;
        internal System.Windows.Forms.PictureBox pbLogo;
        internal System.Windows.Forms.Button btnLater;
        internal System.Windows.Forms.Label lblVersion;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnUpdate;
        internal System.Windows.Forms.ProgressBar pbDownload;
        internal System.Windows.Forms.Button btnManualUpdate;
    }
}

