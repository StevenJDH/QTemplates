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
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using QInterfaces;
using QTemplates.Classes;
using QTemplates.Classes.Interfaces;
using QTemplates.Models;
using QTemplates.Models.UnitOfWork;

namespace QTemplates
{
    public partial class FrmMain : Form
    {
        private readonly GlobalHotKey _globalHotKey;
        private bool _startupVisible;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Dictionary<string, IPlugin> _pluginsDictionary;
        private readonly IHost _host;
        private GitHubLatestReleaseResponse _latestReleaseInfo;
        private const int CP_DISABLED_CLOSE_BUTTON = 0x200;
        private readonly ISendable _keyboardInput;
        private readonly ILogger _logger;

        public FrmMain()
        {
            InitializeComponent();

            _unitOfWork = new UnitOfWork(new AppDbContext());
            _globalHotKey = new GlobalHotKey();
            _startupVisible = false;
            _keyboardInput = new KeyboardSimulator();
            _logger = AppConfiguration.Instance.AppLogger;

            _host = new Host();
            _pluginsDictionary = PluginProvider.Instance.LoadPlugins("Plugins");
            _logger.Debug($"Plugins loaded: {_pluginsDictionary.Keys.Count}");
            if (_pluginsDictionary.Keys.Count > 0)
            {
                foreach (var entry in _pluginsDictionary)
                {
                    mnuTools.DropDownItems.Add(new ToolStripMenuItem(entry.Key, null, MnuPluginSelected_Click)
                    {
                        ToolTipText = entry.Value.Description
                    });
                }
            }
            else
            {
                mnuTools.DropDownItems.Add("::: No plugins found :::");
            }
           
            // Hotkey for showing template selector.
            _globalHotKey.AddHotKey(123, GlobalHotKey.NOREPEAT + GlobalHotKey.CTRL, Keys.T, () =>
            {
                if (IsFormOpen(typeof(FrmMain)))
                {
                    return;
                }
                _keyboardInput.HookWindow();

                // Resets form state that enables the form to load to system tray directly on first load.
                _startupVisible = true;
                this.Visible = _startupVisible;

                btnUse.Focus();
                lstTemplates.ClearSelected();
                this.Show();
                this.Focus();

                // Ensures selection window appears as the active window.
                this.TopMost = true;
                this.TopMost = false;
            });
            // Hotkey for using last template automatically.
            _globalHotKey.AddHotKey(456, GlobalHotKey.NOREPEAT + GlobalHotKey.CTRL + GlobalHotKey.SHIFT, Keys.T, () =>
            {
                _keyboardInput.SendTextAgain();
            });

            this.Icon = notifyIcon1.Icon;
            notifyIcon1.Visible = true;
            Task.Run(() => BackgroundUpdateCheck());
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
            var languages = _unitOfWork.Languages.GetAll()
                .OrderBy(l => l.Name)
                .Select(l => l.Name)
                .ToList();
            foreach (var language in languages)
            {
                cmbLang.Items.Add(language);
            }
            cmbLang.Text = "English";

            var categories = _unitOfWork.Categories.GetAll()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToList();
            foreach (var category in categories)
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.Text = "All";

            FilterList();
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
                cmbLang.Items.Clear();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All");
                FrmMain_Load(this, EventArgs.Empty);
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

            BtnHide_Click(this, EventArgs.Empty);

            try
            {
                _keyboardInput.SendText(version?.Message ?? "<[ Template not found ]>");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is Win32Exception)
            {
                _logger.Error(ex, "Got exception.");
                MessageBox.Show($"Error: {ex.Message} Use Ctrl+Shift+T to inject selected template manually.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Removes the hook since template selection window is now closed.
            _keyboardInput.ReleaseWindow();
        }

        private void CmnuAbout_Click(object sender, EventArgs e)
        {
            if (IsFormOpen(typeof(FrmAbout)) == false)
            {
                using (FrmAbout frm = new FrmAbout())
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private async void MnuUpdateCheck_Click(object sender, EventArgs e)
        {
            if (Connection.IsInternetAvailable() == false)
            {
                _logger.Warn("A connection to the Internet was not detected.");
                MessageBox.Show("A connection to the Internet was not detected.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GitHubLatestReleaseResponse response;
            try
            {
                response = await new GitHubAPI().GetLatestVersionAsync("StevenJDH", "QTemplates");
            }
            catch (Exception ex)
            {
                // HTTP status ode 404 can indicate there are no pre/releases available yet, so we want to
                // show as current version by setting response to null when we get one.
                if (!(ex is HttpRequestException) && ex.Message.Contains("status code 404") == false)
                {
                    _logger.Error(ex, "Got exception.");
                    MessageBox.Show("Error: Could not connect to GitHub's servers. Please check your connection or try again later.",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _logger.Warn(ex, "Possible exception or no pre/releases available yet.");
                response = null;
            }

            if (response?.IsUpdateAvailable() == true)
            {
                if (IsFormOpen(typeof(FrmUpdater)) == false)
                {
                    using (var frm = new FrmUpdater(response, this.Icon))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            else
            {
                MessageBox.Show($"You are using the latest version of QTemplates ({Application.ProductVersion}).",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// For use by a separate thread other than the UI thread to check for updates in the background.
        /// </summary>
        private void BackgroundUpdateCheck()
        {
            if (Connection.IsInternetAvailable() == false)
            {
                _logger.Warn("A connection to the Internet was not detected.");
                return;
            }

            try
            {
                _latestReleaseInfo = new GitHubAPI().GetLatestVersionAsync("StevenJDH", "QTemplates").Result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Got exception.");
                // Returns here no matter what Exception is thrown as we can get an aggregated
                // exception unlike a manual update check.
                return;
            }

            if (_latestReleaseInfo?.IsUpdateAvailable() == true)
            {
                notifyIcon1.ShowBalloonTip(30000, "Update Available", 
                    $"A new version of QTemplates ({_latestReleaseInfo.VersionTag}) is available! Click this notification for more information.", ToolTipIcon.None);
            }
            else
            {
                _latestReleaseInfo = null;
                notifyIcon1.ShowBalloonTip(1000, "Up-to-Date", 
                    $"You are using the latest version of QTemplates ({Application.ProductVersion}).", ToolTipIcon.None);
            }
        }

        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (_latestReleaseInfo != null && IsFormOpen(typeof(FrmUpdater)) == false)
            {
                using (var frm = new FrmUpdater(_latestReleaseInfo, this.Icon))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void LstTemplates_DoubleClick(object sender, EventArgs e)
        {
            BtnUse_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disables the close button in the title bar of the form because the form's closing event only
        /// runs if it was opened at least once, and we can't have two places wrapping things up.
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

        /// <summary>
        /// Checks to see if a particular form is open, but does not include hidden forms.
        /// </summary>
        /// <param name="form">Form to check for</param>
        /// <returns>True if open, false if not</returns>
        /// <example>
        /// <code>
        /// if (IsFormOpen(typeof(FrmMain)))
        /// {
        ///    // ....
        /// }
        /// </code>
        /// </example>
        public bool IsFormOpen(Type form)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType().Name == form.Name && openForm.Visible)
                {
                    return true;
                }
            }
            return false;
        }

        private void CmbLang_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevents mouse scrolling via mouse wheel and arrow keys.
            e.Handled = true;
        }

        private void CmbCategory_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            this.Focus();
        }
    }
}
