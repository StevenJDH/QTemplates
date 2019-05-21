using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTemplates.Classes
{
    class WindowEffect
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        /// <summary>
        /// Flashes a window in the system tray by providing its handle.
        /// </summary>
        /// <param name="windowHandle">Handle of window to flash.</param>
        public void FlashWindow(IntPtr windowHandle)
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.dwFlags = 2;
            fInfo.dwTimeout = 0;
            fInfo.hwnd = windowHandle;
            fInfo.uCount = 3;

            FlashWindowEx(ref fInfo);
        }

        /// <summary>
        /// Flashes a window in the system tray by providing the application name.
        /// </summary>
        /// <param name="appName">Name of window to flash.</param>
        public void FlashWindow(string appName)
        {
            foreach (Process process in Process.GetProcessesByName(appName))
                FlashWindow(process.MainWindowHandle);
        }
    }
}
