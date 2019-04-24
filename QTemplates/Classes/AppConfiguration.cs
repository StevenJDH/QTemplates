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
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

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
                AppLogger.Fatal(ex, "Unable to load settings, so terminating application.");
                MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }

        public static AppConfiguration Instance { get; } = new AppConfiguration();

        public ILogger AppLogger { get; private set; }

        /// <summary>
        /// Gets the database path from %AppData%/ASC-C/QTemplates in the current user's profile.
        /// </summary>
        /// <returns>Path to database</returns>
        public string GetDbPath() => Path.Combine(_configPath, "QTemplates.sqlite3");

        /// <summary>
        /// Gets the path where the configuration needs to be stored for the current user.
        /// </summary>
        /// <returns>Configuration folder path</returns>
        public string GetConfigPath() => _configPath;

        /// <summary>
        /// Sets up the personal database and loads any settings used by the application in general once implemented.
        /// </summary>
        private void LoadSettings()
        {
            AppLogger = GetLogger();

            if (File.Exists(GetDbPath()) == false)
            {
                MessageBox.Show("QTemplates needs to set up your personal database as it could not find one already in place.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Directory.CreateDirectory(_configPath); // Builds any missing folders in path.
                File.WriteAllBytes(GetDbPath(), Properties.Resources.QTemplates);
                AppLogger.Info("Personal database created successfully.");

                MessageBox.Show("All done! You are now ready to start using the program.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Gets the logger that will write and archive json-based structured logs as needed.
        /// </summary>
        /// <returns>Logger used for logging</returns>
        private Logger GetLogger()
        {
            var config = new LoggingConfiguration();

            var jsonLayout = new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("longDate", "${longdate}"),
                    new JsonAttribute("Logger", "${logger}"),
                    new JsonAttribute("level", "${level:uppercase=true}"),
                    new JsonAttribute("message", "${message}"),
                    new JsonAttribute("type", "${exception:format=Type}"),
                    new JsonAttribute("exception", "${exception}"),
                    new JsonAttribute("innerException", new JsonLayout
                        {
                            Attributes =
                            {
                                new JsonAttribute("type", "${exception:format=:innerFormat=Type:" +
                                                             "MaxInnerExceptionLevel=1:InnerExceptionSeparator=}"),
                                new JsonAttribute("message", "${exception:format=:innerFormat=Message:" +
                                                             "MaxInnerExceptionLevel=1:InnerExceptionSeparator=}"),
                            },
                            RenderEmptyObject = false
                        },
                        // Don't escape layout.
                        false),
                    new JsonAttribute("stacktrace", @"${replace:inner=${exception:format=StackTrace}:searchFor=\\r\\n|\\s:replaceWith= :regex=true:trimWhiteSpace=true}"),
                    new JsonAttribute("callSite", "${callsite:className=true:fileName=true:" +
                                                  "includeSourcePath=false:methodName=true}")
                }
            };

            var jsonFileTarget = new FileTarget("mainTarget")
            {
                FileName = "${specialfolder:folder=ApplicationData}/ASC-C/QTemplates/Logs/Application.log",
                // Dedicated archive folder is needed or other files will be deleted.
                ArchiveFileName = "${specialfolder:folder=ApplicationData}/ASC-C/QTemplates/Logs/Archived/" +
                                  "Application-{#}.log", 
                Layout = jsonLayout,
                MaxArchiveFiles = 7,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveDateFormat = "yyyyMMdd",
                CreateDirs = true,
                Encoding = Encoding.UTF8
            };

            config.AddTarget(jsonFileTarget);
            config.AddRuleForAllLevels(jsonFileTarget);
            LogManager.Configuration = config;

            return LogManager.GetLogger($"{Application.ProductName} {Application.ProductVersion}");
        }
    }
}
