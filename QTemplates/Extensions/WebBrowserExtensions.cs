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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTemplates.Extensions
{
    /// <summary>
    /// Useful extensions to overcome some missing functionality and limitations while keeping then code clean.
    /// </summary>
    public static class WebBrowserExtensions
    {
        /// <summary>
        /// Sets the HTML content displayed in the <see cref="T:System.Windows.Forms.WebBrowser" /> control using a string.
        /// </summary>
        /// <param name="wb">The object where this extension will be attached.</param>
        /// <param name="html">A string containing HTML content.</param>
        /// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Windows.Forms.WebBrowser" /> instance is no longer valid.</exception>
        public static void NavigateToString(this WebBrowser wb, string html)
        {
            wb.Refresh();
            wb.DocumentText = html;
        }
    }
}
