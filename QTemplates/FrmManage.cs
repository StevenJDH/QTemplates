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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using QTemplates.Classes;
using QTemplates.Extensions;
using QTemplates.Models;
using QTemplates.Models.UnitOfWork;

namespace QTemplates
{
    public partial class FrmManage : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public FrmManage(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _logger = AppConfiguration.Instance.AppLogger;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmManage_Load(object sender, EventArgs e)
        {
            // Loading the template list is not needed because the cmbCategoryFilter index change event will do it.

            var languages = _unitOfWork.Languages.GetAll()
                .OrderBy(l => l.Name)
                .Select(l => l.Name)
                .ToList();
            foreach (var language in languages)
            {
                cmbLang.Items.Add(language);
            }

            var categories = _unitOfWork.Categories.GetAll()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToList();
            foreach (var category in categories)
            {
                cmbCategoryFilter.Items.Add(category);
                cmbCategory.Items.Add(category);
            }

            cmbCategoryFilter.Text = "All";
            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }
 
        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTemplates.Items.Clear();

            if (cmbCategoryFilter.Text == "All")
            {
                _unitOfWork.Templates.GetAll()
                    .Select(t => t.Title)
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
            else
            {
                _unitOfWork.Templates.GetAll()
                    .Where(c => c.Category.Name == cmbCategoryFilter.Text)
                    .Select(t => t.Title)
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }

            // Resets interface as nothing will be selected after a category change.
            ResetInterface();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var template = new Template()
            {
                Title = txtTitle.Text.Trim(),
                CategoryId = _unitOfWork.Categories.FirstOrDefault(c => c.Name == cmbCategory.Text).CategoryId
            };
            _unitOfWork.Templates.Add(template);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: A template with the title '{template.Title}' already exists.", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var version = new TemplateVersion()
            {
                Message = txtMessage.Text.Trim(),
                TemplateId = template.TemplateId,
                LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == "English").LanguageId
            };
            _unitOfWork.TemplateVersions.Add(version);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                _unitOfWork.Templates.Remove(template); // If version creation fails, remove the initial template created.
                _unitOfWork.Complete();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"The '{template.Title}' template was created successfully.", 
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cmbCategoryFilter.Text == "All" || cmbCategoryFilter.Text == cmbCategory.Text)
            {
                lstTemplates.Items.Add(template.Title);
                lstTemplates.Text = template.Title;
                ValidateTemplateControls();
                lstTemplates.Focus();
            }
            else
            {
                lstTemplates.ClearSelected();
                ResetInterface();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete the '{lstTemplates.Text}' template?", 
                Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var template = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text);
            _unitOfWork.Templates.Remove(template);
            
            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("The template and all of its associated translations have been deleted successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstTemplates.Items.Remove(lstTemplates.Text);
                ResetInterface();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Resets the interface so that all controls and validation will work correctly.
        /// </summary>
        private void ResetInterface()
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnDeleteVersion.Enabled = false;
            cmbLangAvailable.Items.Clear();
            lblLangAvailable.Text = "Languages available: 0";
            txtTitle.Text = "";
            txtMessage.Text = "";
            cmbLang.SelectedIndex = -1;
            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void BtnDeleteVersion_Click(object sender, EventArgs e)
        {
            if (cmbLangAvailable.Text == "English")
            {
                MessageBox.Show("The English version can only be deleted by deleting the template itself.", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete the '{cmbLangAvailable.Text}' version of the '{lstTemplates.Text}' template?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            var version = _unitOfWork.TemplateVersions.FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangAvailable.Text);
            _unitOfWork.TemplateVersions.Remove(version);
            
            try
            {
                _unitOfWork.Complete();
                MessageBox.Show($"The '{cmbLangAvailable.Text}' version of the selected template was deleted successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLangAvailable.Items.Remove(cmbLangAvailable.Text);
                lblLangAvailable.Text = $"Languages available: {cmbLangAvailable.Items.Count}";
                cmbLangAvailable.Text = "English";
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateVersion_Click(object sender, EventArgs e)
        {
            var version = new TemplateVersion()
            {
                Message = txtMessage.Text.Trim(),
                TemplateId = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text).TemplateId,
                LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == cmbLang.Text).LanguageId,
            };
            _unitOfWork.TemplateVersions.Add(version);
            
            try
            {
                _unitOfWork.Complete();
                MessageBox.Show($"The '{cmbLang.Text}' version of the selected template was created successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLangAvailable.Items.Add(cmbLang.Text);
                lblLangAvailable.Text = $"Languages available: {cmbLangAvailable.Items.Count}";
                cmbLangAvailable.Text = cmbLang.Text;
                ValidateVersionControls();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var template = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text);

            if (MessageBox.Show($"Are you sure you want to update the '{lstTemplates.Text}' template?\n\n" +
                                $"[ Before ]\n" +
                                $"Title: {template.Title}\n" +
                                $"Category: {template.Category.Name}\n\n" +
                                $"[ After ]\n" +
                                $"Title: {txtTitle.Text}\n" +
                                $"Category: {cmbCategory.Text}\n\n" +
                                $"NOTE: Updating the message content is done from the Version tab.",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            template.Title = txtTitle.Text.Trim();
            template.CategoryId = _unitOfWork.Categories.FirstOrDefault(c => c.Name == cmbCategory.Text).CategoryId;

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("Changes to the template were saved successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstTemplates.Items.Remove(lstTemplates.Text);
                lstTemplates.Items.Add(template.Title);
                ResetInterface();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: A template with the title '{txtTitle.Text}' already exists.", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdateVersion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update the '{cmbLangAvailable.Text}' version of the '{lstTemplates.Text}' template?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var version = _unitOfWork.TemplateVersions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangAvailable.Text);

            version.Message = txtMessage.Text.Trim();
            version.LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == cmbLang.Text).LanguageId;

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("Changes to the selected template version were saved successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLangAvailable.Items.Remove(cmbLangAvailable.Text);
                cmbLangAvailable.Items.Add(cmbLang.Text);
                cmbLangAvailable.Text = cmbLang.Text;
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Got exception.");
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbLangAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var version = _unitOfWork.TemplateVersions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangAvailable.Text);

            cmbLang.Text = version?.Language.Name ?? ""; // Has to appear before txtMessage for when validation is triggered.
            cmbCategory.Text = version?.Template.Category.Name ?? "";
            txtTitle.Text = version?.Template.Title ?? "";
            txtMessage.Text = version?.Message ?? "";
        }

        private void LstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbLangAvailable.Items.Clear();
            _unitOfWork.TemplateVersions.GetVersionsWithAll()
                .Where(v => v.Template.Title == lstTemplates.Text)
                .OrderBy(v => v.Language.Name)
                .Select(v => v.Language.Name)
                .ToList()
                .ForEach(v => cmbLangAvailable.Items.Add(v));
            lblLangAvailable.Text = $"Languages available: {cmbLangAvailable.Items.Count}";
            cmbLangAvailable.Text = "English";
        }

        private void TxtMessage_TextChanged(object sender, EventArgs e)
        {
            ValidateTemplateControls();
            ValidateVersionControls();
        }

        private void TxtTitle_TextChanged(object sender, EventArgs e)
        {
            ValidateTemplateControls();
        }

        private void CmbLang_DropDownClosed(object sender, EventArgs e)
        {
            ValidateVersionControls();
        }

        /// <summary>
        /// The following are the template validation rules:
        /// Create:
        ///     Title, Message, and Category must be filled.
        ///     Title must not be in the current list.
        /// Update:
        ///     Title and Category must be filled.
        ///     List must have a title selected.
        /// Delete:
        ///     List must have a title selected.
        /// </summary>
        private void ValidateTemplateControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtMessage.Text) == false && 
                          String.IsNullOrWhiteSpace(txtTitle.Text) == false &&
                          cmbCategory.Text != "");
            btnCreate.Enabled = (state &&  lstTemplates.Items.ContainsEx(txtTitle.Text.Trim()) == false);
            txtMessage.ReadOnly = !(btnCreate.Enabled || String.IsNullOrWhiteSpace(txtTitle.Text) || lstTemplates.Text == "" || tabControl1.SelectedTab == tabVersion);
            btnSpellCheck.Enabled = !txtMessage.ReadOnly;
            btnUpdate.Enabled = (String.IsNullOrWhiteSpace(txtTitle.Text) == false && cmbCategory.Text != "" && lstTemplates.Text != "");
            btnDelete.Enabled = (lstTemplates.Text != "");
        }

        /// <summary>
        /// The following are the template version validation rules:
        /// Create:
        ///     Title in list is selected, Message and Language are filled.
        ///     Language cannot be the English default, and it must not be already created.
        /// Update:
        ///     Title in list is selected, Message and Language are filled.
        ///     Language must match selected language versions created, or without changing the
        ///     required English default, be a new Language.
        /// Delete:
        ///     There must be a language selected in the language versions created.
        /// </summary>
        private void ValidateVersionControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtMessage.Text) == false && 
                          String.IsNullOrWhiteSpace(lstTemplates.Text) == false &&
                          cmbLang.Text != "");
            btnCreateVersion.Enabled = (state && cmbLang.Text != "English" && cmbLangAvailable.Items.ContainsEx(cmbLang.Text) == false);
            btnUpdateVersion.Enabled = (state && (cmbLangAvailable.Text == cmbLang.Text || (cmbLangAvailable.Items.Count > 1 && cmbLangAvailable.Items.ContainsEx(cmbLang.Text) == false)));
            btnDeleteVersion.Enabled = (cmbLangAvailable.Text != ""); // Not also validating for English to explain in message box how to delete it.
        }

        private void BtnManageCategories_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmManageCategories(_unitOfWork))
            {
                frm.ShowDialog(this);
            }
            if (_unitOfWork.IsDisposed == false) // This is for when exit is clicked while the dialog is open.
            {
                ReloadComboBoxes();
            }
        }

        private void BtnManageLanguages_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmManageLanguages(_unitOfWork))
            {
                frm.ShowDialog(this);
            }
            if (_unitOfWork.IsDisposed == false) // This is for when exit is clicked while the dialog is open.
            {
                ReloadComboBoxes();
            }
        }

        private void ReloadComboBoxes()
        {
            cmbCategoryFilter.Items.Clear();
            cmbCategoryFilter.Items.Add("All");
            cmbCategory.Items.Clear();
            cmbLang.Items.Clear();
            ResetInterface();
            FrmManage_Load(this, EventArgs.Empty);
        }

        private void BtnSpellCheck_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSpellChecker(txtMessage.Text, cmbLang.Text, this))
            {
                txtMessage.Text = frm.CorrectedText;
            }
        }

        private void CmbCategoryFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevents mouse scrolling via mouse wheel and arrow keys.
            e.Handled = true;
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateTemplateControls();
        }

        private void TxtMessage_ReadOnlyChanged(object sender, EventArgs e)
        {
            txtMessage.BackColor = txtMessage.ReadOnly ? 
                Color.FromArgb(246, 246, 246) : SystemColors.Window;
        }
    }
}
