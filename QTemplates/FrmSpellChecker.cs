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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHunspell;
using NLog;
using QTemplates.Classes;
using QTemplates.Extensions;

namespace QTemplates
{
    public partial class FrmSpellChecker : Form
    {
        private readonly IEnumerable<string> _words;
        private readonly List<string> _misspelledWords;
        private readonly string _language;
        private int _wordIndex;
        private readonly string _dictPath;
        private readonly List<string> _dictionaries;
        private Hunspell _hunspell;
        private readonly ILogger _logger;

        /// <summary>
        /// Gets the corrected text with any changes that were made.
        /// </summary>
        public string CorrectedText { get; private set; }

        public FrmSpellChecker(string originalText, string language, IWin32Window owner)
        {
            InitializeComponent();
            txtOriginal.Text = originalText;
            _words = originalText.GetAllWords().Distinct();
            _misspelledWords = new List<string>();
            _language = language;
            _wordIndex = -1;
            _dictPath = "Dictionaries";
            _dictionaries = Directory.GetFiles(_dictPath, "*.dic", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
            _hunspell = null;
            _logger = AppConfiguration.Instance.AppLogger;
            CorrectedText = originalText;

            this.Icon = Properties.Resources.spell_icon_mini;
            this.ShowDialog(owner);
        }

        private void FrmSpellChecker_Load(object sender, EventArgs e)
        {
            foreach (var dict in _dictionaries)
            {
                try
                {
                    cmbDictionaries.Items.Add(CultureInfo.GetCultureInfo(dict.Replace('_', '-'))
                        .EnglishName);
                }
                catch (CultureNotFoundException ex)
                {
                    _logger.Error(ex, "Got exception.");
                    MessageBox.Show($"Error: The '{dict}.dic' is not using a valid locale for its filename.",
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _dictionaries.Remove(dict);
                }
            }
        }

        private void FrmSpellChecker_Shown(object sender, EventArgs e)
        {
            try
            {
                cmbDictionaries.Text = LoadDictionary(_language);
                CheckNextWord();
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error("Got exception.");
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();
            }
        }

        /// <summary>
        /// Loads the dictionary that best matches the specified language to identify and correct spelling mistakes.
        /// </summary>
        /// <param name="language">Language of the source text being provided.</param>
        /// <returns>Dictionary name to be used for the spell check.</returns>
        /// <exception cref="T:System.InvalidOperationException">Could not find a dictionary installed for the selected language.</exception>
        /// <exception cref="T:System.FileNotFoundException">One or both files of a selected dictionary are missing.</exception>
        private string LoadDictionary(string language)
        {
            string locale = GetLocale(language)?.ToLower() ?? "en_us";
            string foundDictLocale = null;

            foreach (var dict in _dictionaries)
            {
                string dictFile = dict.ToLower();

                // If there is a matching dictionary, then use it, or if region is missing
                // form template language, use first matching language.
                if (dictFile == locale || dictFile.StartsWith(locale))
                {
                    foundDictLocale = dictFile;
                    break;
                }

                // If dictionary is missing region, use first matching language, but we don't
                // stop checking since there still might be a better match.
                if (dictFile.Contains("_") == false && locale.StartsWith(dictFile))
                {
                    foundDictLocale = dictFile;
                }
            }

            if (foundDictLocale == null)
            {
                throw new InvalidOperationException($"Could not find a dictionary installed for the '{language}' language.");
            }

            string aff = Path.Combine(_dictPath, $"{foundDictLocale}.aff");
            string dic = Path.Combine(_dictPath, $"{foundDictLocale}.dic");
            string dictEnglishName = CultureInfo
                .GetCultureInfo(foundDictLocale.Replace('_', '-'))
                .EnglishName;

            if (File.Exists(aff) == false || File.Exists(dic) == false)
            {
                throw new FileNotFoundException($"One or both files for the '{dictEnglishName}' dictionary are missing or not using the correct locale format, e.g. 'xx_XX'.");
            }

            _hunspell?.Dispose();
            _hunspell = new Hunspell(aff, dic);

            _misspelledWords.Clear();
            foreach (var word in _words)
            {
                if (_hunspell.Spell(word) == false)
                {
                    _misspelledWords.Add(word);
                }
            }

            return dictEnglishName;
        }

        /// <summary>
        /// Displays each misspelled word for correction and word suggestions. If there are no misspelled
        /// word left, the method displays the complete message before closing the spell checker.
        /// </summary>
        public void CheckNextWord()
        {
            _wordIndex++;

            if (_wordIndex > _misspelledWords.Count - 1)
            {
                MessageBox.Show("The spelling check is complete.", Application.ProductName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                string misspelledWord = _misspelledWords[_wordIndex];
                int index = txtOriginal.Text.FindWholeWord(misspelledWord);

                txtOriginal.Select(index, misspelledWord.Length);
                txtOriginal.ScrollToCaret(); // This will have no effect if initially called from form_load event.

                statusPaneWord.Text = misspelledWord;
                statusPaneCount.Text = $"Word: {_wordIndex + 1} of {_misspelledWords.Count}";
                statusPaneIndex.Text = $"Index: {index}";

                lstSuggestions.Items.Clear();
                _hunspell.Suggest(_misspelledWords[_wordIndex]).ForEach(s => lstSuggestions.Items.Add(s));
                if (lstSuggestions.Items.Count > 0)
                {
                    lstSuggestions.SelectedIndex = 0;
                }
            }
        }

        private void LstSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtReplaceWith.Text = lstSuggestions.Text;
        }

        /// <summary>
        /// Gets the locale (ISO 639-1/2 and ISO 3166-1 standard codes) for the specified language name.
        /// </summary>
        /// <param name="englishLanguageName">Language to search for in English.</param>
        /// <returns>Locale of language using underscores instead of dashes.</returns>
        private string GetLocale(string englishLanguageName)
        {
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (cultureInfo.EnglishName == englishLanguageName)
                {
                    return cultureInfo.Name.Replace('-', '_');
                }
            }

            return null;
        }

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            // Case-sensitive
            CorrectedText = CorrectedText.ReplaceWholeWord(statusPaneWord.Text, txtReplaceWith.Text);
            txtOriginal.Text = CorrectedText;
            CheckNextWord();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnIgnore_Click(object sender, EventArgs e)
        {
            CheckNextWord();
        }

        private void FrmSpellChecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            _hunspell?.Dispose();
        }

        private void CmbDictionaries_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string selectedLang = LoadDictionary(cmbDictionaries.Text);

                // Resets interface before checking with newly selected dictionary.
                txtOriginal.Select(0, 0);
                txtOriginal.ScrollToCaret();
                txtReplaceWith.Text = "";
                lstSuggestions.Items.Clear();
                lstSuggestions.Focus();
                statusPaneWord.Text = "";
                statusPaneCount.Text = "Word: 0 of 0";
                statusPaneIndex.Text = "Index: 0";
                _wordIndex = -1;

                CheckNextWord();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is FileNotFoundException)
            {
                _logger.Error(ex, "Got exception.");
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
        }

        private void CmbDictionaries_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevents mouse scrolling via mouse wheel and arrow keys.
            e.Handled = true;
        }
    }
}
