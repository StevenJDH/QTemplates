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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QInterfaces;

namespace QTemplates.Example.Plugin
{
    public class Program : IPlugin
    {
        private IHost _host;

        public string Title => "Example Plugin";

        public string Description => "An example plugin to show how plugins work.";

        public void Initialize(IHost host) => _host = host; // Forces the use of IHost.

        public void InvokeAction() => new FrmMain(_host).ShowDialog();
    }
}
