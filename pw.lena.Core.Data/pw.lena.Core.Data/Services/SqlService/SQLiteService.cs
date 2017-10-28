using SQLite.Net.Async;
using SQLite.Net.Interop;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using SQLite.Net;
using pw.lena.Core.Data.Models.SQLite;
using System.Linq.Expressions;

namespace pw.lena.Core.Data.Services.SqlService
{

    #region new Repository

    ///https://stackoverflow.com/questions/29050400/generic-repository-for-sqlite-net-in-xamarin-project

    public interface IRepository<T> where T : class
    {
        Task<List<T>> Get();
        Task<T> Get(int id);
        Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        AsyncTableQuery<T> AsQueryable();
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private SQLiteAsyncConnection db;

        public Repository(ISQLitePlatform sqlitePlatform, string dbPath)
        {
            this.db = new SQLiteAsyncConnection(() => CreateConnection(sqlitePlatform, dbPath));
            new CreateDB(db);
        }

        public SQLiteConnectionWithLock CreateConnection(ISQLitePlatform sqlitePlatform, string dbPath)
        {
            try
            {
                SQLiteConnectionString connectionString = new SQLiteConnectionString(dbPath, false);
                SQLiteConnectionWithLock connection = new SQLiteConnectionWithLock(sqlitePlatform, connectionString);
                return connection;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public AsyncTableQuery<T> AsQueryable() =>
            db.Table<T>();

        public async Task<List<T>> Get() =>
            await db.Table<T>().ToListAsync();

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

        public async Task<T> Get(int id) =>
             await db.FindAsync<T>(id);

        public async Task<T> Get(Expression<Func<T, bool>> predicate) =>
            await db.FindAsync<T>(predicate);

        public async Task<int> Insert(T entity) =>
             await db.InsertAsync(entity);

        public async Task<int> Update(T entity) =>
             await db.UpdateAsync(entity);

        public async Task<int> Delete(T entity) =>
             await db.DeleteAsync(entity);
    }

    #endregion


    public class SQLiteService<T> where T : class
    {
        private SQLiteAsyncConnection _db;

        public SQLiteService(ISQLitePlatform sqlitePlatform, string dbPath)
        {
            _db = new SQLiteAsyncConnection(() => CreateConnection(sqlitePlatform, dbPath));
            new  CreateDB(_db);
        } 

        public SQLiteConnectionWithLock CreateConnection(ISQLitePlatform sqlitePlatform, string dbPath)
        {
            try
            {
                SQLiteConnectionString connectionString = new SQLiteConnectionString(dbPath, false);
                SQLiteConnectionWithLock connection = new SQLiteConnectionWithLock(sqlitePlatform, connectionString);
                return connection;
            }
            catch (ArgumentException)
            {
                throw;              
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<T> Get(string id) 
        {
            try
            {
                return await _db.FindAsync<T>(id);
            }
            catch (SQLiteException ex)
            {
                var err = ex.Message;
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<List<T>> Get() 
        {
            try
            {                
                return await _db.Table<T>().ToListAsync();
            }
            catch (SQLiteException)
            {
             
                return null;
            }
        }

        public async Task<int> Insert(T item)
        {
            int result = 0;
            try
            {
                result = await _db.InsertOrIgnoreAsync(item);
            }
            catch (SQLiteException ex)
            {
                var err = ex.Message;
                result = -1;
            }
            return result;
        }

        public async Task<int> Delete(string id) 
        {
            int result = 0;
            try
            {
                result = await _db.DeleteAsync<T>(id);
            }
            catch (SQLiteException)
            {
                result = -1;
            }
            return result;
        }

        public async Task<int> Update(T item)
        {
            int result = 0;
            try
            {
                result = await _db.UpdateAsync(item);
            }
            catch (SQLiteException)
            {
                result = -1;
            }
            return result;
        }

        /// <summary>
        ///         var search = await stockRepo.Get<T>(x => x.ProductName.StartsWith("something"));
        ///         var orderedSearch = await stockRepo.Get(predicate: x => x.DaysToExpire < 4, orderBy: x => x.EntryDate);
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<List<T>> GetWhere<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null) 
        {
            var query = _db.Table<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = query.OrderBy<TValue>(orderBy);

            return await query.ToListAsync();
        }

    }

    public class CreateDB
    {
        SQLiteAsyncConnection db;
        public CreateDB(SQLiteAsyncConnection db)
        {
            this.db = db;
            if (!IsExist<CodeResponceSQL>())
            {
                Task task = Task.Run(async () => await db.CreateTableAsync<CodeResponceSQL>());
                task.Wait();
            }
            if (!IsExist<MasterSQL>())
            {
                Task task = Task.Run(async () => await db.CreateTableAsync<MasterSQL>());
                task.Wait();
            }

            if (!IsExist<PrefSql>())
            {
                Task task = Task.Run(async () => await db.CreateTableAsync<PrefSql>());
                task.Wait();
            }

            if (!IsExist<PowerPCSQL>())
            {
                Task task = Task.Run(async () => await db.CreateTableAsync<PowerPCSQL>());
                task.Wait();
            }
            if (!IsExist<ScreenShotSQL>())
            {
                Task task = Task.Run(async () => await db.CreateTableAsync<ScreenShotSQL>());
                task.Wait();
            }
        }

        private bool IsExist<T>() where T : class
        {
            try
            {
                Task<T> task = Task.Run(async () => await db.Table<T>().FirstOrDefaultAsync());
                task.Wait();
                return true;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return false;
            }
        }
    }

    /*SQLite type data
INTEGER - Числовой тип данных (целые положительные или отрицательные числа). Данный тип данных имеет переменный размер 1,2,3,4,6 или 8 байтов. Максимальный размер для данных данного типа состовляет 8 байтов и может хранить числовые значения в диапазоне [-9223372036854775808,-1,0,1,-9223372036854775807]. SQLite автоматически изменяет размер данного типа данных в зависимости от значения.

REAL - Числовой тип данных имеющий размер 8 байтов и может хранить любые (в том числе и не целые) числа.

TEXT - Текстовый тип данных который может хранить текстовые строки произвольной длины в кодировке UTF-8 или UTF-16. Максимальная длина строки для данного типа данных не лимитирована. Этот же тип данных используется для хранения даты и времени.

BLOB - Тип данных для хранения двоичных объектов. Максимальный размер данных данного типа не лимитирован.
     */
}