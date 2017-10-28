using System;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Interfaces;
using PlatformAbstractions.Interfaces;
using SQLite.Net.Interop;
using pw.lena.Core.Data.Services.SqlService;
using pw.lena.Core.Data.Services.WebServices;
using pw.lena.Core.Data.Services.DataService.Contracts;
using System.Collections.Generic;
using pw.lena.Core.Data.Models.SQLite;
using pw.lena.Core.Data.Converters;
using WeakEvent;
using System.Linq;

namespace pw.lena.Core.Data.Services.DataService
{
    public class MastersService : IMastersService
    {
        private readonly WeakEventSource<EventArgs> listDataChangedSource = new WeakEventSource<EventArgs>();

        private SQLiteService<MasterSQL> sqliteService = null;
        private ModelConverter converter;
        private IFileSystemService fileSystemService;
        private ISQLitePlatform sqlitePlatform;        
        private IConfiguration configuration;
        private ILocalizeService localizservice;
        private string CRC = "ASDASDYHRdasf"; //only for test TODO: add service for secure data check with CRC for block 

        public MastersService(
            ModelConverter converter,
            IConfiguration configuration,
            IFileSystemService fileSystemService,
            ISQLitePlatform sqlitePlatform,
            ILocalizeService localizservice)
        {
            this.converter = converter;
            this.configuration = configuration;
            this.fileSystemService = fileSystemService;
            this.sqlitePlatform = sqlitePlatform;
            this.localizservice = localizservice;
        }

        /// <summary>
        /// Event after date Master changed - read in REST and Save to SQLite, after need read from SQLite - GetSQLPairedMasters
        /// </summary>
        public event EventHandler<EventArgs> ListDataChanged
        {
            add { listDataChangedSource.Subscribe(value); }
            remove { listDataChangedSource.Unsubscribe(value); }
        }

        /// <summary>
        /// event Data changed after get data in Rest service (slow read) and save it to SQLite for Read Data direct from local Sqlite - (fast read)
        /// </summary>
        private void OnDataChanged()
        {
            listDataChangedSource.Raise(this, EventArgs.Empty);
        }
                
        public async Task<IEnumerable<Master>> GetPairedMastersRest(DeviceModel device)
        {
            string result = String.Empty;
            List<Master> responce = null;
            try
            {
                RestService restService = new RestService(configuration.RestServerUrl, "MasterPW");
                restService.Timeout = configuration.ServerTimeOut;                
                responce = await restService.Get<Master>(new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash, CRC = this.CRC, TypeDeviceID = device.TypeDeviceID });

            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions[0].Data.Count > 0)
                {
                    result = e.InnerExceptions[0].Data["message"].ToString();
                }
                else
                {
                    result = "undefinedException";
                }
            }
            catch (Exception e)
            {
                result = String.Format("Error = " + e.Message);
            }            
            return responce;
        }

        /// <summary>
        /// Save Masters to local SQLite for buffer for slow rest internet connections
        /// </summary>
        /// <param name="responce"></param>
        /// <returns></returns>

        public async Task SaveMastersToSql(IEnumerable<Master> responce)
        {
            if (responce == null)
            {
                await ClearMasterPair();
                return;
            }
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<MasterSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<MasterSQL> oldmasters = await sqliteService.Get();
                    if (oldmasters != null && oldmasters.Count != 0)
                    {
                        foreach (var master in oldmasters)
                        {
                            await sqliteService.Delete(master.Id.ToString());                            
                        }
                    }
                    foreach (var master in responce)
                    {
                        await sqliteService.Insert(converter.ConvertToMasterSQL(master));
                    }
                     
                    
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
        }
            
        
          /// <summary>
          /// Delete pair Master from Rest and SQLite
          /// </summary>
          /// <param name="device"></param>
          /// <param name="masterId"></param>
          /// <returns></returns>
        public async Task DeleteMasterPair(DeviceModel device, long masterId)
        {
            bool result = await DeleteMasterPairRest(device, masterId);
            if (result)
            {
                await DeleteMasterPairSql(masterId);
                OnDataChanged();
            }           
        }

        /// <summary>
        /// Clear all data in SQLite local DB
        /// </summary>
        /// <returns></returns>

        public async Task ClearMasterPair()
        {            
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<MasterSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<MasterSQL> oldmasters = await sqliteService.Get();
                    if (oldmasters != null && oldmasters.Count != 0)
                    {
                        foreach (var master in oldmasters)
                        {
                            await sqliteService.Delete(master.Id.ToString());
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
        /// <summary>
        /// Read maters from local SQLite where been saved after read from REST
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<Master>> GetSQLPairedMasters()
        {           
            List<Master> listMasters = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<MasterSQL> (sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<MasterSQL> listMasterSql = await sqliteService.Get();
                    if (listMasterSql != null && listMasterSql.Count != 0)
                    {
                        listMasters = new List<Master>();
                        foreach (var masterSql in listMasterSql)
                        {
                            listMasters.Add(converter.ConvertToMaster(masterSql));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
            return listMasters;
        }

        /// <summary>
        /// Only get from REST and save to SQL = event for read from SQL (GetSQLPairedMasters()) when over
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public async Task GetPairedMasters(DeviceModel device)
        {
            var responce = await GetPairedMastersRest(device);
            if (responce != null)
            {
                List<Master> list = responce.ToList();
                if (list.Count != 0)
                {
                    await SaveMastersToSql(responce);
                }
            }
            else
            {
                await ClearMasterPair();
            }
            OnDataChanged();
        }

        public async Task<bool> DeleteMasterPairRest(DeviceModel device, long masterId)
        {
            string result = String.Empty;
            try
            {
                RestService restService = new RestService(configuration.RestServerUrl, "MasterPW");
                restService.Timeout = configuration.ServerTimeOut;
                var responce = await restService.Delete(new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash, CRC = this.CRC, TypeDeviceID = masterId });
                if (responce.StatusCode != System.Net.HttpStatusCode.NoContent && responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Rest serivece not answer 158");
                }
            }
            catch (Exception e)
            {
                var err = e.Message;
                return false;
            }
            return true;
        }

        public async Task DeleteMasterPairSql(long masterId)
        {
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<MasterSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<MasterSQL> listMasterSql = await sqliteService.Get();
                    if (listMasterSql != null && listMasterSql.Count != 0)
                    {                        
                        foreach (var masterSql in listMasterSql)
                        {
                            if (masterSql.MasterId.Equals(masterId))
                            {
                                await sqliteService.Delete(masterSql.Id.ToString());
                                break;
                            }
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