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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTemplates.Classes
{

    public sealed class GlobalHotKey : NativeWindow, IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int NO_MOD = 0x0000;
        public const int ALT = 0X0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;
        private const int WM_HOTKEY_MSG_ID = 0x0312;

        private struct HotKeyEntry
        {
            public int Id;
            public Action Method;

        }

        private readonly List<HotKeyEntry> _hotKeyEntryList;

        public GlobalHotKey()
        {
            CreateHandle(new CreateParams()); // Generates a handle that let's this work under any situation.
            _hotKeyEntryList = new List<HotKeyEntry>();
        }

        /// <summary>
        /// Registers one or more global hotkeys that will work even if application is minimized
        /// or hidden.
        /// </summary>
        /// <param name="id">ID to represent the hotkey to be assigned. Must be unique.</param>
        /// <param name="fsModifiers">One or more key modifiers for hotkey.</param>
        /// <param name="vlc">Standard key for hotkey.</param>
        /// <param name="method">Void method to delegate with no parameters to run when hotkey is triggered.</param>
        /// <returns>True if successful or false if the process failed.</returns>
        public bool AddHotKey(int id, int fsModifiers, Keys vlc, Action method)
        {
            if (_hotKeyEntryList.Exists(e => e.Id == id))
            {
                throw new ArgumentException($"ID '{id}' is already in use.");
            }

            if (RegisterHotKey(this.Handle, id, fsModifiers, (int)vlc)) // Alternative this.GetType().GetHashCode() for id instead of static id.
            {
                HotKeyEntry entry;
                entry.Id = id;
                entry.Method = method;
                _hotKeyEntryList.Add(entry);
                return true;
            }
            return false; 
        }

        /// <summary>
        /// Unregisters the hotkey associated with the provided ID.
        /// </summary>
        /// <param name="id">ID representing the hotkey.</param>
        /// <returns>True if successful or false if the process failed.</returns>
        public bool RemoveHotKey(int id)
        {
            int index = _hotKeyEntryList.FindIndex(e => e.Id == id);

            if (index < 0)
            {
                throw new ArgumentException($"ID '{id}' does not exist.");
            }

            if (UnregisterHotKey(this.Handle, id))
            {
                _hotKeyEntryList.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calls the method associated with each registered hotkey that is triggered.
        /// </summary>
        /// <param name="m">Windows messages</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY_MSG_ID)
            {
                int id = m.WParam.ToInt32();
                int index = _hotKeyEntryList.FindIndex(e => e.Id == id);

                if (index >= 0)
                {
                    _hotKeyEntryList.ElementAt(index).Method.Invoke();
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Cleans up unmanaged resources allocated in memory.
        /// </summary>
        public void Dispose() => DestroyHandle();
    }
}
