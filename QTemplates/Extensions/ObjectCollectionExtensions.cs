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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTemplates.Extensions
{
    /// <summary>
    /// Useful extensions to overcome some missing functionality and limitations while keeping the code clean.
    /// </summary>
    public static class ObjectCollectionExtensions
    {
        /// <summary>
        /// Determines if the specified item is located within the collection. A parameter specifies the culture, case, 
        /// and sort rules used in the determination. The default is OrdinalIgnoreCase.
        /// </summary>
        /// <param name="items">The object where this extension will be attached.</param>
        /// <param name="value">The string to compare to this instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>True if equal, and false if not.</returns>
        public static bool ContainsEx(this ListBox.ObjectCollection items, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            foreach (var item in items)
            {
                if (item.ToString().Equals(value, comparisonType))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the specified item is located within the collection. A parameter specifies the culture, case, 
        /// and sort rules used in the determination. The default is OrdinalIgnoreCase.
        /// </summary>
        /// <param name="items">The object where this extension will be attached.</param>
        /// <param name="value">The string to compare to this instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>True if equal, and false if not.</returns>
        public static bool ContainsEx(this ComboBox.ObjectCollection items, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            foreach (var item in items)
            {
                if (item.ToString().Equals(value, comparisonType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
