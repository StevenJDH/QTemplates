﻿/**
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
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using QTemplates.Classes;

namespace QTemplates
{
    public partial class FrmAbout : Form
    {
        private readonly ILogger _logger;

        public FrmAbout()
        {
            InitializeComponent();
            lblButton.BackColor = Color.Transparent;
            _logger = AppConfiguration.Instance.AppLogger;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LblButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Sends URL to the operating system for opening.
                Process.Start("https://www.paypal.me/stevenjdh");
            }
            catch (Exception ex)
            {
                // Consuming exceptions
                _logger.Error(ex, "Got exception.");
            }
        }

        private void PnlButtonImage_Click(object sender, EventArgs e)
        {
            LblButton_Click(this, EventArgs.Empty);
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            // Automatically sets title and version information in label.
            lblTitleVer.Text = $"{Application.ProductName} v{Application.ProductVersion}";

            // We store the actual link this way in case we ever want to make changes to the link label.
            lnkGitHub.Links.Add(new LinkLabel.Link() { LinkData = "https://github.com/StevenJDH/QTemplates" });
        }

        private void LnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Casts URL back to string and sends it to the operating system for opening.
                Process.Start(e.Link.LinkData.ToString());
            }
            catch (Exception ex)
            {
                // Consuming exceptions
                _logger.Error(ex, "Got exception.");
            }
        }
    }
}
