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

namespace QTemplates.Classes
{
    class GitHubLatestReleaseResponse
    {
        public string html_url { get; set; }

        public string tag_name { get; set; }

        public string published_at { get; set; }

        public string body { get; set; }

        public string message { get; set; }

        /// <summary>
        /// Checks to see if the release version on GitHub is newer than the current version running.
        /// </summary>
        /// <remarks>
        /// If <see cref="message"/> is not null, then we likely have a HTTP 404 Not Found for no releases found.
        /// </remarks>
        /// <returns>True if update available or false is not</returns>
        public bool IsUpdateAvailable()
        {
            if (message != null)
            {
                return false;
            }

            var gitVersion = new Version(tag_name);
            var appVersion = new Version(Application.ProductVersion);

            // Be aware that 1.0.0 is less than (<) 1.0.0.0, they are NOT equal. Use four places on GitHub.
            switch (appVersion.CompareTo(gitVersion))
            {
                default:
                case 0: // Same as
                case 1: // later than
                    return false;
                case -1: // earlier than
                    return true;
            }
        }
    }
}
