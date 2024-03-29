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

using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace QTemplates.Models
{
    class SqliteConfiguration : DbConfiguration
    {
        public SqliteConfiguration()
        {
            this.SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            this.SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            this.SetProviderServices("System.Data.SQLite",
                SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)) as DbProviderServices);
        }
    }
}
