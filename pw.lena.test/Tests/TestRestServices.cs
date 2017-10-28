using Microsoft.VisualStudio.TestTools.UnitTesting;
using pw.lena.Core.Data.Services.WebServices;
using pw.lena.CrossCuttingConcerns;
using pw.lena.test.Ninject;
using System.Threading.Tasks;
using pw.lena.Core.Data.Models;
using PlatformAbstractions.Helpers;
using System.Collections.Generic;

namespace pw.lena.test.Tests
{
    [TestClass]
    public class TestRestServices
    {
        #region var
        string config = FactorySingleton.Factory.Get<Configuration>().RestServerUrl;
        int timeout = FactorySingleton.Factory.Get<Configuration>().ServerTimeOut;        
        string hash = "hash";
        string CRC = "CRC";
        int typedevice = 1;
        int BotTypeDeviceID = 2;
        #endregion

        [TestMethod]
        public async Task TestPairDeviceService()
        {
            var restS = new RestService(config, "GetCodePW2"); //Wrong Model
            restS.Timeout = timeout;
            CodeResponce codeResponce;               
            Assert.IsNull(await GetResponce(restS), "Message: " + config + "/api/GetCodePW/post return error");

            restS = new RestService(config, "GetCodePW"); //Good Model
            restS.Timeout = timeout;
            codeResponce = await GetResponce(restS);
            Assert.IsNotNull(codeResponce, "Message: " + config + "/api/GetCodePW/post return error");
            Assert.AreEqual(codeResponce.ResultCode, 0, "Message: " + config + "/api/vacations/post return result code not 0");
            Assert.AreEqual(codeResponce.Hash, hash + CRC, "Message: " + config + "/api/vacations/post return hash not  token + CRC");
            Assert.AreEqual(codeResponce.Code.ToString().Length, 6, "Message: " + config + "/api/vacations/post return code not 6 length");

            var a = codeResponce.Code;
            codeResponce = await GetResponce(restS);
            var b = codeResponce.Code;
            Assert.AreNotEqual(a, b, "Message: " + config + "/api/vacations/post return code a not unique");
        }

        /// <summary>
        /// Need create in DB data for hash = 'hash'
        ///  DECLARE	@return_value int
        ///  EXEC @return_value = [dbo].[bGetMasters]
        ///  @hash = N'hash'
        ///  SELECT	'Return Value' = @return_value
        /// </summary>
        /// <returns></returns>        
        
        [TestMethod]
        public async Task TestGetMastersService()
        {
            //StringAssert.StartsWith("sdas", "s");
            var restS = new RestService(config, "MasterPW2"); //Wrong Model
            restS.Timeout = timeout;
            List<Master> listMasters;
            Assert.IsNull(await GetResponceMaster(restS), "Message: " + config + "/api/MasterPW/post return error");

            restS = new RestService(config, "MasterPW"); //Good Model
            restS.Timeout = timeout;
            listMasters = await GetResponceMaster(restS);
            //check for null if empty
            Assert.IsNotNull(listMasters, "Message: " + config + "/api/bGetMasters/get return null - no DATA!");
            if (listMasters != null && listMasters.Count != 0)
            {
                Assert.AreNotEqual(listMasters[0].MasterId, 0, "Message: SQL exception if MasterId == 0");
                Assert.AreEqual(listMasters[0].TypeDeviceID, BotTypeDeviceID, "Message: BotTypeDeviceID not 2");
                Assert.AreEqual(listMasters[0].codeB.ToString().Length, 6, "Message: code B not 6 length");
            }
            else
            {
                Assert.Fail("listMasters.Count == 0 - empty data? Error - need null");
            }   
        }

        /// <summary>
        /// Delete Master Pair in Rest
        /// !!! Need exist Records with  AndroidIDmacHash = 'hash' in [bDevice] and Pair Master in [bDeviceMasterPair]
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestDeleteMastersService()
        {           
            List<Master> listMasters;            
            var restS = new RestService(config, "MasterPW");
            restS.Timeout = timeout;
            listMasters = await GetResponceMaster(restS);
            long MasterID = 0;
            Assert.IsNotNull(listMasters, "Message: " + config + "/api/bGetMasters/get return null - no DATA!");
            /// !!! Need exist Records with  AndroidIDmacHash = 'hash' in [bDevice] and Pair Master in [bDeviceMasterPair]
            if (listMasters != null && listMasters.Count != 0)
            {
                MasterID = listMasters[0].MasterId;
                if (MasterID == 0)
                {
                    Assert.Fail("MasterID == 0 - Error - must been not 0");
                }
                else
                {
                    var coderequest = req();
                    coderequest.TypeDeviceID = MasterID;
                    await restS.Delete(coderequest);
                    listMasters = await GetResponceMaster(restS);                    
                    Assert.IsNull(listMasters, "Message: " + config + "/api/bGetMasters/get return not null after DELETE!");
                    await restS.Put(coderequest);//Restore master pair
                    listMasters = await GetResponceMaster(restS);
                    Assert.IsNotNull(listMasters, "Message: " + config + "/api/bGetMasters/get return null after RESTORE!");
                }
            }
            else
            {
                Assert.Fail("listMasters.Count == 0 - empty data? Error - need null");
            }
        }

        private async Task<CodeResponce> GetResponce(RestService restService)
        {            
            return await Helper.Complete(await restService.PostAndGet<CodeResponce>(req()));
        }

        private async Task<List<Master>> GetResponceMaster(RestService restService)
        {
            return  await restService.Get<Master>(req());
        }

        private CodeRequest req()
        {
            return new CodeRequest
            {
                AndroidIDmacHash = hash,
                CRC = CRC,
                TypeDeviceID = typedevice
            };
        }
    }
}
