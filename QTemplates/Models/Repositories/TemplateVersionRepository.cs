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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTemplates.Models.Repositories.Interfaces;

namespace QTemplates.Models.Repositories
{
    public sealed class TemplateVersionRepository : Repository<TemplateVersion>, ITemplateVersionRepository
    {
        private readonly AppDbContext _context;

        public TemplateVersionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<TemplateVersion> GetVersionsWithAll()
        {
            // The Includes are to eager load the navigation properties.
            return _context.TemplateVersions
                .Include(v => v.Language)
                .Include(v => v.Template)
                .Include(v => v.Template.Category)
                .OrderBy(v => v.Template.Title)
                .ToList();
        }
    }
}
