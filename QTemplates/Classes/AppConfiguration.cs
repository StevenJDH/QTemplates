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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTemplates.Classes
{
    sealed class AppConfiguration
    {
        private readonly string _configPath;

        private AppConfiguration()
        {
            _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "ASC-C", "QTemplates");
            try
            {
                LoadSettings();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }

        public static AppConfiguration Instance { get; } = new AppConfiguration();

        /// <summary>
        /// Gets the database path from %AppData%/ASC-C/QTemplates in the current user's profile.
        /// </summary>
        /// <returns>Path to database</returns>
        public string GetDbPath() => Path.Combine(_configPath, "QTemplates.sqlite3");

        /// <summary>
        /// Sets up the personal database and loads any settings used by the application in general once implemented.
        /// </summary>
        private void LoadSettings()
        {
            if (File.Exists(GetDbPath()) == false)
            {
                MessageBox.Show("QTemplates needs to set up your personal database as it could not find one already in place.",
                    "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                Directory.CreateDirectory(_configPath); // Builds any missing folders in path.
                File.WriteAllBytes(GetDbPath(), Properties.Resources.QTemplates);


                MessageBox.Show("All done! You are now ready to start using the program.",
                    "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
