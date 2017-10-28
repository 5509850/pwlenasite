using PlatformAbstractions.Interfaces;
using pw.lena.Core.Data.Models.Enums;
using pw.lena.Core.Data.Models.SQLite;
using pw.lena.Core.Data.Services.DataService.Contracts;
using pw.lena.Core.Data.Services.SqlService;
using pw.lena.CrossCuttingConcerns.Interfaces;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Services.DataService
{
    public class PreferenceService : IPreferenceService
    {
        private SQLiteService<PrefSql> sqliteService = null;
        private IFileSystemService fileSystemService;
        private ISQLitePlatform sqlitePlatform;
        private IConfiguration configuration;
        private ILocalizeService localizservice;

        public PreferenceService(IConfiguration configuration,
            IFileSystemService fileSystemService,
            ISQLitePlatform sqlitePlatform,
            ILocalizeService localizservice)
        {
            this.configuration = configuration;
            this.fileSystemService = fileSystemService;
            this.sqlitePlatform = sqlitePlatform;
            this.localizservice = localizservice;
        }

        public async Task<string> GetPrefValue(PrefEnums key)
        {
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PrefSql>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
                }
            }
            catch (Exception exp)
            {
                sqliteService = null;
            }
            if (sqliteService != null)
            {
                try
                {                    
                    var oldprefSql = await sqliteService.Get(((int)key).ToString());
                    if (oldprefSql != null)
                    {
                        return oldprefSql.Value;
                    }                   
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }               
            }
            return null;
        }

        public async Task<bool> SavePrefValue(PrefEnums key, string value)
        {
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PrefSql>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
                }
            }
            catch (Exception exp)
            {
                sqliteService = null;
            }
            if (sqliteService != null)
            {
                try
                {
                    var oldprefSql = await sqliteService.Get(((int)key).ToString());
                    if (oldprefSql != null)
                    {
                        oldprefSql.Value = value;
                        await sqliteService.Update(oldprefSql);
                    }
                    else
                    {
                        await sqliteService.Insert(new PrefSql { Id = (int)key, Value = value });
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;                   
                }                
            }
            return false;
        }

        public async Task ClearPreference()
        {
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PrefSql>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
                }
            }
            catch (Exception exp)
            {
                var err = exp.Message;
                sqliteService = null;
            }
            if (sqliteService != null)
            {
                try
                {
                    List<PrefSql> list = await sqliteService.Get();
                    if (list != null && list.Count != 0)
                    {
                        foreach (var item in list)
                        {
                            await sqliteService.Delete(item.Id.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
        }
    }
}
