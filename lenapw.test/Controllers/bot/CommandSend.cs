using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace lenapw.test.Controllers
{
    public partial class WebHookPwController
    {
        const string MENU_RENAME = "2";
        const string MENU_DELETE = "3";
        const string MENU_STATUS = "4";                       

        private void MenuOperate(string data, long chatid)
        {
            int ID = 1;
            int NAME = 2;

            string[] alldata = data.Split('|');
            if (alldata.Length == 3)
            {
                if (alldata[NAME].Equals("<< Back to Device List"))
                {
                    SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                    MainMenuShow(chatid);
                    return;
                }
                switch (alldata[ID])
                {
                    case MENU_RENAME:
                        {
                            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = status.Data, Name = status.Name, StatusID = RENAME_SLAVE });
                            SendMessage(chatid, string.Format("Enter new NAME for device '{0}' or /CANCEL", status.Name));
                            break;
                        }
                    case MENU_DELETE:
                        {
                            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = status.Data, Name = status.Name, StatusID = DELETE_SLAVE });
                            SendMessage(chatid, "Enter Device NAME for DELETE or /CANCEL");
                            break;
                        }
                    //case MENU_STATUS:
                    //    {
                    //        ShowDeviceStatus(chatid);
                    //        break;
                    //    }
                    default:
                        {
                            SendCommand(chatid, alldata[ID]);
                            break;
                        }
                }
                //TODO DEVICE MENU!!!!!!!!!!!!!!
                //SetStatus(new CurrentStatus { ChatID = chatid, Data = alldata[ID], Name = alldata[NAME], StatusID = SELECTED_SLAVE });
                // SendMessage(chatid, string.Format("id {0}. name {1}", alldata[ID], alldata[NAME]));
                //DeviceMenuShow(chatid, alldata[ID]);
            }
        }

        private async void SendCommand(long chatid, string commandID)
        {
            int command = 0;
            int.TryParse(commandID, out command);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            InitSqlPath();           
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bSendCommand");
                MyCom.AddParam(chatid);
                MyCom.AddParam(command);

                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    SendMessage(chatid, "Error sending command");
                }
                else
                {
                    long QueueCommandID = 0;
                    long.TryParse(dic["i"].ToString(), out QueueCommandID);
                    if (QueueCommandID != 0)
                    {
                        string responce = await ExecuteCommand(command, QueueCommandID);
                        SendMessage(chatid, responce);
                    }
                    else
                    {
                        SendMessage(chatid, "Error SendCommand()");
                    }
                }
                //CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                //Task.Delay(300);
                //DeviceMenuShow(chatid, status.Data, status.Name);
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "SendCommand(): " + ex.Message);
            }
        }

        private async Task<string> ExecuteCommand(int commandID, long QueueCommandID)
        {
            //DateTime start = DateTime.Now;       
            string result = string.Empty;

            switch (commandID)
            {
                case (int)Commands.DeviceStatus:
                    {
                        result = await GetStatusMessage(QueueCommandID);
                        break;
                    }
                case (int)Commands.StartupPCtime:
                    {
                        result = await GetStartUpTimeMessage(QueueCommandID, 0);
                        break;
                    }
                case (int)Commands.Startup3days:
                    {
                        result = await GetStartUpTimeMessage(QueueCommandID, 3);
                        break;
                    }
                case (int)Commands.GetScreenshot:
                    {
                        //Check for online device!!!!
                        bool online = IsDeviceOnline(QueueCommandID);
                        if (online)
                        {
                            result = "Wait please";
                        }
                        else
                        {
                            result = "Device is Offline";
                        }                            
                        
                        break;
                    }
                default:
                    {
                        result = string.Format("ExecuteCommand not define for {0}", commandID);
                        break;
                    }

            }
            //TimeSpan span = DateTime.Now - start;
            //int ms = (int)span.TotalMilliseconds;
            return result;// + string.Format(" Diff = {0}ms", ms); ;
        }

        //get list startup - powertime from slave pc to master telegram bot
        private async Task<string> GetStartUpTimeMessage(long queueCommandID, int days)
        {
            DataTable table = new DataTable();
            StringBuilder sb = new StringBuilder();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                if (days == 0)
                {
                    MyCom.setCommand("bGetStartUpTimeToday");
                }
                else if (days == 3)
                {
                    MyCom.setCommand("bGetStartUpTime");
                }
                MyCom.AddParam(queueCommandID);
                if (days == 3)
                {
                    MyCom.AddParam(DateTime.Now.Date.AddDays(-2));
                    MyCom.AddParam(DateTime.Now.Date);
                }
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return "Today is no information";
                }
                int r = 0;
                string synctime = string.Empty;              
                foreach (DataRow row in table.Rows)
                {                    
                    if (r == 0)
                    {
                        sb.Append(row["name"]);
                        sb.Append(" (");
                        sb.Append(row["type"]);
                        sb.Append(")");
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append("power ON: ");
                    sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["on"])));
                    sb.Append(Environment.NewLine);
                    if (r == (table.Rows.Count - 1)) //last row
                    {
                        synctime = string.Format("sync: {0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["sync"]));
                        if (IsDeviceOnline(queueCommandID))
                        {
                            sb.Append("Device Online");
                        }
                        else
                        {
                            sb.Append("power OFF: ");
                            sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["off"])));
                        }
                    }
                    else
                    {
                        sb.Append("power OFF: ");
                        sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["off"])));
                    }
                    sb.Append(Environment.NewLine);
                    sb.Append("-----------------------------");
                    sb.Append(Environment.NewLine);
                    r++;
                }
                sb.Append(synctime);               
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("/menu");
            }
            catch (Exception ex)
            {
                SaveErrorLog(queueCommandID, "GetStartUpTimeToday(queueCommandID): " + ex.Message);
            }
            return await Complete(sb.ToString());
        }

        private async Task<string> GetStatusMessage(long queueCommandID)
        {
            DataTable table = new DataTable();
            StringBuilder sb = new StringBuilder();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bGetSlavePCStatus");                
                MyCom.AddParam(queueCommandID);                
                table = MyCom.GetResult();

                if (table == null || table.Rows.Count == 0)
                {
                    return "No Information";
                }                
              
                foreach (DataRow row in table.Rows)
                {   
                        sb.Append(row["name"]);
                        sb.Append(" (");
                        sb.Append(row["type"]);
                        sb.Append(")");
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);

                    bool online = Convert.ToBoolean(row["online"]);
                    if (online)
                    {
                        sb.Append("Device is Online");
                    }
                    else
                    {
                        sb.Append("Device is Offline");
                    }
                    sb.Append(Environment.NewLine);
                    sb.Append("Last Power ON: ");
                    sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["on"])));
                    sb.Append(Environment.NewLine);
                    if (!online)
                    {
                        sb.Append("Last Power OFF: ");
                        sb.Append(string.Format("{0: hh:mm tt (d MMM)} ", Convert.ToDateTime(row["off"])));
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append("-----------------------------");
                    sb.Append(Environment.NewLine);
                    
                }
               
                sb.Append(Environment.NewLine);
                sb.Append("/menu");
            }
            catch (Exception ex)
            {
                SaveErrorLog(queueCommandID, "GetStartUpTimeToday(queueCommandID): " + ex.Message);
            }
            return await Complete(sb.ToString());
        }


        private bool IsDeviceOnline(long queueCommandID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            InitSqlPath();
            bool online = false;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bCheckDeviceOnline");
                MyCom.AddParam(queueCommandID);                

                dic = MyCom.GetResultD();
                if (dic != null && dic.Count != 0)                
                {                    
                    bool.TryParse(dic["online"].ToString(), out online);                    
                }              
            }
            catch (Exception ex)
            {
                SaveErrorLog(queueCommandID, "IsDeviceOnline(): " + ex.Message);
            }
            return online;
        }



        private void InitSqlPath()
        {
#if DEBUG
            connectionString = ConfigurationManager.ConnectionStrings["Local_alexandr_gorbunov_ConnectionString"].ConnectionString;
#endif
        }
    }
}