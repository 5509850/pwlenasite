using pw.lena.Core.Data.Models;
using pw.lena.Core.Data.Services.DataService;
using pw.lena.Core.Data.Services.WebServices;
using pw.lena.CrossCuttingConcerns;
using pw.lena.CrossCuttingConcerns.Interfaces;
using pw.lena.slave.winpc.Ninject;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pw.lena.slave.winpc
{
    public class mServices
    {
        byte[] sevenItems = new byte[] { 0x21, 0x20, 0x21, 0x19, 0x18, 0x20, 0x20 };

        string cCRC = "sdG42DFGDF"; //only for test
        private PairDeviceService pairDeviceService;
        private MastersService mastersService;
        private PowerPcService powerpcService;
        private CommandService commandService;
        private ScreenShotPCService screenShotPCService;
        //from REST
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
                IConfiguration config = FactorySingleton.Factory.Get<Configuration>();
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
                    tb.Text = "undefinedException";
                }
            }
            catch (Exception e)
            {
                tb.Text = String.Format(e.Message);
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

        //from rest
        public async Task GetListMasters(DeviceModel device, ListBox lb)
        {
            lb.Items.Clear();
            string result = string.Empty;
            List<Master> listmasters = null;
            try
            {
                IConfiguration config = FactorySingleton.Factory.Get<Configuration>();
                RestService restService = new RestService(config.RestServerUrl, "MasterPW");
                restService.Timeout = 10000;
                listmasters = await restService.Get<Master>(new CodeRequest { AndroidIDmacHash = device.AndroidIDmacHash, CRC = cCRC, TypeDeviceID = device.TypeDeviceID });

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
                result = string.Format("Error = " + e.Message);
            }
            if (listmasters != null && listmasters.Count != 0)
            {
                foreach (var master in listmasters)
                {
                    lb.Items.Add(string.Format("{0} - ({1}) codeB = {2}", master.Name, master.TypeName, master.codeB));
                }

                await UpdateOrCreateMastersToSql(listmasters);
            }
            else
            {
                lb.Items.Add("No pair masters!");
            }
        }

        //from local SQL
        public async Task GetListMastersLocalSql(ListBox lb)
        {
            lb.Items.Clear();
            List<Master> listmasters = null;
            try
            {
                mastersService = FactorySingleton.Factory.Get<MastersService>();
                listmasters = (await mastersService.GetSQLPairedMasters()).ToList();
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
            if (listmasters != null && listmasters.Count != 0)
            {
                foreach (var master in listmasters)
                {
                    lb.Items.Add(string.Format("{0} - ({1}) codeB = {2}", master.Name, master.TypeName, master.codeB));
                }

                await UpdateOrCreateMastersToSql(listmasters);
            }
            else
            {
                lb.Items.Add("No pair masters!");
            }
        }

        public async Task SaveAndGetListPowerTimeFromLocalSql(ListBox lb, DateTime from, DateTime to, string guid)
        {
            lb.Items.Clear();
            await UpdateOrCreatePowerPCtoSql(guid);
            await GetListPowerTimeFromLocalSql(lb, from, to);
        }

        public async Task GetListPowerTimeTODAYFromLocalSql(ListBox lb)
        {
            lb.Items.Clear();
            List<PowerPC> listpowerpcs = null;
            try
            {
                powerpcService = FactorySingleton.Factory.Get<PowerPcService>();
                listpowerpcs = (await powerpcService.GetSQLPowerTime(DateTime.Now.Date))
                                                                                .ToList();
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
            if (listpowerpcs != null && listpowerpcs.Count != 0)
            {
                lb.Items.Add(string.Format("Founds {0} records for {1}", listpowerpcs.Count, DateTime.Now.Date.ToShortDateString()));
                foreach (var powerpc in listpowerpcs)
                {
                    lb.Items.Add(string.Format("{0} - {1} ID = {2} synch = {3}", powerpc.dateTimeOnPC, powerpc.dateTimeOffPC, powerpc.GUID, powerpc.IsSynchronized));
                }
            }
            else
            {
                lb.Items.Add("No powertime by data!");
            }
        }

        public async Task GetListPowerTimeFromLocalSql(ListBox lb, DateTime from, DateTime to)
        {
            lb.Items.Clear();
            List<PowerPC> listpowerpcs = null;
            try
            {
                powerpcService = FactorySingleton.Factory.Get<PowerPcService>();
                listpowerpcs = (await powerpcService.GetSQLPowerTime(from, to))
                                                                                .ToList();
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
            if (listpowerpcs != null && listpowerpcs.Count != 0)
            {
                lb.Items.Add(string.Format("Founds {0} records for {1}", listpowerpcs.Count, DateTime.Now.Date.ToShortDateString()));
                foreach (var powerpc in listpowerpcs)
                {
                    lb.Items.Add(string.Format("{0} - {1} ID = {2} synch = {3}", powerpc.dateTimeOnPC, powerpc.dateTimeOffPC, powerpc.GUID, powerpc.IsSynchronized));
                }
            }
            else
            {
                lb.Items.Add("No powertime by data!");
            }
        }

        private async Task UpdateOrCreateMastersToSql(IEnumerable<Master> codeResponce)
        {
            try
            {
                mastersService = FactorySingleton.Factory.Get<MastersService>();
                await mastersService.SaveMastersToSql(codeResponce);
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
        }

        public async Task<int> UpdateOrCreatePowerPCtoSql(string guid)
        {
            int result = 0;
            PowerPC powerpc = new PowerPC()
            {
                dateTimeOnPC = DateTime.Now,
                dateTimeOffPC = DateTime.Now,
                GUID = guid,
                IsActive = true,
                IsSynchronized = false
            };
            try
            {
                powerpcService = FactorySingleton.Factory.Get<PowerPcService>();
                result = await powerpcService.SavePowerTimeToSql(powerpc);
            }
            catch (Exception e)
            {
                var err = e.Message;
                result = -1;
            }
            return result;
        }


        public async Task<int> SyncSqlWithRest(DeviceModel device, DateTime date)
        {
            int result = 0;
            int result2 = 0;
            try
            {
                powerpcService = FactorySingleton.Factory.Get<PowerPcService>();
                result = await powerpcService.SynchronizePowerTimeRest(device, date);
                screenShotPCService = FactorySingleton.Factory.Get<ScreenShotPCService>();
                result2 = await screenShotPCService.SynchronizeScreenShotRest(device, date);
                if (result2 > 0)
                {
                    result += result2;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                result = -55;
            }
            return result;
        }


        public async Task<List<Command>> SyncCommandWithRest(DeviceModel device)
        {
            List<Command> listcommand = null;
            try
            {
                commandService = FactorySingleton.Factory.Get<CommandService>();
                var list = await commandService.GetCommandRest(device);
                if (list != null)
                {
                    listcommand = list.ToList();
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;                
            }
            return listcommand;
        }


        public async Task SaveAndGetListScreenShotFromLocalSql(ListBox lb, DateTime from, DateTime to, long queueCommandID)
        {
            lb.Items.Clear();
            await UpdateOrCreateScreenShotToSql(queueCommandID);
            await GetLisScreenShotFromLocalSql(lb, from, to);
        }

        private async Task GetLisScreenShotFromLocalSql(ListBox lb, DateTime from, DateTime to)
        {
            lb.Items.Clear();
            List<ScreenShot> listscreenshots = null;
            try
            {
                screenShotPCService = FactorySingleton.Factory.Get<ScreenShotPCService>();
                listscreenshots = (await screenShotPCService.GetSQLScreenShot(from, to))
                                                                                .ToList();
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
            if (listscreenshots != null && listscreenshots.Count != 0)
            {
                lb.Items.Add(string.Format("Founds {0} records for {1}", listscreenshots.Count, DateTime.Now.Date.ToShortDateString()));
                foreach (var screenshot in listscreenshots)
                {
                    lb.Items.Add(string.Format("{0} - {1} ID = {2} synch = {3}", screenshot, screenshot, screenshot.GUID, screenshot.IsSynchronized));
                }
            }
            else
            {
                lb.Items.Add("No screenshot by data!");
            }
        }

      
        private async Task<int> UpdateOrCreateScreenShotToSql(long queueCommandID)
        {
            int result = 0;
            ScreenShot screenshot = new ScreenShot()
            {
                dateCreate = DateTime.Now,
                GUID = Guid.NewGuid().ToString(),
                IsActive = true,
                IsSynchronized = false,
                QueueCommandID = queueCommandID,
                ImageScreen = utils.ScreenShot.MakeScreenShot()
            };
    //        utils.ScreenShot.MakeScreenShot(Application.StartupPath); // need delete - no need save screenshot in disk
            try
            {
                screenShotPCService = FactorySingleton.Factory.Get<ScreenShotPCService>();
                result = await screenShotPCService.SaveScreenShotToSql(screenshot);
            }
            catch (Exception e)
            {
                var err = e.Message;
                result = -1;
            }
            return result;
        }

        public async Task testttttt(DateTime from)
        {           
            List<ScreenShot> listscreenshots = null;
            try
            {
                screenShotPCService = FactorySingleton.Factory.Get<ScreenShotPCService>();
                listscreenshots = (await screenShotPCService.GetSQLScreenShot(from))
                                                                                .ToList();
            
                if (listscreenshots != null && listscreenshots.Count != 0)
                {

                    //byte[] img =  utils.ObjectToByteArray(listscreenshots[0].ImageScreen);
                    byte[] img = (byte[])listscreenshots[0].ImageScreen;
                    
                    if (img != null)
                    {
                        System.IO.File.WriteAllBytes("Foo.jpg", img.ToArray());
                       // System.IO.File.WriteAllBytes("sevenItems.jpg", sevenItems.ToArray());
                        
                    }
                
                }

            }
            catch (Exception e)
            {
                var err = e.Message;
            }

        }

    }
}