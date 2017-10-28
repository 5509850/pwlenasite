using pw.lena.Core.Data.Services.DataService.Contracts;
using System;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Interfaces;
using PlatformAbstractions.Interfaces;
using SQLite.Net.Interop;
using pw.lena.Core.Data.Services.SqlService;
using pw.lena.Core.Data.Services.WebServices;
using WeakEvent;
using pw.lena.Core.Data.Models.SQLite;
using pw.lena.CrossCuttingConcerns.Helpers;

namespace pw.lena.Core.Data.Services.DataService
{
    public class PairDeviceService : IPairDeviceService
    {
        private SQLiteService<CodeResponceSQL> sqliteService = null;
        private IFileSystemService fileSystemService;
        private ISQLitePlatform sqlitePlatform;
        private readonly WeakEventSource<EventArgs> codeAChangedEventSource = new WeakEventSource<EventArgs>();
        private IConfiguration configuration;

        private ILocalizeService localizservice;

        private string CRC = "ASDASDYHRdasf"; //only for test


        public PairDeviceService(
            IConfiguration configuration,
            IFileSystemService fileSystemService,
            ISQLitePlatform sqlitePlatform,
            ILocalizeService localizservice)
        {
            this.configuration = configuration;
            this.fileSystemService = fileSystemService;
            this.sqlitePlatform = sqlitePlatform;
            this.localizservice = localizservice;
        }


        public event EventHandler<EventArgs> CodeAChanged
        {
            add { codeAChangedEventSource.Subscribe(value); }
            remove { codeAChangedEventSource.Unsubscribe(value); }
        }

        public async Task GetCodeA(DeviceModel device)
        {
            string result = String.Empty;
            CodeResponce codeResponce = null;
            try
            {
                RestService restService = new RestService(configuration.RestServerUrl, "GetCodePW");
                restService.Timeout = configuration.ServerTimeOut;
                //for test = (string hash, string crc, int type)
                codeResponce = await restService.PostAndGet<CodeResponce>(new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash, CRC = this.CRC, TypeDeviceID = device.TypeDeviceID });

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
                await UpdateOrCreateCodeAToSql(codeResponce);
            }
            OnCodeChanged();
        }

        public async Task UpdateOrCreateCodeAToSql(CodeResponce codeResponce)
        {
            if (codeResponce == null)
            {
                return;
            }
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<CodeResponceSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    CodeResponceSQL currentCodeAResponce = new CodeResponceSQL {Id = 1, Code = codeResponce.Code, Hash = codeResponce.Hash, ResultCode = codeResponce.ResultCode, Date = ConverterHelper.ConvertDateTimeToMillisec(DateTime.Now).ToString() };
                    var codeResponcesOld = await sqliteService.Get("1");
                    if (codeResponcesOld != null)
                    {
                        await sqliteService.Update(currentCodeAResponce);
                    }
                    else
                    {
                        await sqliteService.Insert(currentCodeAResponce);
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                    throw ex;
                }
            }
        }

        private void OnCodeChanged()
        {
            codeAChangedEventSource.Raise(this, EventArgs.Empty);
        }

        public Task<CodeResponce> SetCodeB(DeviceModel device)
        {
            throw new NotImplementedException();
        }

        //Get saved to local sql code A
        public async Task<Pair> GetPair()
        {
            Pair pair = null;
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<CodeResponceSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    CodeResponceSQL codeResponcesSQL = await sqliteService.Get("1");
                  
                    if (codeResponcesSQL != null)
                    {
                        //DateTime saved = ConverterHelper.ConvertMillisecToDateTime(Convert.ToInt64(codeResponcesSQL.Date));
                        //DateTime expiredtime = saved.AddSeconds(configuration.TimeOutValidCodeASecond);
                        //DateTime now = DateTime.Now;
                        //bool exp = now > expiredtime;
                        pair = new Pair
                        {
                            CodeA = codeResponcesSQL.Code,
                            CodeB = 0,
                            ErrorMessage = string.Empty,
                            isCodeAExpired = (DateTime.Now > (ConverterHelper.ConvertMillisecToDateTime(Convert.ToInt64(codeResponcesSQL.Date)).AddSeconds(configuration.TimeOutValidCodeASecond))),
                            TimeOutValidCodeA = configuration.TimeOutValidCodeASecond
                        };
                    }
                }
                catch (Exception ex)
                {
                    pair = new Pair
                    {
                        CodeA = 0,
                        CodeB = 0,
                        ErrorMessage = ex.Message,
                        isCodeAExpired = true,
                        TimeOutValidCodeA = configuration.TimeOutValidCodeASecond
                    };
                }
            }

            return pair;// await Helper.Complete(pair);
        }

        public async Task DeletePair()
        {
            try
            {
                if (sqliteService == null)
                {
                    sqliteService = new SQLiteService<CodeResponceSQL>(sqlitePlatform, await fileSystemService.GetPath(configuration.SqlDatabaseName));
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
                    await sqliteService.Delete("1");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
