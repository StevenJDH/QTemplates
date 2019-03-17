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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QInterfaces;
using QTemplates.Classes;
using QTemplates.Models;
using QTemplates.Models.UnitOfWork;

namespace QTemplates
{
    public partial class FrmMain : Form
    {
        private readonly GlobalHotKey _globalHotKey;
        private bool _startupVisible;
        private readonly IUnitOfWork _unitOfWork;
        private string _lastTemplateUsed;
        private readonly Dictionary<string, IPlugin> _pluginsDictionary;
        private readonly IHost _host;

        public FrmMain()
        {
            InitializeComponent();

            _unitOfWork = new UnitOfWork(new AppDbContext());
            _globalHotKey = new GlobalHotKey();
            _startupVisible = false;
            _lastTemplateUsed = "<[ No templates used yet ]>";

            _host = new Host();
            _pluginsDictionary = PluginProvider.Instance.LoadPlugins("Plugins");
            if (_pluginsDictionary.Keys.Count > 0)
            {
                foreach (var entry in _pluginsDictionary)
                {
                    mnuTools.DropDownItems.Add(new ToolStripMenuItem(entry.Key, null, MnuPluginSelected_Click));
                }
            }
            else
            {
                mnuTools.DropDownItems.Add("::: No plugins found :::");
            }
           
            // Hotkey for showing template selector.
            _globalHotKey.AddHotKey(123, GlobalHotKey.CTRL, Keys.T, () =>
            {
                NotifyIcon1_DoubleClick(this, null);
            });
            // Hotkey for using last template automatically.
            _globalHotKey.AddHotKey(456, GlobalHotKey.CTRL + GlobalHotKey.SHIFT, Keys.T, () =>
            {
                Clipboard.SetText(_lastTemplateUsed);
                Thread.Sleep(600); // Delay to allow time for the text to set in clipboard or you only get SYN.
                SendKeys.Send("^(v)");
            });

            this.Icon = notifyIcon1.Icon;
            notifyIcon1.Visible = true;
        }

        /// <summary>
        /// Click event used by plugins, which is determined by the plugin's name. 
        /// </summary>
        /// <param name="sender">The plugin being clicked in the menu</param>
        /// <param name="e">This is not used</param>
        private void MnuPluginSelected_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem clickedMenuItem = sender as ToolStripMenuItem;
            string key = clickedMenuItem?.Text ?? "";
            
            if (_pluginsDictionary.ContainsKey(key))
            {
                IPlugin plugin = _pluginsDictionary[key];
                plugin.Initialize(_host); // Passes instance to share functionality with plugin.
                plugin.InvokeAction();
            }
        }

        /// <summary>
        /// Overrides the defaults to allow the main form to start hidden. This method is used by the framework,
        /// we do not use it directly. Instead, control it via the <see cref="_startupVisible"/> instance
        /// variable. Then once things are loaded, set this variable to true as well as the form's Visible
        /// property before trying to call show on it. Once show is called, the form's load event will be raised.
        /// </summary>
        /// <param name="value">True for the default and false for hidden startup</param>
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_startupVisible ? value : _startupVisible);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var languages = _unitOfWork.Languages.GetAll();
            foreach (var entry in languages)
            {
                cmbLang.Items.Add(entry.Name);
            }
            cmbLang.Text = "English";

            var categories = _unitOfWork.Categories.GetAll();
            foreach (var entry in categories)
            {
                cmbCategory.Items.Add(entry.Name);
            }
            cmbCategory.Text = "All";

            FilterList();
        }

        private void NotifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            // Resets form state that enables the form to load to system tray directly on first load.
            _startupVisible = true;
            this.Visible = _startupVisible;

            this.Show();
        }

        private void CmnuExit_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            _globalHotKey.RemoveHotKey(123);
            _globalHotKey.RemoveHotKey(456);
            _globalHotKey.Dispose();
            _unitOfWork.Dispose();

            // Workaround to ensures that the Application.Run() message loop in Program.cs exits when
            // form was never shown. If Close() is used in this scenario, process will remain open.
            Application.Exit(); 
        }

        private void BtnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnManage_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmManage(_unitOfWork))
            {
                frm.ShowDialog(this);
            }

            if (_unitOfWork.IsDisposed == false) // This is for when exit is clicked while the manage dialog is open.
            {
                FilterList();
            }
        }

        private void CmbLang_DropDownClosed(object sender, EventArgs e)
        {
            FilterList();
        }

        private void CmbCategory_DropDownClosed(object sender, EventArgs e)
        {
            FilterList();
        }

        /// <summary>
        /// Filters the template list based on the selected language and category chosen.
        /// </summary>
        private void FilterList()
        {
            lstTemplates.Items.Clear();

            if (cmbCategory.Text == "All")
            {
                _unitOfWork.TemplateVersions.GetVersionsWithAll()
                    .Where(v => v.Language.Name == cmbLang.Text)
                    .Select(v => v.Template.Title)
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
            else
            {
                _unitOfWork.TemplateVersions.GetVersionsWithAll()
                    .Where(v => v.Template.Category.Name == cmbCategory.Text && v.Language.Name == cmbLang.Text)
                    .Select(v => v.Template.Title)
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
        }

        private void BtnUse_Click(object sender, EventArgs e)
        {
            if (lstTemplates.Text == "")
            {
                return;
            }

            var version = _unitOfWork.TemplateVersions.GetVersionsWithAll()
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLang.Text);

            _lastTemplateUsed = version?.Message ?? "<[ Template not found ]>";
            Clipboard.SetText(_lastTemplateUsed);
            BtnHide_Click(this, EventArgs.Empty);
        }

        private void CmnuAbout_Click(object sender, EventArgs e)
        {
            using (FrmAbout frm = new FrmAbout())
            {
                frm.ShowDialog(this);
            }
        }

        private async void MnuUpdateCheck_Click(object sender, EventArgs e)
        {
            if (Connection.IsInternetAvailable() == false)
            {
                MessageBox.Show("A connection to the Internet was not detected.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GitHubLatestReleaseResponse response = await new GitHubAPI().GetLatestVersionAsync("StevenJDH", "QTemplates");

            if (response != null && response.IsUpdateAvailable())
            {
                if (MessageBox.Show($"A new version of QTemplates ({response.VersionTag}) is available! Do you want to download the update now?",
                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // Sends URL to the operating system for opening.
                        Process.Start(response.ReleaseUrl);
                    }
                    catch (Exception) {/* Consuming exceptions */ }
                }
            }
            else
            {
                MessageBox.Show($"You are using the latest version of QTemplates ({Application.ProductVersion}).",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
