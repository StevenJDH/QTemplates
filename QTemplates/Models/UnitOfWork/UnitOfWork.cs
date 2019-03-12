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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QTemplates.Models.Repositories;
using QTemplates.Models.Repositories.Interfaces;

namespace QTemplates.Models.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private bool _isDisposed;

        public ITemplateRepository Templates { get; private set; }
        public ILanguageRepository Languages { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IVersionRepository Versions { get; private set; }
        public bool IsDisposed { get; private set; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            IsDisposed = false;
            Templates = new TemplateRepository(_context);
            Languages = new LanguageRepository(_context);
            Categories = new CategoryRepository(_context);
            Versions = new VersionRepository(_context);
        }

        public void EditRecord<TEntity>(TEntity entity, Expression<Func<TEntity, string>> predicate)
            where TEntity : class
        {
            _context.Entry(entity).Property(predicate).IsModified = true;
        }

        public void UndoChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public int Complete() => _context.SaveChanges();

        public void Dispose()
        {
            _context.Dispose();
            IsDisposed = true;
        }
    }
}
