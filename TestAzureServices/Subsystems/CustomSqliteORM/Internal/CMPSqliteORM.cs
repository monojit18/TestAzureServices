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
using Diagnostics = System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using Subsystems.CustomSQliteORM.External;

namespace Subsystems.CustomSQliteORM.Internal
{

    public sealed class CMPSqliteORM
	{

        #region Private Variables
        private string _dbPathString;
        private SemaphoreSlim _dbSemaphore;
		#endregion

        #region Private/Protected Methods
        private CMPSqliteORM() { }

        private List<T> FindMatchingItems<T>(Predicate<T> predicate, List<T> allItemsList)
        {

            if (predicate == null)
                return allItemsList;

            var foundItemsList = allItemsList.FindAll(predicate);
            return foundItemsList;

        }

        private class Nested
        {

            static Nested() { }
            internal static readonly CMPSqliteORM _instance = new CMPSqliteORM();

        }
        #endregion

        #region Public Methods
        public static CMPSqliteORM SharedInstance
        {
            get
            {
                return Nested._instance;
            }
        }

        public void Initialize(string dbPathString)
        {

            if (string.IsNullOrEmpty(_dbPathString) == true)
                _dbPathString = string.Copy(dbPathString);

            if (_dbSemaphore == null)
                _dbSemaphore = new SemaphoreSlim(1);

        }

        public bool Create<T>() where T : new()
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var result = sqliteConnection.CreateTable<T>();
                    sqliteConnection.Close();
                    return (result == 0);

                }
            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return false;

            }
        }

        public async Task<bool> CreateAsync<T>() where T : new()
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteConnection = new SQLiteAsyncConnection(_dbPathString);
                var createTableResult = await sqliteConnection.CreateTableAsync<T>();

                _dbSemaphore.Release();
                return (createTableResult == CreateTableResult.Created);

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }
            catch (AggregateException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }

        }

        public bool Insert(CMPModelBase item)
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var result = sqliteConnection.Insert(item);
                    sqliteConnection.Close();
                    return (result == 0);

                }
            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return false;

            }

        }

        public async Task<bool> InsertAsync(CMPModelBase item)
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteAsyncConnection = new SQLiteAsyncConnection(_dbPathString);
                var insertResult = await sqliteAsyncConnection.InsertAsync(item);

                _dbSemaphore.Release();
                return (insertResult > 0);

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }

        }

        public bool InsertAll(List<CMPModelBase> itemsList)
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var result = sqliteConnection.InsertAll(itemsList);
                    sqliteConnection.Close();
                    return (result > 0);

                }
            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return false;

            }

        }

        public async Task<bool> InsertAllAsync(List<CMPModelBase> itemsList)
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteAsyncConnection = new SQLiteAsyncConnection(_dbPathString);
                var insertAllResult = await sqliteAsyncConnection.InsertAllAsync(itemsList);

                _dbSemaphore.Release();
                return (insertAllResult > 0);

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }

        }

        public List<T> Fetch<T>(Predicate<T> predicate = null) where T : new()
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var allItemsList = sqliteConnection.Table<T>().ToList();
                    sqliteConnection.Close();

                    var foundItemsList = FindMatchingItems(predicate, allItemsList);
                    return foundItemsList;

                }
            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return null;

            }

        }

        public async Task<List<T>> FetchAsync<T>(Predicate<T> predicate = null) where T : new()
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteAsyncConnection = new SQLiteAsyncConnection(_dbPathString);
                var fetchedResults = await sqliteAsyncConnection.Table<T>().ToListAsync();

                _dbSemaphore.Release();
                var foundItemsList = FindMatchingItems(predicate, fetchedResults);
                return foundItemsList;

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return null;

            }

        }

        public bool Update(CMPModelBase item)
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var result = sqliteConnection.Update(item);
                    sqliteConnection.Close();
                    return (result > 0);

                }
            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return false;

            }
        }

        public async Task<bool> UpdateAsync(CMPModelBase item)
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteAsyncConnection = new SQLiteAsyncConnection(_dbPathString);
                var updateResult = await sqliteAsyncConnection.UpdateAsync(item);

                _dbSemaphore.Release();
                return (updateResult > 0);

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }

        }

        public bool Delete(CMPModelBase item)
        {

            try
            {

                using (var sqliteConnection = new SQLiteConnection(_dbPathString))
                {

                    var result = sqliteConnection.Delete(item);
                    sqliteConnection.Close();
                    return (result > 0);

                }

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                return false;

            }

        }

        public async Task<bool> DeleteAsync(CMPModelBase item)
        {

            try
            {

                await _dbSemaphore.WaitAsync();

                var sqliteAsyncConnection = new SQLiteAsyncConnection(_dbPathString);
                var deleteResult = await sqliteAsyncConnection.DeleteAsync(item);

                _dbSemaphore.Release();
                return (deleteResult > 0);

            }
            catch (SQLiteException exception)
            {

                Diagnostics.Debug.WriteLine(exception.StackTrace);
                _dbSemaphore.Release();
                return false;

            }
        }
        #endregion

	}
}
