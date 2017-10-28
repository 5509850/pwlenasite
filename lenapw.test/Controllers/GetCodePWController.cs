using lenapw.test.Helpers;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class GetCodePWController : ApiController
    {
        #region VAR
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        private MLDBUtils.SQLCom MyCom;      
        private string EMPTY_SQL = "-1";
        private string NOT_UNIQUE = "-2";
        private string SQL_ERROR = "-3";
        #endregion

        public string Get()
        {           
            return "PW slave API";                       
        }

        //for test only
        public CodeResponce Get(int id)
        {
            //TestGetCodeB      TODO: need delete!!!! 
            string scode = GetCodeB(id, new Master { ChatId = 213532, TypeDeviceID = (int)TypeDevicePW.TelegramBotMaster, Name = "master name" });
            int code = 0;
            int result = 0;
            int.TryParse(scode, out code);
            if (code < 100000)
            {
                result = code;
            }
            return new CodeResponce { Code = code, Hash = "hash", ResultCode = result };

        }

        //for test only
        public CodeResponce Post(string hash, string crc, int type)
        {
            if (hash == null || crc == null)
            {
                return null;
            }
            //GetCodeA
            //http://localhost:62774/api/GetCodePW?hash=sdfdsf21&crc=checkCode&type=1
            //https://lena.pw/api/GetCodePW?hash=${hash}&crc=${crc}&type=${type}
            int code = 0;
            int result = 0;
            int.TryParse(GetCodeA(new Device { TypeDeviceID = type, AndroidIDmacHash = hash, }), out code);
            if (code < 100000)
            {
                result = code;
            }         
            return new CodeResponce { Code = code, Hash = hash + crc, ResultCode = result };
        }

        /// <summary>
        /// For Slave device save Deivec and get generated CodeA
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CodeResponce Post(CodeRequest request)
        {
            if (request == null)
            {
                return null;
            }
            //GetCodeA        
            int code = 0;
            int result = 0;
            int.TryParse(GetCodeA(new Device { TypeDeviceID = (int)request.TypeDeviceID, AndroidIDmacHash = request.AndroidIDmacHash, }), out code);
            if (code < 100000)
            {
                result = code;
            }
            return new CodeResponce { Code = code, Hash = request.AndroidIDmacHash + request.CRC, ResultCode = result };
        }

        #region private methodes

       
        private string GetCodeA(Device device)
        {
            //-3 exception sql
            //-2 return random not uniq
            //-1 data return empty
            connectionString = Utils.InitSqlPath(connectionString);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("sGETcodeA");
                MyCom.AddParam(Utils.getRandom());
                MyCom.AddParam(device.AndroidIDmacHash); //hash
                MyCom.AddParam(device.TypeDeviceID);

                dic = MyCom.GetResultD();

                if (dic == null || dic.Count == 0)
                {
                    return EMPTY_SQL;
                }
                result = dic["a"].ToString();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return SQL_ERROR;
            }
            if (!result.Equals(NOT_UNIQUE))
            {
                return result;
            }
            return GetCodeA(device);
        }

        private string GetCodeB(int codeA, Master master)
        {
            //-11 not valid code A
            //-12 code A is Expired 2min
            //-22 for code A pair EXIST before!!!
            //-4 unexpected error		
            //-3 exception sql
            //-1 data return empty
            connectionString = Utils.InitSqlPath(connectionString);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bGETcodeB");
                MyCom.AddParam(Utils.getRandom());
                MyCom.AddParam(codeA);
                MyCom.AddParam(master.ChatId);
                MyCom.AddParam(master.TypeDeviceID);
                MyCom.AddParam(master.Name);

                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    return EMPTY_SQL;
                }
                result = dic["b"].ToString();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                result = SQL_ERROR;
            }
            return result;
       }
       #endregion
    }
}