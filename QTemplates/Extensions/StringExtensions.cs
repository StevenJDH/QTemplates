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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QTemplates.Extensions
{
    /// <summary>
    /// Useful extensions to overcome some missing functionality and limitations while keeping then code clean.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces all occurrences of a word with the specified replacement word. Specified options modify the matching operation.
        /// </summary>
        /// <param name="str">Object owner of method.</param>
        /// <param name="word">Word being replaced.</param>
        /// <param name="replacement">The replacement word.</param>
        /// <param name="regexOptions">An optional bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns>A new string that is identical to the original string, except that the replacement word takes the place of each matched word. If <paramref name="word" /> is not matched in the current instance, the method returns the original string unchanged.</returns>
        /// <exception cref="T:System.ArgumentException">A regular expression parsing error occurred.</exception>
        /// <exception cref="T:System.ArgumentNullException">A string is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="regexOptions" /> is not a valid bitwise combination of <see cref="T:System.Text.RegularExpressions.RegexOptions" /> values.</exception>
        public static string ReplaceWholeWord(this String str, string word, 
            string replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            return Regex.Replace(str, @"\b" + word + @"\b", replacement, regexOptions);
        }

        /// <summary>
        /// Searches for a whole word and returns its start position from within a string. Specified options modify the matching operation.
        /// </summary>
        /// <param name="str">Object owner of method.</param>
        /// <param name="word">Word to search for.</param>
        /// <param name="regexOptions">An optional bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns>Index of word position.</returns>
        /// <exception cref="T:System.ArgumentException">A regular expression parsing error occurred.</exception>
        /// <exception cref="T:System.ArgumentNullException">A string is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="regexOptions" /> is not a valid bitwise combination of <see cref="T:System.Text.RegularExpressions.RegexOptions" /> values.</exception>
        public static int FindWholeWord(this String str, string word, RegexOptions regexOptions = RegexOptions.None)
        {
            return Regex.Match(str, @"\b" + word + @"\b", regexOptions).Index;
        }

        /// <summary>
        /// Gets all words from a string including those words that use a dash and or an apostrophe.
        /// </summary>
        /// <param name="str">Object owner of method.</param>
        /// <returns>A list of words found.</returns>
        /// <exception cref="T:System.ArgumentException">A regular expression parsing error occurred.</exception>
        /// <exception cref="T:System.ArgumentNullException">A string is <see langword="null" />.</exception>
        public static IEnumerable<string> GetAllWords(this String str)
        {
            return Regex.Matches(str, @"\b[\w-']+\b")
                .OfType<Match>()
                .Select(m => m.Value);
        }
    }
}
