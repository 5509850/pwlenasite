using pw.lena.Core.Data.Models;
using pw.lena.Core.Data.Services.DataService;
using pw.lena.Core.Data.Services.WebServices;
using pw.lena.CrossCuttingConcerns;
using pw.lena.CrossCuttingConcerns.Interfaces;
using slave.maket.test.Ninject;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slave.maket.test
{
    public class TEST
    {
        private PairDeviceService pairDeviceService;
        public async Task GetCodeA(DeviceModel device, TextBox tb)
        {
            string cCRC = "ASDASDYHRdasf"; //only for test
            string result = String.Empty;
            CodeResponce codeResponce = null;
            //pairDeviceService = FactorySingleton.Factory.Get<PairDeviceService>();
            //pairDeviceService.GetCodeA(new DeviceModel { AndroidIDmacHash = "hash", codeA = });          

            //tb.Text = codeResponce.Code.ToString();
            try
            {
                IConfiguration config =  FactorySingleton.Factory.Get<Configuration>();                
                RestService restService = new RestService(config.RestServerUrl, "GetCodePW"); 
                restService.Timeout = 10000;
                //for test = (string hash, string crc, int type)
                codeResponce = await restService.PostAndGet<CodeResponce>(new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash, CRC = cCRC, TypeDeviceID = device.TypeDeviceID });

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
                tb.Text = codeResponce.Code.ToString();
                await UpdateOrCreateCodeAToSql(codeResponce);
            }
        }

        private async Task UpdateOrCreateCodeAToSql(CodeResponce codeResponce)
        {
            try
            {
                pairDeviceService = FactorySingleton.Factory.Get<PairDeviceService>();              
                await pairDeviceService.UpdateOrCreateCodeAToSql(codeResponce);
            }
            catch (Exception e)
            {
                var err = e.Message;
            }            
        }
        
     

        private class SQLitePlatformTest : ISQLitePlatform
        {
            private SQLitePlatformWin32 _sqlitePlatformWin32;
            public SQLitePlatformTest()
            {
                _sqlitePlatformWin32 = new SQLitePlatformWin32();
            }

            public IReflectionService ReflectionService { get { return _sqlitePlatformWin32.ReflectionService; } }
            public ISQLiteApi SQLiteApi { get { return _sqlitePlatformWin32.SQLiteApi; } }
            public IStopwatchFactory StopwatchFactory { get { return _sqlitePlatformWin32.StopwatchFactory; } }
            public IVolatileService VolatileService { get { return _sqlitePlatformWin32.VolatileService; } }
        }
              
    }
    //public class DeviceModel
    //{
    //    public string Name { get; set; }

    //    public int TypeDeviceID { get; set; }

    //    public string Token { get; set; }

    //    public string AndroidIDmacHash { get; set; }

    //    public int codeA { get; set; }
    //    public int codeB { get; set; }
    //}

    //public class CodeResponce
    //{
    //    public int Code { get; set; }

    //    public string Hash { get; set; }

    //    public int ResultCode { get; set; }
    //}
    //public class CodeRequest
    //{
    //    public string CRC { get; set; }

    //    public string AndroidIDmacHash { get; set; }

    //    public int TypeDeviceID { get; set; }

    //}
    //public class RestService
    //{
    //    private string _url;
    //    private int _timeout = 5000;

    //    public RestService(string server, string model)
    //    {
    //        _url = server + @"/api/" + model;
    //    }

    //    public int Timeout { get { return _timeout; } set { _timeout = value; } }        
      
    //    public async Task<T> PostAndGet<T>(object data)
    //    {
    //        //  public async Task<T> Get<T>(int id) where T : class
    //        string url = _url;
    //        try
    //        {
    //            using (var httpClient = new HttpClient())
    //            {
    //                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
    //                HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    //                var response = await httpClient.PostAsync(url, content);
    //                if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
    //                {
    //                    var contentreturn = response.Content.ReadAsStringAsync().Result;
    //                    return JsonConvert.DeserializeObject<T>(contentreturn);
    //                }
    //                else
    //                {
    //                    return JsonConvert.DeserializeObject<T>(string.Empty);
    //                }
    //            }
    //        }
    //        catch (TaskCanceledException)
    //        {
                
    //        }
    //        return JsonConvert.DeserializeObject<T>(string.Empty);
    //    }

    //}

    //public class FileSystemService
    //{
    //    public Task<string> GetPath(string dbName)
    //    {
    //        string filename = dbName + ".db3";
    //        var path = GetFilePath(filename);
    //        return Helper.Complete(path);
    //    }
    //    public Task SaveText(string filename, string text)
    //    {
    //        var filePath = GetFilePath(filename);
    //        System.IO.File.WriteAllText(filePath, text);
    //        return Helper.Complete();
    //    }
    //    public Task<string> LoadText(string filename)
    //    {
    //        var filePath = GetFilePath(filename);
    //        return Helper.Complete(System.IO.File.ReadAllText(filePath));
    //    }
    //    public Task<bool> ExistsFile(string filename)
    //    {
    //        string filepath = GetFilePath(filename);
    //        return Helper.Complete(File.Exists(filepath));
    //    }

    //    #region private methodes
    //    private string GetFilePath(string filename)
    //    {
    //        string docsPath = "D:\\";
    //        return Path.Combine(docsPath, filename);
    //    }
    //    #endregion

    //}

    //public class SQLitePlatformTest : ISQLitePlatform
    //{
    //    private SQLitePlatformWin32 _sqlitePlatformWin32;
    //    public SQLitePlatformTest()
    //    {
    //        _sqlitePlatformWin32 = new SQLitePlatformWin32();
    //    }

    //    public IReflectionService ReflectionService { get { return _sqlitePlatformWin32.ReflectionService; } }
    //    public ISQLiteApi SQLiteApi { get { return _sqlitePlatformWin32.SQLiteApi; } }
    //    public IStopwatchFactory StopwatchFactory { get { return _sqlitePlatformWin32.StopwatchFactory; } }
    //    public IVolatileService VolatileService { get { return _sqlitePlatformWin32.VolatileService; } }
    //}
}
