using lenapw.test.Helpers;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace lenapw.test.Controllers
{
    public class ScreenshotController : ApiController
    {
        #region VAR
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        private MLDBUtils.SQLCom MyCom;
             
           
        private int NOT_FOUND_DEVICEID = -2;
        private int SQL_ERROR = -3;
        private int NotActive = -777;       
        #endregion

        public IEnumerable<ScreenShot> Get(string hash, string CRC)
        {
            List<ScreenShot> res = new List<ScreenShot>();
            res.Add(new ScreenShot { GUID = "PW slave API" });
            return res;
         
        }
       
        public CodeResponce Post(CodeRequestData<ScreenShot> requestdata)
        {
            if (requestdata == null)
            {
                return null;
                }
            //save powertime pc    
            int result = 0;
            int code = SaveScreenShotPC(new Device { TypeDeviceID = (int)requestdata.TypeDeviceID, AndroidIDmacHash = requestdata.AndroidIDmacHash }, requestdata.data).Result;
            if (code > 0)
            {
                result = code;
            }
            return new CodeResponce { Code = code, Hash = requestdata.AndroidIDmacHash + requestdata.CRC, ResultCode = result };
        }


        private void SaveToDisk(byte[] b)
        {
            try
            {
                string targetFolder = HttpContext.Current.Server.MapPath("~/uploads");
                string targetPath = Path.Combine(targetFolder, "hash.jpg");         
                if (b != null)
                {
                    System.IO.File.WriteAllBytes(targetPath, b.ToArray()); //.toArray
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
        }

        private async Task<SavedResult> SaveToSql(Device device, ScreenShot sst)
        {
            SavedResult saveresult = null;
            try
            {                
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"EXEC sSetScreenShot @hash, @TypeDeviceID, @GUID, @QueueCommandID, @dateCreate, @img ";
                        command.Parameters.Add("@hash", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@TypeDeviceID", SqlDbType.Int);
                        command.Parameters.Add("@GUID", SqlDbType.NVarChar, 50);                    
                        command.Parameters.Add("@QueueCommandID", SqlDbType.BigInt);
                        command.Parameters.Add("@dateCreate", SqlDbType.SmallDateTime);
                        command.Parameters.Add("@img", SqlDbType.Image, 1000000);                       

                        command.Parameters["@hash"].Value = device.AndroidIDmacHash;
                        command.Parameters["@TypeDeviceID"].Value = device.TypeDeviceID;
                        command.Parameters["@GUID"].Value = sst.GUID;
                        command.Parameters["@QueueCommandID"].Value = sst.QueueCommandID;
                        command.Parameters["@dateCreate"].Value = sst.dateCreate;
                        command.Parameters["@img"].Value = sst.ImageScreen;
                    
                    SqlDataReader reader = command.ExecuteReader();

                    saveresult = new SavedResult();
                    while (reader.Read())
                    {
                        saveresult.Count = reader.GetInt32(0);
                        saveresult.NewRecords = reader.GetInt32(1);                        
                    }                    
                }             
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
            return await Utils.Complete(saveresult);
        }
       
        private async Task<int> SaveScreenShotPC(Device device, List<ScreenShot> screenshot)
        {
            connectionString = Utils.InitSqlPath(connectionString);
            int result = 0;          
            try
            {
                string guidnew = string.Empty;
                int newrecords = 0;
                foreach (var sst in screenshot)
                {
                    var savedResult = await SaveToSql(device, sst);                   
                    
                    if (savedResult == null)
                    {
                        result = 0;
                        break;
                    }                   
                    if (savedResult.NewRecords == 1)
                    {
                        guidnew = sst.GUID;
                        newrecords = 1;
                    }
                    if (savedResult.Count == NOT_FOUND_DEVICEID || savedResult.Count == NotActive)
                    {
                        result = savedResult.Count;
                        break;
                    }
                    else
                    {
                        result += savedResult.Count;
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
            return await  Utils.Complete(result);
        }

        private async void SendAutoInfoToMaster(string guidnew, string androidIDmacHash)
        {
            int chatID = 0;
            int screenshotID = 0;

            DataTable table = new DataTable();
            StringBuilder sb = new StringBuilder();
            WebHookPwController whc = new WebHookPwController();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bAutoSendScreenShotToMasterBot");
                MyCom.AddParam(androidIDmacHash);
                MyCom.AddParam((int)TypeDevicePW.TelegramBotMaster);
                MyCom.AddParam(guidnew);
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return;
                }
                //for [QueueCommandID] master (chatid) paired with slave sending message with screenshot
                foreach (DataRow row in table.Rows)
                {

                    sb.Append(row["name"]);
                    sb.Append(" (");
                    sb.Append(row["type"]);
                    sb.Append(")");
                    sb.Append(Environment.NewLine);
                    sb.Append("ScreenShot: ");                    
                    sb.Append(Environment.NewLine);
                    sb.Append("-----------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append(string.Format("sync: {0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["synchronizeTime"])));
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("/menu");
                    chatID = 0;
                    screenshotID = 0;
                    int.TryParse(row["chatID"].ToString(), out chatID);
                    int.TryParse(row["screenshotid"].ToString(), out screenshotID);                    
                    if (chatID != 0 && sb.Length != 0)
                    {
                        MyCom.clearParams();
                        MyCom.setCommand("sCreateMessPhotoForSending");
                        MyCom.AddParam(chatID);
                        MyCom.AddParam(screenshotID);
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
                whc.SaveErrorLog(chatID, "ScreenshotController:SendAutoInfoToMaster():" + ex.Message);
            }
        }

        public static Task<T> Complete<T>(T result)
        {
            var r = new TaskCompletionSource<T>();
            r.SetResult(result);
            return r.Task;
        }

        public List<ScreenShot> getScreenShotPC(string hash, string GUID)
        {
            DataTable table = new DataTable();
            string result = string.Empty;
            var res = new List<ScreenShot>();

            try
            {
               
              MyCom = new MLDBUtils.SQLCom(connectionString, "");

              MyCom.setCommand("bGetScreenShotByDevice");                
              MyCom.AddParam(GUID); 
              MyCom.AddParam(hash);
              table = MyCom.GetResult();

              if (table == null || table.Rows.Count == 0)
              {
                  return null;
              }

              foreach (DataRow row in table.Rows)
              {
                  res.Add(new ScreenShot
                  {
                      GUID = row["GUID"].ToString(),
                      ImageScreen = (byte[])row["ImageScreen"], //Utils.ObjectToByteArray(row["ImageScreen"]),//Encoding.UTF8.GetBytes(row["ImageScreen"].ToString()),//Utils.ObjectToByteArray(row["ImageScreen"]),
                      dateCreate = Convert.ToDateTime(row["dateCreate"]),                        
                      synchronizeTime = Convert.ToDateTime(row["synchronizeTime"])
                  });
              }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                res.Add(new ScreenShot { GUID = "SQL error " + err });
                return res;
            }
           
            return res;
        }       

        public class SavedResult
        {
            public int Count { get; set; }
            public int NewRecords { get; set; }            
        }
    }
}