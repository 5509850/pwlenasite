using pw.lena.Core.Data.Services.DataService.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using WeakEvent;
using pw.lena.Core.Data.Services.SqlService;
using pw.lena.Core.Data.Converters;
using PlatformAbstractions.Interfaces;
using SQLite.Net.Interop;
using pw.lena.CrossCuttingConcerns.Interfaces;
using pw.lena.Core.Data.Models.SQLite;
using pw.lena.CrossCuttingConcerns.Helpers;
using System.Linq;
using pw.lena.Core.Data.Services.WebServices;

namespace pw.lena.Core.Data.Services.DataService
{
    public class PowerPcService : IPowerPcService
    {        

        private readonly WeakEventSource<EventArgs> powerTimeChanged = new WeakEventSource<EventArgs>();

        private SQLiteService<PowerPCSQL> sqliteService = null;
        private ModelConverter converter;
        private IFileSystemService fileSystemService;
        private ISQLitePlatform sqlitePlatform;
        private IConfiguration configuration;
        private ILocalizeService localizservice;
        private string CRC = "ASDASDYHRdasf"; //only for test TODO: add service for secure data check with CRC for block 


        public PowerPcService(
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

        public event EventHandler<EventArgs> PowerTimeChanged
        {
            add { powerTimeChanged.Subscribe(value); }
            remove { powerTimeChanged.Unsubscribe(value); }
        }

        private void OnDataChanged()
        {
            powerTimeChanged.Raise(this, EventArgs.Empty);
        }

        public Task DeletePowerTime(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePowerTimeRest(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public Task DeletePowerTimeSql()
        {
            throw new NotImplementedException();
        }

        public Task GetPowerTime(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PowerPC>> GetPowerTimeRest(DeviceModel device)
        {
            string result = String.Empty;
            List<PowerPC> codeResponce = null;
            try
            {
                RestService restService = new RestService(configuration.RestServerUrl, "PowerPC");
                restService.Timeout = configuration.ServerTimeOut;
                //for test = (string hash, string crc, int type)
                codeResponce = await restService.Get<PowerPC>
                    (new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash,
                        CRC = this.CRC,
                        TypeDeviceID = device.TypeDeviceID });

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
            return codeResponce;
        }

        public async Task<IEnumerable<PowerPC>> GetSQLPowerTime(DateTime date)
        {
            List<PowerPC> listPowerPCs = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PowerPCSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    long d = ConverterHelper.ConvertDateWithoutTimeToMillisec(date);
                    //List<PowerPCSQL> listPowerPCSql = await sqliteService.GetWhere<PowerPCSQL>(x => x.Date.Equals(d));
                    List<PowerPCSQL> listPowerPCSql = await sqliteService.GetWhere(predicate: x => x.Date.Equals(d) && x.IsActive, orderBy: x => x.Date);
                    if (listPowerPCSql != null && listPowerPCSql.Count != 0)
                    {
                        listPowerPCs = new List<PowerPC>();
                        foreach (var powerPCSql in listPowerPCSql)
                        {
                            listPowerPCs.Add(converter.ConvertToPowerPC(powerPCSql));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
            return listPowerPCs;
        }

        public async Task<IEnumerable<PowerPC>> GetSQLPowerTime(DateTime from, DateTime to)
        {
            List<PowerPC> listPowerPCs = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PowerPCSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    long f = ConverterHelper.ConvertDateWithoutTimeToMillisec(from);
                    long t = ConverterHelper.ConvertDateWithoutTimeToMillisec(to);
                    //List<PowerPCSQL> listPowerPCSql = await sqliteService.GetWhere<PowerPCSQL>(x => x.Date >= f && x.Date <= t); 
                    List<PowerPCSQL> listPowerPCSql = await sqliteService.GetWhere(predicate: x => x.Date >= f && x.Date <= t && x.IsActive, orderBy: x => x.Date);
                    if (listPowerPCSql != null && listPowerPCSql.Count != 0)
                    {
                        listPowerPCs = new List<PowerPC>();
                        foreach (var powerPCSql in listPowerPCSql)
                        {
                            listPowerPCs.Add(converter.ConvertToPowerPC(powerPCSql));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
            return listPowerPCs;
        }

        public async Task<int> SavePowerTimeToSql(PowerPC powerpc)
        {
            int result = 0;
            if (powerpc == null)
            {
                return result;
            }
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PowerPCSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
                }
            }
            catch (Exception exp)
            {
                result = -1;
                sqliteService = null;
            }
            if (sqliteService != null)
            {
                try
                {
                    //only for new guid insert new records, for old GUID only update timeOff
                    List<PowerPCSQL> oldpowerpc = await sqliteService.GetWhere<PowerPCSQL>(x => x.GUID.Equals(powerpc.GUID) && x.IsActive);
                    if (oldpowerpc != null && oldpowerpc.Count != 0)
                    {                        
                        foreach (var ppc in oldpowerpc)
                        {                                                        
                                ppc.dateTimeOffPC = ConverterHelper.ConvertDateTimeToMillisec(powerpc.dateTimeOffPC).ToString();
                                ppc.IsSynchronized = false;
                                result = await sqliteService.Update(ppc);                         
                        }
                        
                    }
                    else
                    {
                       result =  await sqliteService.Insert(converter.ConvertToPowerPCSQL(powerpc));
                    }
                }
                catch (Exception ex)
                {
                    result = -1;
                    var err = ex.Message;
                    throw ex;
                }
            }
            return result;
        }       
        public async Task<int> SynchronizePowerTimeRest(DeviceModel device, DateTime date)
        {
            //save powertime to rest service from local sql
            string result = String.Empty;
            CodeResponce codeResponce = null;
            List<PowerPC> listPowerPC = null;
            try
            {
                var allpower = await GetSQLPowerTime(date);
                if (allpower != null)
                {
                    listPowerPC = allpower.ToList();
                    var notsyncpower = listPowerPC.Where(x => !x.IsSynchronized);
                    if (notsyncpower != null)
                    {
                        listPowerPC = notsyncpower.ToList();
                    }
                    
                }
                
                
                if (listPowerPC != null && listPowerPC.Count != 0)
                {
                    RestService restService = new RestService(configuration.RestServerUrl, "PowerPC");
                    restService.Timeout = configuration.ServerTimeOut;
                    //for test = (string hash, string crc, int type)
                    codeResponce = await restService.PostAndGet<CodeResponce>
                        (new CodeRequestData<PowerPC>
                        {
                            AndroidIDmacHash = device.AndroidIDmacHash,
                            CRC = this.CRC,
                            TypeDeviceID = device.TypeDeviceID,
                            data = listPowerPC
                        });
                }
                else
                {
                    return 0;
                }
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
            if (codeResponce != null)
            {
                int update = 0;
                if (codeResponce.ResultCode == 0 && codeResponce.Code < 0)
                {
                    return codeResponce.Code;
                }
                else
                {
                    if (codeResponce.ResultCode != 0 && listPowerPC != null && listPowerPC.Count != 0)
                    {
                        update = await UpdateSynchToSql(listPowerPC);
                    }
                    if (update == codeResponce.ResultCode)
                    {
                        return codeResponce.ResultCode;
                    }
                }
            }
            return -1;
        }

        private async Task<int> UpdateSynchToSql(List<PowerPC> listPowerPC)
        {
            int result = 0;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<PowerPCSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
                }
            }
            catch (Exception exp)
            {
                result = -1;
                sqliteService = null;
            }
            if (sqliteService != null)
            {
                try
                {
                    foreach (var powerpc in listPowerPC)
                    {
                        List<PowerPCSQL> oldpowerpc = await sqliteService.GetWhere<PowerPCSQL>(x => x.GUID.Equals(powerpc.GUID));
                        if (oldpowerpc != null && oldpowerpc.Count != 0)
                        {
                            foreach (var ppc in oldpowerpc)
                            {
                                ppc.IsSynchronized = true;
                                result += await sqliteService.Update(ppc);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = -1;
                    var err = ex.Message;
                    throw ex;
                }
            }
            return result;
        }

    }

}
