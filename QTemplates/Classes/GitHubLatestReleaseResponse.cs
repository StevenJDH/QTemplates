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
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace QTemplates.Classes
{
    class GitHubLatestReleaseResponse
    {
        [JsonProperty("html_url")]
        public string ReleaseUrl { get; set; }

        [JsonProperty("tag_name")]
        public string VersionTag { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPrerelease { get; set; }

        [JsonProperty("published_at")]
        public DateTime PublishedDateTime { get; set; }

        [JsonProperty("body")]
        public string ReleaseNotes { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public bool HasError { get; set; }

        /// <summary>
        /// Checks to see if the release version on GitHub is newer than the current version running.
        /// </summary>
        /// <remarks>
        /// If <see cref="HasError"/> is true, then we likely have a HTTP 404 Not Found for no releases found.
        /// </remarks>
        /// <returns>True if update available or false is not</returns>
        public bool IsUpdateAvailable()
        {
            if (HasError || Regex.IsMatch(VersionTag, @"(\d+)\.(\d+)\.(\d+)\.(\d+)") == false)
            {
                return false;
            }

            var gitVersion = new Version(VersionTag);
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

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            if (String.IsNullOrEmpty(ErrorMessage) == false)
            {
                HasError = true;
            }
        }
    }
}
