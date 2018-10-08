/*
 * 
 * Copyright 2018 Monojit Datta

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Subsystems.CustomSQliteORM.Internal;

namespace Subsystems.CustomSQliteORM.External
{
    public class CMPSqliteORMProxy
    {

        #region Private Variables(CMPSqliteORMProxy)
        private readonly CMPSqliteORM _sqliteORM;
        #endregion

        #region Public Methods(CMPSqliteORMProxy)
        public CMPSqliteORMProxy(string dbPathString)
        {

            _sqliteORM = CMPSqliteORM.SharedInstance;
            _sqliteORM.Initialize(dbPathString);

        }

        public bool Create<T>() where T : new()
        {

            var couldCreate = _sqliteORM.Create<T>();
            return couldCreate;

        }

        public async Task<bool> CreateAsync<T>() where T : new()
        {

            var createResult = await _sqliteORM.CreateAsync<T>();
            return createResult;

        }

        public bool Insert(CMPModelBase item)
        {

            var couldInsert = _sqliteORM.Insert(item);
            return couldInsert;

        }

        public async Task<bool> InsertAsync(CMPModelBase item)
        {

            var insertResult = await _sqliteORM.InsertAsync(item);
            return insertResult;

        }

        public bool InsertAll(List<CMPModelBase> itemsList)
        {

            var couldInsert = _sqliteORM.InsertAll(itemsList);
            return couldInsert;

        }

        public async Task<bool> InsertAllAsync(List<CMPModelBase> itemsList)
        {

            var insertAllResult = await _sqliteORM.InsertAllAsync(itemsList);
            return insertAllResult;

        }

        public List<T> Fetch<T>(Predicate<T> predicate = null) where T : new()
        {

            var fetchList = _sqliteORM.Fetch<T>(predicate);
            return fetchList;

        }

        public async Task<List<T>> FetchAsync<T>(Predicate<T> predicate = null) where T : new()
        {

            var fetchResult = await _sqliteORM.FetchAsync<T>(predicate);
            return fetchResult;

        }

        public bool Update(CMPModelBase item)
        {

            var result = _sqliteORM.Update(item);
            return result;

        }

        public async Task<bool> UpdateAsync(CMPModelBase item)
        {

            var updateResult = await _sqliteORM.UpdateAsync(item);
            var result = updateResult;
            return result;

        }

        public bool Delete(CMPModelBase item)
        {

            var result = _sqliteORM.Delete(item);
            return result;

        }

        public async Task<bool> DeleteAsync(CMPModelBase item)
        {

            var deleteResult = await _sqliteORM.DeleteAsync(item);
            var result = deleteResult;
            return result;

        }
        #endregion

    }
}
