using lenapw.test.Helpers;
using pw.lena.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class MasterPWController :  ApiController
    {
        //https://host/api/masterpw
        #region VAR
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        private MLDBUtils.SQLCom MyCom;       
        
        private string EMPTY_SQL = "-1";
        private string NOT_UNIQUE = "-2";
        private string SQL_ERROR = "-3";
        //GET – this operation is used to retrieve data from the web service.
        //POST – this operation is used to create a new item of data on the web service.
        //PUT – this operation is used to update an item of data on the web service.
        //PATCH – this operation is used to update an item of data on the web service by describing a set of instructions about how the item should be modified.This verb is not used in the sample application.
        //DELETE – this operation is used to delete an item of data on the web service.
        #endregion

        public string Get()
        {
            return "PW slave API";
        }

        public IEnumerable<Master> Get(string  hash, string CRC)
        {
            if (hash == null)
            {
                List<Master> res = new List<Master>();
                res.Add(new Master { codeB = 0, MasterId = 0, Name = "PW slave API", TypeDeviceID = 0, TypeName = "Master" });
                return res;
            }
            connectionString = Utils.InitSqlPath(connectionString);
            return getMasters(hash);
        }

        public bool Delete(string id, string hash, string CRC)
        {
            if (id != null && hash != null && CRC != null)
            {
                connectionString = Utils.InitSqlPath(connectionString);
                long masterID = 0;
                long.TryParse(id, out masterID);
                return DeleteMaster(hash, masterID);
            }
            return false;
        }

        public bool Put(string id, string hash, string CRC)
        {
            if (id != null && hash != null && CRC != null)
            {
                connectionString = Utils.InitSqlPath(connectionString);
                long masterID = 0;
                long.TryParse(id, out masterID);
                return RestoreMaster(hash, masterID);
            }
            return false;
        }

        #region Private methods

        private List<Master> getMasters(string hash)
        {
            DataTable table = new DataTable();
            string result = string.Empty;
            var res = new List<Master>();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("sGetMasters");
                MyCom.AddParam(hash);
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }
                            
                foreach (DataRow row in table.Rows)
                {
                    res.Add(new Master {                        
                        MasterId = Convert.ToInt32(row["MasterID"]),
                        codeB = Convert.ToInt32(row["codeB"]),
                        Name = row["name"].ToString(),
                        TypeDeviceID = Convert.ToInt32(row["typedeviceID"]),
                        TypeName = row["typedevice"].ToString(),
                        IsActive = Convert.ToBoolean(row["isActive"])
                    });                    
                }                              
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                res = new List<Master>();
                res.Add(new Master { codeB = 0, MasterId = 0, Name = err, TypeDeviceID = 0, TypeName = "ERROR SQL" });
                return res;
            }
            return res;
        }

        private bool DeleteMaster(string hash, long masterID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            var res = new List<Master>();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("sDeletePair");
                MyCom.AddParam(hash);
                MyCom.AddParam(masterID);
                dic = MyCom.GetResultD();

                if (dic == null || dic.Count == 0)
                {
                    return false;
                }
                result = dic["ok"].ToString();
                
            }
            catch (Exception ex)
            {
                var err = ex.Message;                
                return false;
            }
            return result.Equals("0");
        }

        private bool RestoreMaster(string hash, long masterID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            var res = new List<Master>();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("sRestorePair");
                MyCom.AddParam(hash);
                MyCom.AddParam(masterID);
                dic = MyCom.GetResultD();

                if (dic == null || dic.Count == 0)
                {
                    return false;
                }
                result = dic["ok"].ToString();

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return false;
            }
            return result.Equals("0");
        }

       
        #endregion
    }
 }