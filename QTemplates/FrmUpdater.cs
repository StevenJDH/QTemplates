/**
 * This file is part of QTemplates <https://github.com/StevenJDH/QTemplates>.
 * Copyright (C) 2019 Steven Jenkins De Haro.
 *
 * QTemplates is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * QTemplates is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with QTemplates.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QTemplates.Classes;
using QTemplates.Extensions;

namespace QTemplates
{
    public partial class FrmUpdater : Form 
    {
        private readonly GitHubLatestReleaseResponse _response;
        private readonly string _tempUpdateFile;
        private const int CP_DISABLED_CLOSE_BUTTON = 0x200;

        public FrmUpdater(GitHubLatestReleaseResponse response, Icon frmIcon)
        {
            InitializeComponent();
            _response = response;
            this.Icon = frmIcon;
            _tempUpdateFile = Path.Combine(Path.GetTempPath(), $"QUpdate-{Guid.NewGuid().ToString("N")}.exe");
        }

        private void BtnLater_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void FrmUpdater_Load(object sender, EventArgs e)
        {
            var changelog = new Changelog();

            wbReleaseNotes.NavigateToString(changelog.GetLoadingHTML());
            lblVersion.Text = $"Version {_response.VersionTag} is now available. You have version " +
                              $"{Application.ProductVersion} installed. Would you like to update it now?";
            await Task.Run(() =>
            {
                wbReleaseNotes.NavigateToString(changelog.GetHTMLVersion(_response.VersionTag, 
                    _response.ReleaseNotes));
            });
        }

        private void BtnManualUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Sends URL to the operating system for opening.
                Process.Start(_response.ReleaseUrl);
            }
            catch (Exception) {/* Consuming exceptions */ }

            this.Close();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update now?", Application.ProductName, 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            btnLater.Enabled = false;
            btnUpdate.Enabled = false;
            btnManualUpdate.Enabled = false;

            var client = new WebClient();
            client.DownloadProgressChanged += DownloadProgressChanged;
            client.DownloadFileCompleted += DownloadFileCompleted;

            if (File.Exists(_tempUpdateFile))
            {
                File.Delete(_tempUpdateFile);
            }

            var releaseAsset = _response.Assets.FirstOrDefault(r => r.ContentType == "application/x-msdownload");
            if (releaseAsset != null)
            {
                client.DownloadFileAsync(new Uri(releaseAsset.DownloadUrl), _tempUpdateFile);
            }
            else
            {
                MessageBox.Show("Could not find a setup file to download.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            IncrementProgressNoAnimation(pbDownload, e.ProgressPercentage);
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"Error: {e.Error.Message}",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbDownload.Value = 0;
                btnLater.Enabled = true;
                btnUpdate.Enabled = true;
                btnManualUpdate.Enabled = true;
                if (File.Exists(_tempUpdateFile))
                {
                    File.Delete(_tempUpdateFile);
                }
            }
            else
            {
                MessageBox.Show("Download complete. The updating process will now start.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(_tempUpdateFile);
                Application.Exit();
            }
        }

        /// <summary>
        /// Increments a progress bar and speeds up the aero animation to effectively disable 
        /// it so that it is more responsive during value changes.
        /// </summary>
        /// <param name="pb">The progress bar to increment</param>
        /// <param name="progress">The progress value to set</param>
        private void IncrementProgressNoAnimation(ProgressBar pb, int progress)
        {
            pb.Value = progress;

            // Prevents the animation by moving the progress bar backwards.
            if (pb.Value == pb.Maximum) // Special case, can't set value > Maximum.
            {
                pb.Maximum += 1;
                pb.Value += 1; // Moves past
                pb.Value -= 1; // and back to set correct value
                pb.Maximum -= 1;
            }
            else
            {
                pb.Value += 1; // Moves past
                pb.Value -= 1; // and back to set correct value
            }
        }

        /// <summary>
        /// Disables the close button in the title bar of the form.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ClassStyle |= CP_DISABLED_CLOSE_BUTTON;
                return param;
            }
        }
    }
}
