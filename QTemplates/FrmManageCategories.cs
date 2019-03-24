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
    public partial class FrmManageCategories : Form
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrmManageCategories(IUnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmManageCategories_Load(object sender, EventArgs e)
        {
            _unitOfWork.Categories.GetAll()
                .Select(c => c.Name)
                .ToList()
                .ForEach(c => lstCat.Items.Add(c));
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var category = new Category()
            {
                Name = txtCat.Text.Trim(),
            };
            _unitOfWork.Categories.Add(category);

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("The category has been added successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstCat.Items.Add(category.Name);
                lstCat.Text = category.Name;
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to update the '{lstCat.Text}' category?",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var category = _unitOfWork.Categories.FirstOrDefault(c => c.Name == lstCat.Text);
            category.Name = txtCat.Text.Trim();

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("Changes to the category were saved successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstCat.Items.Remove(lstCat.Text);
                lstCat.Items.Add(category.Name);
                txtCat.Text = "";
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int inUse = _unitOfWork.Categories.GetAll()
                .FirstOrDefault(c => c.Name == lstCat.Text)?
                .Templates.Count() ?? -1;

            if (inUse > 0)
            {
                MessageBox.Show($"You cannot delete this category because it has {inUse} template{(inUse > 1 ? "s " : " ")}assigned to it.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete the '{lstCat.Text}' category? Don't worry, there are no templates assigned to it.",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var category = _unitOfWork.Categories.FirstOrDefault(c => c.Name == lstCat.Text);
            _unitOfWork.Categories.Remove(category);

            try
            {
                _unitOfWork.Complete();
                MessageBox.Show("The category has been deleted successfully.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstCat.Items.Remove(lstCat.Text);
                txtCat.Text = "";
            }
            catch (DbUpdateException ex)
            {
                _unitOfWork.UndoChanges();
                MessageBox.Show($"Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtCat_TextChanged(object sender, EventArgs e)
        {
            ValidateCategoryControls();
        }

        private void LstCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCat.Text = lstCat.Text;
        }

        /// <summary>
        /// The following are the category validation rules:
        /// Create:
        ///     Input field must be filled.
        ///     Category must not be created already.
        /// Update:
        ///     Input field must be filled.
        ///     List must have a category selected.
        ///     Selected category in list must be different than the one in the input field, at least case-wise.
        /// Delete:
        ///     List must have a category selected.
        /// </summary>
        private void ValidateCategoryControls()
        {
            bool state = (String.IsNullOrWhiteSpace(txtCat.Text) == false);
            btnAdd.Enabled = (state && lstCat.Items.ContainsEx(txtCat.Text.Trim()) == false);
            btnUpdate.Enabled = (state && lstCat.Text != "" && lstCat.Text != txtCat.Text.Trim());
            btnDelete.Enabled = (lstCat.Text != "");
        }
    }
}
