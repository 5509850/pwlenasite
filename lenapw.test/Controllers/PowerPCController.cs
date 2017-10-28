using lenapw.test.Helpers;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class PowerPCController : ApiController
    {

        #region VAR
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        private MLDBUtils.SQLCom MyCom;        
        private int NOT_FOUND_DEVICEID = -2;
        private int SQL_ERROR = -3;
        private int NotActive = -777;
        
        #endregion

        public IEnumerable<PowerPC> Get(string hash, string CRC)
        {
            if (hash == null)
            {
                List<PowerPC> res = new List<PowerPC>();
                res.Add(new PowerPC { GUID = "PW slave API"});
                return res;
            }
            connectionString = Utils.InitSqlPath(connectionString);
            return getPowerPC(hash);
        }
        // GET: PowerPC
        public CodeResponce Post(CodeRequestData<PowerPC> requestdata)
        {
            if (requestdata == null)
            {
                return null;
            }
            //save powertime pc    
            int result = 0;
            int code = SavePowerPC(new Device { TypeDeviceID = (int)requestdata.TypeDeviceID, AndroidIDmacHash = requestdata.AndroidIDmacHash }, requestdata.data).Result;
            if (code > 0)
            {
                result = code;
            }
            return new CodeResponce { Code = code, Hash = requestdata.AndroidIDmacHash + requestdata.CRC, ResultCode = result };
        }

        private async Task<int> SavePowerPC(Device device, List<PowerPC> powerpcs)
        {
            connectionString = Utils.InitSqlPath(connectionString);
            int result = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            MyCom = new MLDBUtils.SQLCom(connectionString, "");
            int count = 0;
            int newrecords = 0;
            try
            {
                string guidnew = string.Empty;
                foreach (var ppc in powerpcs)
                {
                
                MyCom.clearParams();
                MyCom.setCommand("sSetpowerTime");
                MyCom.AddParam(device.AndroidIDmacHash);
                MyCom.AddParam(device.TypeDeviceID);
                MyCom.AddParam(ppc.GUID);
                MyCom.AddParam(ppc.dateTimeOnPC);
                MyCom.AddParam(ppc.dateTimeOffPC);
                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    result = 0;
                    break;
                }
                int.TryParse(dic["c"].ToString(), out count);
                int.TryParse(dic["n"].ToString(), out newrecords);
                    if (newrecords == 1)
                    {
                        guidnew = ppc.GUID;
                    }
                    if (count == NOT_FOUND_DEVICEID || count == NotActive)
                    {
                        result = count;
                        break;
                    }
                    else
                    {
                        result += count;
                    }
                }
                if (newrecords == 1 && !string.IsNullOrEmpty(guidnew))
                {
                    SendAutoInfoToMaster(guidnew, device.AndroidIDmacHash);
                }
            }

            catch (Exception ex)
            {
                var err = ex.Message;
                result = SQL_ERROR;
            }
            return result;         
        }

        private async void SendAutoInfoToMaster(string guidnew, string androidIDmacHash)
        {
            int chatID = 0;//NEED GET FROM SQL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            DataTable table = new DataTable();
            StringBuilder sb = new StringBuilder();
            WebHookPwController whc = new WebHookPwController();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");                
                MyCom.setCommand("bAutoSendPowerPCToMasterBot");
                MyCom.AddParam(androidIDmacHash);
                MyCom.AddParam((int)TypeDevicePW.TelegramBotMaster);
                MyCom.AddParam(guidnew);                
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return;
                }               
                //for all master paired with slave sending message with info powerON
                foreach (DataRow row in table.Rows)
                {
                   
                  sb.Append(row["name"]);
                  sb.Append(" (");
                  sb.Append(row["type"]);
                  sb.Append(")");
                  sb.Append(Environment.NewLine);
                  sb.Append(Environment.NewLine);
                  sb.Append("power ON: ");
                  sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["on"])));                  
                  sb.Append(Environment.NewLine);
                  sb.Append("-----------------------------");
                  sb.Append(Environment.NewLine);
                  sb.Append(string.Format("sync: {0: hh:mm:ss tt (d MMM)} ", Convert.ToDateTime(row["sync"])));                  
                  sb.Append(Environment.NewLine);
                  sb.Append(Environment.NewLine);
                  sb.Append("/menu");
                  chatID = 0;
                  int.TryParse(row["chatID"].ToString(), out chatID);
                  if (chatID != 0 && sb.Length != 0)
                    {                        
                        MyCom.clearParams();
                        MyCom.setCommand("sCreateMessForSending");
                        MyCom.AddParam(chatID);
                        MyCom.AddParam(sb.ToString());                        
                        MyCom.ExecuteCommand();                        
                    }
                  sb.Clear();
                }
                //Send To Bot Command Sending Message from [bMessForSending]             !!!!!!!!!!!!!!!
                whc.Get(777);
            }
            catch (Exception ex)
            {
                whc.SaveErrorLog(chatID, "SendAutoInfoToMaster():" + ex.Message);
            }            
        }

        public static Task<T> Complete<T>(T result)
        {
            var r = new TaskCompletionSource<T>();
            r.SetResult(result);
            return r.Task;
        }

        private List<PowerPC> getPowerPC(string hash)
        {
            DataTable table = new DataTable();
            string result = string.Empty;
            var res = new List<PowerPC>();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("bGetPowerPCByDevice");
                MyCom.AddParam(DateTime.Now.Date); //only for today power on
                MyCom.AddParam(hash);
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return null;
                }

                foreach (DataRow row in table.Rows)
                {
                    res.Add(new PowerPC
                    {
                        GUID = row["GUID"].ToString(),
                        dateTimeOnPC = Convert.ToDateTime(row["dateTimeOnPC"]),
                        dateTimeOffPC = Convert.ToDateTime(row["dateTimeOffPC"]),
                        synchronizeTime = Convert.ToDateTime(row["synchronizeTime"])
                    });
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;               
                res.Add(new PowerPC { GUID = "SQL error " + err });
                return res;
            }
            return res;
        }

       
    }
}