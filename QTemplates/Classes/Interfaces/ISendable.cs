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

namespace QTemplates.Classes.Interfaces
{
    interface ISendable
    {
        /// <summary>
        /// Hooks the current foreground window to restore focus to it if lost before sending input to it.
        /// </summary>
        /// <returns>True if successful or false if otherwise</returns>
        bool HookWindow();

        /// <summary>
        /// Sends simulated keyboard input to an external window.
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <returns>True if successful or false if otherwise</returns>
        bool SendText(string text);

        /// <summary>
        /// Sends simulated keyboard input again to any external window currently in focus.
        /// </summary>
        void SendTextAgain();

        /// <summary>
        /// Resets and forgets the hooked foreground window.
        /// </summary>
        /// <returns>True if successful or false if otherwise</returns>
        bool ReleaseWindow();
    }
}
