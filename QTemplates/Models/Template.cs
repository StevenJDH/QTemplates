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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTemplates.Classes;

namespace QTemplates.Models
{
    public class Template
    {
        public Template()
        {
            TemplateVersions = new HashSet<TemplateVersion>();
        }

        public int TemplateId { get; set; }

        [Required]
        [StringLength(40)]
        [Index("ix_template_title", IsUnique = true)]
        public string Title { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [InverseProperty("Templates")]
        public Category Category { get; set; }

        [InverseProperty("Template")]
        public ICollection<TemplateVersion> TemplateVersions { get; set; }
    }
}
