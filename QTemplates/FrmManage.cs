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
using System.Data.SQLite;
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
            cmbCategoryVersions.SelectedIndex = 0;
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
                cmbLangFilter.Items.Add(entry.Name);
                cmbLang.Items.Add(entry.Name);
            }

            var categories = _unitOfWork.Categories.GetAll();
            foreach (var entry in categories)
            {
                cmbCategoryVersions.Items.Add(entry.Name);
                cmbCategory.Items.Add(entry.Name);
            }
        }
 
        private void cmbCategoryVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTemplates.Items.Clear();

            if (cmbCategoryVersions.Text == "All")
            {
                _unitOfWork.Templates.GetAll()
                    .Select(t => t.Title)
                    .Distinct()
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
            else
            {
                _unitOfWork.Templates.GetAll()
                    .Where(c => c.Category.Name == cmbCategoryVersions.Text)
                    .Select(t => t.Title)
                    .Distinct()
                    .ToList()
                    .ForEach(t => lstTemplates.Items.Add(t));
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var template = new Template()
            {
                Title = txtTitle.Text,
                CategoryId = _unitOfWork.Categories.GetId(cmbCategoryVersions.Text)
            };
            _unitOfWork.Templates.Add(template);
            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateException)
            {
                MessageBox.Show($"Error: A template with the title '{txtTitle.Text}' already exists.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var version = new Models.Version()
            {
                Message = txtMessage.Text,
                TemplateId = _unitOfWork.Templates.GetId(template.Title),
                LanguageId = _unitOfWork.Languages.GetId(cmbLang.Text),
            };
            _unitOfWork.Versions.Add(version);

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show($"Template with ID: {version.TemplateId} was created successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (cmbLang.Text == cmbLangVersions.Text && cmbCategory.Text == cmbCategoryVersions.Text)
                {
                    lstTemplates.Items.Add(template.Title);
                }
                txtTitle.Text = "";
                txtMessage.Text = "";
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var err in ex.EntityValidationErrors)
                {
                    sb.AppendLine($"Entity of type {err.Entry.GetType().Name} in state {err.Entry.State} has the following validation errors:");
                    foreach (var val in err.ValidationErrors)
                    {
                        sb.AppendLine($"- {val.PropertyName}: {val.ErrorMessage}");
                    }
                    sb.AppendLine();
                }
                MessageBox.Show(sb.ToString(), "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = _unitOfWork.Templates.GetId(lstTemplates.Text);
            if (id != null)
            {
                var template = _unitOfWork.Templates.GetRecord((int)id);
                _unitOfWork.Templates.Remove(template);
                _unitOfWork.Complete();
                MessageBox.Show("The template and all of its associated translations have been removed.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstTemplates.Items.Remove(lstTemplates.Text);
                cmbLangVersions.Items.Clear();
                txtTitle.Text = "";
                txtMessage.Text = "";
                cmbLang.Text = "";
                cmbCategory.Text = "";
            }
        }

        private void btnDeleteVersion_Click(object sender, EventArgs e)
        {
            if (cmbLangVersions.Text == "English")
            {
                MessageBox.Show("The English version can only be removed by removing the template itself.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cmbLangVersions.Text == "")
            {
                return;
            }

            var version = _unitOfWork.Versions.FirstOrDefault(v => v.Language.Name == cmbLangVersions.Text);
            _unitOfWork.Versions.Remove(version);
            _unitOfWork.Complete();
        }

        private void btnNewVersion_Click(object sender, EventArgs e)
        {
            if (cmbLangVersions.Text == cmbLang.Text)
            {
                MessageBox.Show($"There is already an '{cmbLangVersions.Text}' version.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cmbLang.Text == "English")
            {
                MessageBox.Show("You cannot create another 'English' version as this is created by default.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cmbCategoryVersions.Text != cmbCategory.Text)
            {
                MessageBox.Show($"The category '{cmbCategoryVersions.Text}' must be kept the same for all versions of a template.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var version = new Models.Version()
            {
                Message = txtMessage.Text,
                TemplateId = _unitOfWork.Templates.GetId(lstTemplates.Text),
                LanguageId = _unitOfWork.Languages.GetId(cmbLang.Text),
            };
            _unitOfWork.Versions.Add(version);
            _unitOfWork.Complete();

            // LastOrDefault/Last methods are not supported with this SQLite implementation, so we do it this way.
            var versionCreated = _unitOfWork.Versions
                .FirstOrDefault(v => v.TemplateId == version.TemplateId && v.LanguageId == version.LanguageId);

            MessageBox.Show($"The '{cmbLang.Text}' version of the selected template was created with the ID: {versionCreated.VersionId} successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cmbLangVersions.Items.Add(cmbLang.Text);
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            var template = _unitOfWork.Templates
                .FirstOrDefault(t => t.Title == lstTemplates.Text);
            //if (version == null)
            //{
            //    return;
            //}
            template.Title = txtTitle.Text;
            //template.CategoryId = _unitOfWork.Categories.GetId(cmbCategory.Text);
            _unitOfWork.EditRecord(template, t => t.Title);
            //_unitOfWork.EditRecord(template, t => t.CategoryId);
            _unitOfWork.Complete();
            MessageBox.Show("Changes to the template were saved successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lstTemplates.Items.Remove(lstTemplates.Text);
            lstTemplates.Items.Add(txtTitle.Text);
            cmbLangVersions.Items.Clear();
            txtTitle.Text = "";
            txtMessage.Text = "";
            cmbLang.Text = "";
            cmbCategory.Text = "";
        }

        private void btnSaveVersionChanges_Click(object sender, EventArgs e)
        {
            var version = _unitOfWork.Versions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);
            //if (version == null)
            //{
            //    return;
            //}
            version.Message = txtMessage.Text;
            _unitOfWork.EditRecord(version, v => v.Message);
            _unitOfWork.Complete();
            MessageBox.Show($"Changes to the '{cmbLangVersions.Text}' version were saved successfully.", "QTemplates", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbLangVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var version = _unitOfWork.Versions
                .FirstOrDefault(v => v.Template.Title == lstTemplates.Text && v.Language.Name == cmbLangVersions.Text);
            //if (version == null)
            //{
            //    return;
            //}
            txtTitle.Text = version.Template.Title;
            txtMessage.Text = version.Message;
            cmbLang.Text = version.Language.Name;
            cmbCategory.Text = version.Template.Category.Name;

        }

        private void lstTemplates_Click(object sender, EventArgs e)
        {
            if (lstTemplates.Text == "")
            {
                return;
            }
            var versions = _unitOfWork.Versions.GetVersionsWithAll()
                .Where(v => v.Template.Title == lstTemplates.Text)
                .ToList();
            cmbLangVersions.Items.Clear();
            versions.ForEach(v => cmbLangVersions.Items.Add(v.Language.Name));
            cmbLangVersions.SelectedIndex = 0;
        }
    }
}