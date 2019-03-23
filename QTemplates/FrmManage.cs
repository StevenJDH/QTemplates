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
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QTemplates.Classes;
using QTemplates.Extensions;
using QTemplates.Models;
using QTemplates.Models.UnitOfWork;

namespace QTemplates
{
    public partial class FrmManage : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrmManage(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmManage_Load(object sender, EventArgs e)
        {
            // Loading the template list is not needed because the cmbCategoryVersions index change event will do it.

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
                cmbCategoryVersions.Items.Add(category);
                cmbCategory.Items.Add(category);
            }

            cmbCategoryVersions.Text = "All";
            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }
 
        private void CmbCategoryVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTemplates.Items.Clear();

            if (cmbCategoryVersions.Text == "All")
            {
                _unitOfWork.Templates.GetAll()
                    .Select(t => t.Title)
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
            else
            {
                _unitOfWork.Templates.GetAll()
                    .Where(c => c.Category.Name == cmbCategoryVersions.Text)
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
            catch (DbUpdateException)
            {
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
                _unitOfWork.UndoChanges();
                _unitOfWork.Templates.Remove(template); // If version creation fails, remove the initial template created.
                _unitOfWork.Complete();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"The '{template.Title}' template was created successfully.", 
                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (cmbCategoryVersions.Text == "All" || cmbCategoryVersions.Text == cmbCategory.Text)
            {
                lstTemplates.Items.Add(template.Title);
                lstTemplates.Text = template.Title;
                LstTemplates_Click(this, EventArgs.Empty);
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
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Resets the interface so that all controls and validation will work correctly.
        /// </summary>
        private void ResetInterface()
        {
            btnSaveChanges.Enabled = false;
            btnDelete.Enabled = false;
            btnDeleteVersion.Enabled = false;
            cmbLangVersions.Items.Clear();
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
            if (cmbLangVersions.Text == "English")
            {
                MessageBox.Show("The English version can only be deleted by deleting the template itself.", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete the '{cmbLangVersions.Text}' version of the '{lstTemplates.Text}' template?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            var version = _unitOfWork.TemplateVersions.FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);
            _unitOfWork.TemplateVersions.Remove(version);
            
            try
            {
                _unitOfWork.Complete();
                MessageBox.Show($"The '{cmbLangVersions.Text}' version of the selected template was deleted successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbLangVersions.Items.Remove(cmbLangVersions.Text);
                lblLangAvailable.Text = $"Languages available: {cmbLangVersions.Items.Count}";
                cmbLangVersions.Text = "English";
            }
            catch (DbUpdateException ex)
            {
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
                cmbLangVersions.Items.Add(cmbLang.Text);
                lblLangAvailable.Text = $"Languages available: {cmbLangVersions.Items.Count}";
                cmbLangVersions.Text = cmbLang.Text;
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update the '{lstTemplates.Text}' template?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var template = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text);

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
            catch (DbUpdateException)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: A template with the title '{txtTitle.Text}' already exists.", 
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveVersionChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update the '{cmbLangVersions.Text}' version of the '{lstTemplates.Text}' template?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var version = _unitOfWork.TemplateVersions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);

            version.Message = txtMessage.Text.Trim();
            version.LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == cmbLang.Text).LanguageId;

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show($"Changes to the '{cmbLangVersions.Text}' version were saved successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                // TODO: Update list in case language for version was changed.
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbLangVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var version = _unitOfWork.TemplateVersions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);

            cmbLang.Text = version?.Language.Name ?? ""; // Has to appear before txtMessage for when validation is triggered.
            cmbCategory.Text = version?.Template.Category.Name ?? "";
            txtTitle.Text = version?.Template.Title ?? "";
            txtMessage.Text = version?.Message ?? "";
        }

        private void LstTemplates_Click(object sender, EventArgs e) // TODO: Look into changing this back to a selection index change event handler because using the arrow keys does do anything.
        {
            if (lstTemplates.Text == "")
            {
                return;
            }

            cmbLangVersions.Items.Clear();
            _unitOfWork.TemplateVersions.GetVersionsWithAll()
                .Where(v => v.Template.Title == lstTemplates.Text)
                .OrderBy(v => v.Language.Name)
                .Select(v => v.Language.Name)
                .ToList()
                .ForEach(v => cmbLangVersions.Items.Add(v));
            lblLangAvailable.Text = $"Languages available: {cmbLangVersions.Items.Count}";
            cmbLangVersions.Text = "English";
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
        /// Modify:
        ///     Title, Message, and Category must be filled.
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
            btnSaveChanges.Enabled = (state && lstTemplates.Text != "");
            btnDelete.Enabled = (lstTemplates.Text != "");
        }

        /// <summary>
        /// The following are the template version validation rules:
        /// Create:
        ///     Title in list is selected, Message and Language are filled.
        ///     Language cannot be the English default, and it must not be already created.
        /// Modify:
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
            btnCreateVersion.Enabled = (state && cmbLang.Text != "English" && cmbLangVersions.Items.ContainsEx(cmbLang.Text) == false);
            btnSaveVersionChanges.Enabled = (state && (cmbLangVersions.Text == cmbLang.Text || (cmbLangVersions.Items.Count > 1 && cmbLangVersions.Items.ContainsEx(cmbLang.Text) == false)));
            btnDeleteVersion.Enabled = (cmbLangVersions.Text != ""); // Not also validating for English to explain in message box how to delete it.
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
            cmbCategoryVersions.Items.Clear();
            cmbCategoryVersions.Items.Add("All");
            cmbCategory.Items.Clear();
            cmbLang.Items.Clear();
            ResetInterface();
            FrmManage_Load(this, EventArgs.Empty);
        }
    }
}
