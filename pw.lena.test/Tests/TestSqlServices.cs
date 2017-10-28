using Microsoft.VisualStudio.TestTools.UnitTesting;
using pw.lena.test.Ninject;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using pw.lena.Core.Data.Services.DataService;
using System.Threading;
using pw.lena.Core.Data.Services.DataService.Contracts;
using System.Collections.Generic;
using System.Linq;
using Moq;
using pw.lena.Core.Data.Models.Enums;

namespace pw.lena.test.Tests
{
    [TestClass]
    public class TestSqlServices 
    {
        const int code = 123456;
        const long masterID = 654321;
        const string pinhashDemo = "123hash";
        private Mock<Master> master;

        [TestMethod]
        public async Task TestSQLPairDeviceService()
        {         
            await InitData();
            IPairDeviceService pairDeviceService = FactorySingleton.FactoryOffline.Get<PairDeviceService>();
            Pair pair = await pairDeviceService.GetPair();
            Assert.IsNull(pair, "Error: pairDeviceService.GetPair() not null after delete");
            await pairDeviceService.UpdateOrCreateCodeAToSql(new CodeResponce { Code = code, Hash = "hash", ResultCode = 0 });
            pair = await pairDeviceService.GetPair();
            Assert.IsNotNull(pair, "Error: pairDeviceService.GetPair Is Null");
            Assert.AreEqual(pair.CodeA, code, "pair.CodeA not 123456");
            Assert.AreEqual(pair.ErrorMessage, string.Empty, "ErrorMessage not empty");
            Assert.AreNotEqual(pair.TimeOutValidCodeA, 0, "TimeOutValidCodeA = 0");
            Assert.IsFalse(pair.isCodeAExpired, "CodeA is Expired!");

            Thread.Sleep((pair.TimeOutValidCodeA * 1000 + 10));
            pair = await pairDeviceService.GetPair();
            Assert.IsNotNull(pair, "Error: pairDeviceService.GetPair Is Null");
            Assert.AreEqual(pair.CodeA, code, "pair.CodeA not 123456");
            Assert.AreEqual(pair.ErrorMessage, string.Empty, "ErrorMessage not empty");
            Assert.AreNotEqual(pair.TimeOutValidCodeA, 0, "TimeOutValidCodeA = 0");
            Assert.IsTrue(pair.isCodeAExpired, "CodeA is not Expired!");
            await pairDeviceService.DeletePair();
            pair = await pairDeviceService.GetPair();
            Assert.IsNull(pair, "Error: pairDeviceService.GetPair() not null after delete");
        }

        [TestMethod]
        public async Task TestSQLMasterServiceGet()
        {
            await InitData();
            IMastersService mastersService = FactorySingleton.FactoryOffline.Get<MastersService>();
            List<Master> listMasters = new List<Master>();            
            Assert.IsNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() not null after delete");
            master = new Mock<Master>();
            master.Object.codeB = code;
            listMasters.Add(master.Object);
            await mastersService.SaveMastersToSql(listMasters);
            Assert.IsNotNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() null after save");
            IEnumerable<Master> m = await mastersService.GetSQLPairedMasters();
            listMasters = m.ToList();
            Assert.IsNotNull(listMasters, "Error: pairDeviceService.GetPair Is Null");
            Assert.AreEqual(listMasters[0].codeB, code, "codeB not eq code");
            await mastersService.ClearMasterPair();
            Assert.IsNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() not null after delete");
        }

        [TestMethod]
        public async Task TestSQLMasterServiceDelete()
        {
            await InitData();
            IMastersService mastersService = FactorySingleton.FactoryOffline.Get<MastersService>();
            List<Master> listMasters = new List<Master>();
            Assert.IsNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() not null after delete");
            master = new Mock<Master>();
            master.Object.codeB = code;
            master.Object.MasterId = masterID;
            listMasters.Add(master.Object);
            master = new Mock<Master>();
            listMasters.Add(master.Object);
            await mastersService.SaveMastersToSql(listMasters);
            Assert.IsNotNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() null after save");
            IEnumerable<Master> m = await mastersService.GetSQLPairedMasters();            
            listMasters = m.ToList();            
            Assert.IsNotNull(listMasters, "Error: pairDeviceService.GetPair Is Null");
            Assert.AreEqual(listMasters.Count, 2, "Error - not 2 item saved and get!!!");
            Assert.AreEqual(listMasters[0].codeB, code, "item 1 - codeB not eq code");
            Assert.AreNotEqual(listMasters[1].codeB, code, "item 2 - codeB eq code");
            Assert.AreEqual(listMasters[0].MasterId, masterID, "item 1 - masterID not eq masterid");
            Assert.AreNotEqual(listMasters[1].MasterId, masterID, "item 2 - masterID eq masterid");

            await mastersService.DeleteMasterPairSql(masterID);
            Assert.IsNotNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() null after Delete one from two pair");
            m = await mastersService.GetSQLPairedMasters();
            listMasters = m.ToList();
            Assert.IsNotNull(listMasters, "Error: pairDeviceService.GetPair Is Null");
            Assert.AreEqual(listMasters.Count, 1, "Error - not 1 item after Delete and Get!!!");            
            Assert.AreNotEqual(listMasters[0].codeB, code, "codeB eq code");            
            Assert.AreNotEqual(listMasters[0].MasterId, masterID, "masterID eq masterid");

            await mastersService.ClearMasterPair();
            Assert.IsNull(await mastersService.GetSQLPairedMasters(), "Error: mastersService.GetSQLPairedMasters() not null after delete");
        }


        [TestMethod]
        public async Task TestSQLServicePreferenceService()
        {
            await InitData();
            IPreferenceService preferenceService = FactorySingleton.FactoryOffline.Get<PreferenceService>();            
            Assert.IsNull(await preferenceService.GetPrefValue(PrefEnums.PinSecurityHash), "Error: preferenceService.GetPrefValue() not null after delete");
            string pinhash = pinhashDemo;
            bool result = await preferenceService.SavePrefValue(PrefEnums.PinSecurityHash, pinhash);
            Assert.IsTrue(result, "Error: preferenceService.SavePrefValue");
            Assert.IsNotNull(await preferenceService.GetPrefValue(PrefEnums.PinSecurityHash), "Error: preferenceService.GetPrefValue() null after save");
            pinhash = await preferenceService.GetPrefValue(PrefEnums.PinSecurityHash);
            Assert.AreEqual(pinhash, pinhashDemo, "Error - not equal after save and get!");            
            await preferenceService.ClearPreference();
        }

        [ClassInitialize]
        private async Task InitData()
        {         
            IPairDeviceService pairDeviceService = FactorySingleton.FactoryOffline.Get<PairDeviceService>();
            IMastersService mastersService = FactorySingleton.FactoryOffline.Get<MastersService>();
            IPreferenceService preferenceService = FactorySingleton.FactoryOffline.Get<PreferenceService>();  
            await pairDeviceService.DeletePair();
            await mastersService.ClearMasterPair();
            await preferenceService.ClearPreference();
        }       
    }
}
