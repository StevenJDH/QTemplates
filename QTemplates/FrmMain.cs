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
using System.Text;
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
        private bool _startupHidden;
        private readonly IUnitOfWork _unitOfWork;
        private string _lastTemplateUsed;

        private readonly Dictionary<string, IPlugin> _pluginsDictionary;
        private readonly IHost _host;

        public FrmMain()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork(new AppDbContext());
            _globalHotKey = new GlobalHotKey();
            _startupHidden = false;
            _lastTemplateUsed = "<[ No templates used yet ]>";

            _host = new Host();
            _pluginsDictionary = PluginProvider.Instance.LoadPlugins("Plugins");
            foreach (var entry in _pluginsDictionary)
            {
                mnuTools.DropDownItems.Add(new ToolStripMenuItem(entry.Key, null, mnuPluginSelected_Click));
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
                SendKeys.Send("^(v)");
            });

            //notifyIcon1.Icon = this.Icon;
            notifyIcon1.Visible = true;
        }

        private void mnuPluginSelected_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem clickedPlugin = sender as ToolStripMenuItem;
            string key = clickedPlugin?.Text ?? "";

            if (_pluginsDictionary.ContainsKey(key))
            {
                IPlugin plugin = _pluginsDictionary[key];
                plugin.Initialize(_host);
                MessageBox.Show($"{plugin.Title}: {plugin.Description}");
                plugin.InvokeAction();
            }
        }

        /// <summary>
        /// Overrides the defaults to allow the main form to start hidden. This method is used by the
        /// framework, we do not use it directly. Instead, control it via the _startupHidden instance
        /// variable. Then once things are loaded, set this variable to true as well as the form's
        /// Visible property before trying to call show on it. Once show is called, the from's load
        /// event will be raised.
        /// </summary>
        /// <param name="value">True for the default and false for hidden startup</param>
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_startupHidden ? value : _startupHidden);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Loading the template list is not needed because the cmbCategoryVersions index change event will do it.

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
        }

        private void FrmMain_Closing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
            _globalHotKey.RemoveHotKey(123);
            _globalHotKey.Dispose();
            _unitOfWork.Dispose();
        }

        private void NotifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            // Resets form state that enables it to load to system tray cleanly.
            _startupHidden = true;
            this.Visible = _startupHidden;
            this.Show();
        }

        private void CmnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnManage_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmManage(_unitOfWork))
            {
                frm.ShowDialog();
            }
        }

        private void cmbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTemplates.Items.Clear();

            _unitOfWork.Versions.GetVersionsWithAll()
                .Where(v => v.Language.Name == cmbLang.Text)
                .Select(v => v.Template.Title)
                .Distinct()
                .ToList()
                .ForEach(t => lstTemplates.Items.Add(t));
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTemplates.Items.Clear();

            if (cmbCategory.Text == "All")
            {
                _unitOfWork.Templates.GetAll()
                    .Select(t => t.Title)
                    .Distinct()
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
            else
            {
                _unitOfWork.Templates.GetAll()
                    .Where(c => c.Category.Name == cmbCategory.Text)
                    .Select(t => t.Title)
                    .Distinct()
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            if (lstTemplates.Text == "")
            {
                return;
            }

            var version = _unitOfWork.Versions.GetVersionsWithAll()
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLang.Text);

            _lastTemplateUsed = version?.Message ?? "<[ Template not found ]>";
            Clipboard.SetText(version?.Message ?? "<[ Template not found ]>");

            BtnHide_Click(this, EventArgs.Empty);
        }
    }
}
