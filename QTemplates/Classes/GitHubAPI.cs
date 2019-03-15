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
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace QTemplates.Classes
{
    class GitHubAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsUpdateAvailable()
        { 
            GitHubLatestReleaseResponse response = await GetLatestVersionAsync("StevenJDH", "QTemplates");

            if (response.message != null)
            {
                MessageBox.Show(response.message);
                return false;
            }

            var gitVersion = new Version(response.tag_name);

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            var appVersion = new Version($"{version.Major}.{version.Minor}.{version.Build}"); // Application.ProductVersion 1.0.0.0

            // Be aware that Version.Parse("1.0.0") is less than (<) Version.Parse("1.0.0.0") (i.e. are NOT equal). 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <param name="repo"></param>
        /// <returns></returns>
        private async Task<GitHubLatestReleaseResponse> GetLatestVersionAsync(string author, string repo)
        {
            string url = $"https://api.github.com/repos/{author.Trim()}/{repo.Trim()}/releases/latest";

            return await APIServiceCallAsync<GitHubLatestReleaseResponse>(url);
        }


        /// <summary>
        ///
        /// GitHub requires a User-Agent supplied in request header with all API calls or you will
        /// get a HTTP 403 Forbidden error code. See https://developer.github.com/v3/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiLink"></param>
        /// <returns></returns>
        private async Task<T> APIServiceCallAsync<T>(string apiLink)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            // 
            string userAgentFirefox = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", userAgentFirefox);
                using (HttpResponseMessage result = await client.GetAsync(apiLink))
                {
                    string jsonData = await result.Content.ReadAsStringAsync();
                    // Will provide error information if there is a problem.
                    return json.Deserialize<T>(jsonData);
                }
            }
        }
    }
}
