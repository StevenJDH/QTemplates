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
using QTemplates.Classes;
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

            var languages = _unitOfWork.Languages.GetAll();
            foreach (var entry in languages)
            {
                cmbLang.Items.Add(entry.Name);
            }

            var categories = _unitOfWork.Categories.GetAll();
            foreach (var entry in categories)
            {
                cmbCategoryVersions.Items.Add(entry.Name);
                cmbCategory.Items.Add(entry.Name);
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
            if (cmbCategory.Items.Count < 1)
            {
                MessageBox.Show("You do not have any categories created yet.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

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
                MessageBox.Show($"Error: A template with the title '{txtTitle.Text}' already exists.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var version = new Models.Version()
            {
                Message = txtMessage.Text.Trim(),
                TemplateId = _unitOfWork.Templates.FirstOrDefault(t => t.Title == template.Title).TemplateId,
                LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == "English").LanguageId,
            };
            _unitOfWork.Versions.Add(version);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex) // TODO: if version creation fails, it should remove the initial template created.
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"Template with ID: {version.TemplateId} was created successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var template = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text);
            _unitOfWork.Templates.Remove(template);
            
            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("The template and all of its associated translations have been removed.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lstTemplates.Items.Remove(lstTemplates.Text);
            ResetInterface();
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
                MessageBox.Show("The English version can only be removed by removing the template itself.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var version = _unitOfWork.Versions.FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);
            _unitOfWork.Versions.Remove(version);
            
            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"The '{cmbLangVersions.Text}' version of the selected template was deleted successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cmbLangVersions.Items.Remove(cmbLangVersions.Text);
            cmbLangVersions.SelectedIndex = 0;
        }

        private void BtnCreateVersion_Click(object sender, EventArgs e)
        {
            if (cmbLangVersions.Text == cmbLang.Text)
            {
                MessageBox.Show($"There is already an '{cmbLangVersions.Text}' version.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cmbLang.Text == "English")
            {
                MessageBox.Show("You cannot create another 'English' version as this is created by default.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var version = new Models.Version()
            {
                Message = txtMessage.Text.Trim(),
                TemplateId = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text).TemplateId,
                LanguageId = _unitOfWork.Languages.FirstOrDefault(l => l.Name == cmbLang.Text).LanguageId,
            };
            _unitOfWork.Versions.Add(version);
            
            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // LastOrDefault/Last methods are not supported with this SQLite implementation, so we do it this way.
            var versionCreated = _unitOfWork.Versions
                .FirstOrDefault(v => v.TemplateId == version.TemplateId && v.LanguageId == version.LanguageId);

            MessageBox.Show($"The '{cmbLang.Text}' version of the selected template was created with the ID: {versionCreated.VersionId} successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cmbLangVersions.Items.Add(cmbLang.Text);
            cmbLangVersions.Text = cmbLang.Text;
        }

        // TODO: Don't forget to support category changes.
        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            var template = _unitOfWork.Templates.FirstOrDefault(t => t.Title == lstTemplates.Text);

            template.Title = txtTitle.Text.Trim();
            //template.CategoryId = _unitOfWork.Categories.GetId(cmbCategory.Text);
            _unitOfWork.EditRecord(template, t => t.Title);
            //_unitOfWork.EditRecord(template, t => t.CategoryId);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                _unitOfWork.UndoChanges(); // TODO: Run performance test using this method vs just using _unitOfWork.Templates.Find()
                MessageBox.Show($"Error: A template with the title '{txtTitle.Text}' already exists.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Changes to the template were saved successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lstTemplates.Items.Remove(lstTemplates.Text);
            lstTemplates.Items.Add(txtTitle.Text);
            ResetInterface();
        }

        // TODO: Don't forget to support language change on version.
        private void BtnSaveVersionChanges_Click(object sender, EventArgs e)
        {
            var version = _unitOfWork.Versions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);

            version.Message = txtMessage.Text.Trim();
            _unitOfWork.EditRecord(version, v => v.Message);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"Changes to the '{cmbLangVersions.Text}' version were saved successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CmbLangVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var version = _unitOfWork.Versions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);

            cmbLang.Text = version?.Language.Name ?? ""; // Has to appear before txtMessage for when validation is triggered.
            cmbCategory.Text = version?.Template.Category.Name ?? "";
            txtTitle.Text = version?.Template.Title ?? "";
            txtMessage.Text = version?.Message ?? "";
        }

        private void LstTemplates_Click(object sender, EventArgs e)
        {
            if (lstTemplates.Text == "")
            {
                return;
            }

            cmbLangVersions.Items.Clear();
            _unitOfWork.Versions.GetVersionsWithAll()
                .Where(v => v.Template.Title == lstTemplates.Text)
                .Select(v => v.Language.Name)
                .ToList()
                .ForEach(v => cmbLangVersions.Items.Add(v));
            cmbLangVersions.SelectedIndex = 0;
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

        private void ValidateTemplateControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtMessage.Text) == false && 
                          String.IsNullOrWhiteSpace(txtTitle.Text) == false &&
                          cmbCategory.Text != "");
            btnCreate.Enabled = state;
            btnSaveChanges.Enabled = (state && lstTemplates.Text != "");
            btnDelete.Enabled = (lstTemplates.Text != "");
        }

        private void ValidateVersionControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtMessage.Text) == false && 
                          String.IsNullOrWhiteSpace(lstTemplates.Text) == false &&
                          cmbLang.Text != "");
            btnCreateVersion.Enabled = state;
            btnSaveVersionChanges.Enabled = state;
            btnDeleteVersion.Enabled = (cmbLangVersions.Text != "");
        }
    }
}
