using pw.lena.Core.Data.Services.DataService.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Helpers;
using pw.lena.Core.Data.Services.SqlService;
using pw.lena.Core.Data.Converters;
using PlatformAbstractions.Interfaces;
using SQLite.Net.Interop;
using pw.lena.CrossCuttingConcerns.Interfaces;
using WeakEvent;
using pw.lena.Core.Data.Models.SQLite;
using System.Linq;
using pw.lena.Core.Data.Services.WebServices;

namespace pw.lena.Core.Data.Services.DataService
{

    public class ScreenShotPCService : IScreenShotPCService
    {
        private readonly WeakEventSource<EventArgs> screenshotTimeChanged = new WeakEventSource<EventArgs>();

        private SQLiteService<ScreenShotSQL> sqliteService = null;
        private ModelConverter converter;
        private IFileSystemService fileSystemService;
        private ISQLitePlatform sqlitePlatform;
        private IConfiguration configuration;
        private ILocalizeService localizservice;
        private string CRC = "ASDASDYHRdasf";

        public ScreenShotPCService(
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

        public event EventHandler<EventArgs> ScreenShotChanged
        {
            add { screenshotTimeChanged.Subscribe(value); }
            remove { screenshotTimeChanged.Unsubscribe(value); }
        }

        private void OnDataChanged()
        {
            screenshotTimeChanged.Raise(this, EventArgs.Empty);
        }

        public Task DeleteScreenShot(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteScreenShotRest(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public Task DeleteScreenShotSql()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScreenShot>> GetScreenShotRest(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveScreenShotToSql(ScreenShot screenShot)
        {
            int result = 0;
            if (screenShot == null)
            { return result; }

            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<ScreenShotSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    result = await sqliteService.Insert(converter.ConvertToScreenShotSQL(screenShot));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return await TaskHelper.Complete(result);
        }

        public async Task<int> SynchronizeScreenShotRest(DeviceModel device, DateTime date)
        {
            //save powertime to rest service from local sql
            string result = String.Empty;
            CodeResponce codeResponce = null;
            List<ScreenShot> listScreenShot = null;
            try
            {
                var list = await GetSQLScreenShot(date);
                if (list == null)
                {
                    return 0;
                }
                listScreenShot = list.ToList();
                var ll = listScreenShot.Where(x => !x.IsSynchronized);
                if (ll == null)
                {
                    return 0;
                }
                listScreenShot = ll.ToList();
                if (listScreenShot != null && listScreenShot.Count != 0)
                {
                    RestService restService = new RestService(configuration.RestServerUrl, "Screenshot");
                    restService.Timeout = configuration.ServerTimeOut;
                    //for test = (string hash, string crc, int type)
                    codeResponce = await restService.PostAndGet<CodeResponce>
                        (new CodeRequestData<ScreenShot>
                        {
                            AndroidIDmacHash = device.AndroidIDmacHash,
                            CRC = this.CRC,
                            TypeDeviceID = device.TypeDeviceID,
                            data = listScreenShot
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
                    if (codeResponce.ResultCode != 0 && listScreenShot != null && listScreenShot.Count != 0)
                    {
                        update = await UpdateSynchToSql(listScreenShot);
                    }
                    if (update == codeResponce.ResultCode)
                    {
                        return codeResponce.ResultCode;
                    }
                }
            }
            return -1;
        }

        private async Task<int> UpdateSynchToSql(List<ScreenShot> listScreenShot)
        {
            int result = 0;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<ScreenShotSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    foreach (var screenshot in listScreenShot)
                    {
                        List<ScreenShotSQL> oldscreenshot = await sqliteService.GetWhere<ScreenShotSQL>(x => x.GUID.Equals(screenshot.GUID));
                        if (oldscreenshot != null && oldscreenshot.Count != 0)
                        {
                            foreach (var sst in oldscreenshot)
                            {
                                sst.IsSynchronized = true;
                                result += await sqliteService.Update(sst);
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

        public async Task<IEnumerable<ScreenShot>> GetSQLScreenShot(DateTime date)
        {
            List<ScreenShot> listScreenShots = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<ScreenShotSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<ScreenShotSQL> listScreenShotSql = await sqliteService.GetWhere(predicate: x => x.Date.Equals(d) && x.IsActive, orderBy: x => x.Date);
                    if (listScreenShotSql != null && listScreenShotSql.Count != 0)
                    {
                        listScreenShots = new List<ScreenShot>();
                        foreach (var powerPCSql in listScreenShotSql)
                        {
                            listScreenShots.Add(converter.ConvertToScreenShot(powerPCSql));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
            return await TaskHelper.Complete(listScreenShots);
        }

        public async Task<IEnumerable<ScreenShot>> GetSQLScreenShot(DateTime from, DateTime to)
        {
            List<ScreenShot> listScreenShots = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<ScreenShotSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    List<ScreenShotSQL> listScreenShotSql = await sqliteService.GetWhere(predicate: x => x.Date >= f && x.Date <= t && x.IsActive, orderBy: x => x.Date);
                    if (listScreenShotSql != null && listScreenShotSql.Count != 0)
                    {
                        listScreenShots = new List<ScreenShot>();
                        foreach (var powerPCSql in listScreenShotSql)
                        {
                            listScreenShots.Add(converter.ConvertToScreenShot(powerPCSql));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
            return await TaskHelper.Complete(listScreenShots);
        }
    }
}
