using lenapw.test.Helpers;
using pw.lena.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class GetCommandController : ApiController
    {
        #region VAR
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        private MLDBUtils.SQLCom MyCom;             
        private int NOT_FOUND_DEVICEID = -2;
        private int SQL_ERROR = -3;
        private int NotActive = -777;       
        #endregion

        public IEnumerable<Command> Get(string hash, string CRC)
        {
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(CRC))
            {
                return null;
            }
            connectionString = Utils.InitSqlPath(connectionString);
            List<Command> res = new List<Command>();
            if (string.IsNullOrEmpty(hash))
            {                
                res.Add(new Command { commandID = 555 });
                return res;
            }            
            
            return getCommand(hash);

        }

        public List<Command> getCommand(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return null;
            }
            DataTable table = new DataTable();
            string result = string.Empty;
            var res = new List<Command>();

            try
            {

                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("sGetCommand");                
                MyCom.AddParam(hash);
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }

                foreach (DataRow row in table.Rows)
                {
                    res.Add(new Command
                    {
                        commandID = Convert.ToInt32(row["commandID"]),
                        QueueCommandID = Convert.ToInt64(row["QueueCommandID"]),
                        dateCreate = Convert.ToDateTime(row["dataCreate"])
                    });
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                res.Add(new Command { commandID = -1});
                return res;
            }

            return res;
        }


      
    }
}