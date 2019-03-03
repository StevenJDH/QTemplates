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

using QInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QTemplates.Classes
{
    sealed class PluginProvider
    {
        public static PluginProvider Instance { get; private set; }
        private static Dictionary<string, IPlugin> _plugins;

        private PluginProvider()
        {
            _plugins = new Dictionary<string, IPlugin>();
        }

        static PluginProvider()
        {
            Instance = new PluginProvider(); // Singleton design pattern.
        }

        public Dictionary<string, IPlugin> LoadPlugins(string pluginFolderPath)
        {
            //Loads the DLLs from the Plugins directory
            if (Directory.Exists(pluginFolderPath))
            {
                string[] dllFiles = Directory.GetFiles(pluginFolderPath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    Assembly.LoadFile(Path.GetFullPath(dllFile));
                }
                CreateInstances(ExamineAssemblies());
            }
            return _plugins;
        }

        private Type[] ExamineAssemblies()
        {
            Type pluginInterfaceType = typeof(IPlugin);
            //Gets all types that implement the interface IPlugin and are a class
            Type[] pluginTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => pluginInterfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            return pluginTypes;
        }


        private void CreateInstances(Type[] pluginTypes)
        {
            foreach (Type type in pluginTypes)
            {
                if (Activator.CreateInstance(type) is IPlugin loadedPlugin)
                {
                    _plugins.Add(loadedPlugin.Title, loadedPlugin);
                }
            }
        }
    }
}
