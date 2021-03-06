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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using QTemplates.Classes.Interfaces;

namespace QTemplates.Classes
{
    class KeyboardSimulator : ISendable
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SendMessage(IntPtr hWnd, uint msg, int wParam, uint lParam);

        private const uint WM_SYSCOMMAND = 0x0112;
        private const int SC_RESTORE = 0xF120;
        private IntPtr _foregroundHWnd;
        private string _lastTemplateUsed;
        private readonly WindowEffect _winEffect;

        public KeyboardSimulator()
        {
            _foregroundHWnd = IntPtr.Zero;
            _lastTemplateUsed = "<[ No templates used yet ]>";
            _winEffect = new WindowEffect();
        }

        public bool HookWindow()
        {
            // If template selection window is closed, capture handle.
            if (_foregroundHWnd == IntPtr.Zero)
            {
                _foregroundHWnd = GetForegroundWindow();
            }

            if (IsWindow(_foregroundHWnd))
            {
                _winEffect.FlashWindow(_foregroundHWnd);
                return true;
            }

            _foregroundHWnd = IntPtr.Zero;
            return false;
        }

        public bool SendText(string text)
        {
            _lastTemplateUsed = text;
            SwitchWindow(_foregroundHWnd);
            SendTextAgain();
            
            return true;
        }

        public void SendTextAgain()
        {
            var clipboardTextBackup = Clipboard.GetText();
            var clipboardObjectBackup = Clipboard.GetDataObject();

            Clipboard.SetText(_lastTemplateUsed);
            SendKeys.Send("^(v)");

            // Workaround to allow Windows to catch up otherwise clipboard restores before above line finishes.
            Thread.Sleep(500);

            if (String.IsNullOrWhiteSpace(clipboardTextBackup) == false)
            {
                Clipboard.SetText(clipboardTextBackup);
            }
            else
            {
                Clipboard.SetDataObject(clipboardObjectBackup);
            }
        }

        /// <summary>
        /// Brings to the foreground the window associated to the handle provided even if minimized.
        /// </summary>
        /// <param name="windowHandle">Handle of window to bring to the foreground</param>
        /// <exception cref="T:System.ArgumentException">Target window is no longer available.</exception>
        /// <exception cref="T:System.Win32Exception">WinAPI related errors.</exception>
        private void SwitchWindow(IntPtr windowHandle)
        {
            if (IsWindow(windowHandle) == false)
            { 
                throw new ArgumentException("Target window is no longer available.");
            }

            IntPtr foregroundWindowHandle = GetForegroundWindow();

            if (foregroundWindowHandle == windowHandle)
            {
                return; // Target window already in foreground.
            }

            uint currentThreadId = GetCurrentThreadId();
            uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindowHandle, out uint processId);

            if (foregroundThreadId == 0)
            {
                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                throw new Win32Exception($"Unable to get the thread ID of foreground window. {errorMessage}.");
            }

            // AttachThreadInput is needed so we can get the handle of a focused window in another application.
            if (AttachThreadInput(currentThreadId, foregroundThreadId, true))
            {
                // Restore original window only if minimized otherwise SetForegroundWindow() won't work correctly.
                // This check is needed to avoid resizing hooked window sometimes when not minimized.
                if (IsIconic(windowHandle))
                {
                    SendMessage(windowHandle, WM_SYSCOMMAND, SC_RESTORE, 0);
                }
                // Switches back to original Window intended for the text injection.
                SetForegroundWindow(windowHandle);
                // Now detach since we got the focused handle
                AttachThreadInput(currentThreadId, foregroundThreadId, false);
                // Wait for window to become active.
                while (GetForegroundWindow() != windowHandle) {}
            }
            else
            {
                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                throw new Win32Exception($"Unable to attach threads for input. {errorMessage}.");
            }
        }

        public bool ReleaseWindow()
        {
            _foregroundHWnd = IntPtr.Zero;
            return true;
        }
    }
}
