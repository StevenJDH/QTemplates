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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QTemplates.Extensions;
using QTemplates.Models;
using QTemplates.Models.UnitOfWork;

namespace QTemplates
{
    public partial class FrmManageLanguages : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrmManageLanguages(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmManageLanguages_Load(object sender, EventArgs e)
        {
            _unitOfWork.Languages.GetAll()
                .Where(l => l.Name != "English") // English is the required default so we can't edit it.
                .Select(l => l.Name)
                .ToList()
                .ForEach(l => lstLang.Items.Add(l));
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var lang = new Language()
            {
                Name = txtLang.Text.Trim(),
            };
            _unitOfWork.Languages.Add(lang);

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("The language has been added successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstLang.Items.Add(lang.Name);
                lstLang.Text = lang.Name;
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update the '{lstLang.Text}' language?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var lang = _unitOfWork.Languages.FirstOrDefault(l => l.Name == lstLang.Text);
            lang.Name = txtLang.Text.Trim();

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("Changes to the language were saved successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstLang.Items.Remove(lstLang.Text);
                lstLang.Items.Add(lang.Name);
                txtLang.Text = "";
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int inUse = _unitOfWork.Languages.GetAll()
                            .FirstOrDefault(l => l.Name == lstLang.Text)?
                            .TemplateVersions.Count() ?? -1;

            if (inUse > 0)
            {
                MessageBox.Show($"You cannot delete this language because it has {inUse} template{(inUse > 1 ? "s " : " ")}assigned to it.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete the '{lstLang.Text}' language? Don't worry, there are no templates assigned to it.",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var lang = _unitOfWork.Languages.FirstOrDefault(l => l.Name == lstLang.Text);
            _unitOfWork.Languages.Remove(lang);

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("The language has been deleted successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstLang.Items.Remove(lstLang.Text);
                txtLang.Text = "";
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtLang_TextChanged(object sender, EventArgs e)
        {
            ValidateLanguageControls();
        }

        private void LstLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLang.Text = lstLang.Text;
        }

        /// <summary>
        /// The following are the language validation rules:
        /// Create:
        ///     Input field must be filled and not contain the required English default language.
        ///     Language must not be created already.
        /// Update:
        ///     Input field must be filled and not contain the required English default language.
        ///     List must have a language selected.
        ///     Selected language in list must be different than the one in the input field, at least case-wise.
        /// Delete:
        ///     List must have a language selected.
        /// </summary>
        private void ValidateLanguageControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtLang.Text) == false && 
                          txtLang.Text.Trim().ToUpper() != "ENGLISH");
            btnAdd.Enabled = (state && lstLang.Items.ContainsEx(txtLang.Text.Trim()) == false);
            btnUpdate.Enabled = (state && lstLang.Text != "" && lstLang.Text != txtLang.Text.Trim());
            btnDelete.Enabled = (lstLang.Text != "");
        }
    }
}
