using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;
using System.Text.RegularExpressions;
using System.Data;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using lenapw.test.Helpers;
using Telegram.Bot.Types.InlineKeyboardButtons;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System.Configuration;

namespace lenapw.test.Controllers
{
    public partial class WebHookPwController : ApiController
    {

        private string WEB_HOOK_URL = ConfigurationManager.AppSettings["webhook_urlpw"];
        private static string token = ConfigurationManager.AppSettings["token_webhookpw"];
        protected static string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString; 
        private MLDBUtils.SQLCom MyCom;
        static class Bot
        {
            public static readonly TelegramBotClient Api = new TelegramBotClient(token);
        }

        private const int MAIN_MENU = 0;
        private const int CREATE_SLAVE_NAME = 1;
        private const int ENTER_CODE = 2;
        private const int SELECTED_SLAVE = 3;
        private const int RENAME_SLAVE = 4;
        private const int DELETE_SLAVE = 5;


        private string EMPTY_SQL = "-1";
        private string SQL_ERROR = "-3";

        #region VARs

        #endregion

        #region public methode
        public string Get()
        {
            return "telegram Web Hook - pw master";
        }

        public ICollection<Client> Get(int id)
        {
            if (id == 555)
            {
                Bot.Api.SetWebhookAsync(WEB_HOOK_URL).Wait();                
                return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "webhook set up succesful" }
                            }
                       };
            }
            //else
            // if (id == 333) //Need delete!!!!!
            //{

            //    SendMessage(305241655, "hi man");
            //    //SendPhoto(305241655);
            //    SendPhotoTest();
            //    return new Collection<Client>
            //           {
            //                {
            //                    new Client { Id = id, Title = "photo ok" }
            //                }
            //           };
            //}
            else
            {
                if (id == 777)
                {
                    AutoSendMessageFromBotAsync();
                    return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "AutoSendMessageFromBotAsync run succesful" }
                            }
                       };
                }
                else
                {                    
                       return new Collection<Client>
                       {
                            {
                                new Client { Id = id, Title = "wrong" }
                            }
                       };
                }
            }
        }

        private async void SendPhotoTest()
        {
            //InitSqlPath();
            //ScreenShot screenshot = await getScreenShotPC("10268B6E0C1A1F07F62104EF919F83BF", "9b2fc739-c695-4b26-a595-84f58fa2302f");
            //SendPhoto(305241655, screenshot.ImageScreen, string.Format("made: {0: hh:mm tt (d MMM)} ", screenshot.dateCreate), screenshot.synchronizeTime.ToShortTimeString());
        }

        private async Task<ScreenShot> getScreenShotPC(string hash, string GUID)
        {
            DataTable table = new DataTable();
            string result = string.Empty;
            ScreenShot res = null;

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
                    res = new ScreenShot
                    {
                        GUID = row["GUID"].ToString(),
                        ImageScreen = (byte[])row["ImageScreen"], //Utils.ObjectToByteArray(row["ImageScreen"]),//Encoding.UTF8.GetBytes(row["ImageScreen"].ToString()),//Utils.ObjectToByteArray(row["ImageScreen"]),
                        dateCreate = Convert.ToDateTime(row["dateCreate"]),
                        synchronizeTime = Convert.ToDateTime(row["synchronizeTime"])
                    };
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                res = new ScreenShot { GUID = "SQL error " + err };
                return await Utils.Complete(res);
            }

            return await Utils.Complete(res);
        }

        private void AutoSendMessagePhotoFromBotAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> Post(Update update)
        {
            if (update == null)
            {
                return null;
            }
            var msg = update.Message;
            if (update.Type == UpdateType.CallbackQueryUpdate)
            {
                PressedButtonFromInlineMenu(update);
                return Ok();
            }
            if (msg == null)
            {
                //--------------------------------------------------------------------------Inline mode
                if (update.Type == UpdateType.InlineQueryUpdate)
                {

                }
            }
            if (msg == null || msg.Text == null) return Ok();
            long chatid = msg.Chat.Id;
            string username = msg.Chat.Username ?? "-";
            string lastname = msg.Chat.LastName ?? "-";
            string firstname = msg.Chat.FirstName ?? "-";
            //Message recive
            if (msg.Type == MessageType.TextMessage)
            {
                switch (msg.Text)
                {
                    case "/start":
                        {
                            SendHiMessage(msg);
                            break;
                        }
                    case "/help":
                        {
                            HELP(chatid);
                            break;
                        }
                    case "/about":
                        {
                            ABOUT(chatid);
                            break;
                        }
                    case "/add":
                        {
                            SendMessage(chatid, "Alright, a new slave device. How are we going to call it? Please choose a NAME for your paired device. /main - return to main menu", true);
                            SetFullStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = CREATE_SLAVE_NAME, FirstName = firstname, Username = username, LastName = lastname });
                            break;
                        }
                    case "/main":
                        {
                            string[] buttons = { "/main", "/help" };
                            SendMessageAndButtons(chatid, "Main menu", buttons);
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                            MainMenuShow(chatid);
                            break;
                        }
                    case "/CANCEL":
                        {
                            string[] buttons = { "/add", "/main", "/help" };
                            SendMessageAndButtons(chatid, "The command has been cancelled.", buttons);
                            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = status.Data, Name = status.Name, StatusID = SELECTED_SLAVE });
                            DeviceMenuShow(chatid, status.Data, status.Name);
                            break;
                        }
                    case "/test":
                        {
                            SendMessage(msg, "RUN testing...");
                            break;
                        }
                    default:
                        {
                            GetAnswer(chatid, msg.Text);
                            break;
                        }
                }
            }
            return Ok();
        }

        private void ABOUT(long chatid)
        {
            string[] buttons = { "/main" };
            //TODO make it later - screenshot with pair new device
            SendMessageAndButtons(chatid, "ABOUT - make it later!!!!!!", buttons);
        }

        private void HELP(long chatid)
        {
            string[] buttons = { "/main" };
            //TODO make it later - screenshot with pair new device
            SendMessageAndButtons(chatid, "HELP - image step by step. make it later!!!!!!", buttons);
        }

        private async void PressedButtonFromInlineMenu(Update up)
        {
            long chatid = 0;
            try
            {
                string data = string.Empty;
                if (up.CallbackQuery.Data != null)
                {
                    data = up.CallbackQuery.Data;
                }
                //var id = up.Id;
                //var fromId = up.CallbackQuery.From.Id;
                //var CallBackQuerryID = up.CallbackQuery.Id;
                //var MessageID = up.CallbackQuery.Message.MessageId;
                //var MessageText = up.CallbackQuery.Message.Text;
                chatid = up.CallbackQuery.Message.Chat.Id;
                if (data.Contains("_")) //enter digit for code
                {
                    string digit = data.Replace("_", string.Empty);
                    if (IsArrayDigit(digit))
                    {
                        AddDigitToCode(chatid, digit);
                    }
                }
                if (data.Contains("dev|"))
                {
                    int ID = 1;
                    int NAME = 2;
                    string[] alldata = data.Split('|');
                    if (alldata.Length == 3)
                    {

                        SetStatus(new CurrentStatus { ChatID = chatid, Data = alldata[ID], Name = alldata[NAME], StatusID = SELECTED_SLAVE });
                        string[] buttons = { "/add", "/main", "/help" };
                        //  SendMessageAndButtons(chatid, string.Format("Here it is: {0}. What do you want to do with the slave device?", alldata[NAME]), buttons);
                        SendMessage(chatid, string.Format("Here it is: {0}. What do you want to do with the slave device?", alldata[NAME]));
                        await Task.Delay(300);
                        await DeviceMenuShow(chatid, alldata[ID], alldata[NAME]);
                    }
                }
                if (data.Contains("del|"))
                {
                    //int ID = 1;
                    //int NAME = 2;
                    //string[] alldata = data.Split('_');
                    //if (alldata.Length == 3)
                    //{
                    //    SetStatus(new CurrentStatus { ChatID = chatid, Data = alldata[ID], Name = alldata[NAME], StatusID = SELECTED_SLAVE });
                    //    SendMessage(chatid, alldata[NAME]);
                    //}
                }

                if (data.Contains("menu|"))
                {
                    MenuOperate(data, chatid);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "PressedButtonFromInlineMenu():" + ex.Message);
            }
        }


        #endregion


        #region Private methods

        public void SaveErrorLog(long chatID, string error)
        {
            InitSqlPath();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bErrorlog");
                MyCom.AddParam(chatID);
                MyCom.AddParam(error);
                MyCom.ExecuteCommand();
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
        }

        private async void GetAnswer(long chatid, string message)
        {
            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });

            switch (status.StatusID)
            {
                case MAIN_MENU:
                    {
                        MainMenuShow(chatid);
                        break;
                    }
                case CREATE_SLAVE_NAME:
                    {
                        SendMessage(chatid, string.Format("Pair the Device '{0}' and ENTER a PIN (6-digit) from your pair slave device - code valid only 2 minute", message));
                        SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = message.Truncate(45), StatusID = ENTER_CODE });
                        ShowDigitKeyboard(chatid, "Enter pairing code");
                        //ShowReplyDigitKeyboard(chatid, "Enter pairing code");
                        break;
                    }
                case ENTER_CODE:
                    {
                        if (string.IsNullOrEmpty(status.Data))
                        {
                            SendMessage(chatid, "Empty");
                        }
                        else //savede code not whole complete
                        {
                            if (message.Length == 6 && IsStringDigit(message))
                            {
                                if (PairDevice(message, new Master { ChatId = chatid, TypeDeviceID = (int)TypeDevicePW.TelegramBotMaster, Name = (string.Format("{0} {1} ({2})", status.FirstName, status.LastName, status.Username)).Truncate(45) }, status.Name))
                                {
                                    SendMessage(chatid, "Congratulation! Added a device success");
                                    SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = message.Truncate(45), StatusID = MAIN_MENU });
                                    MainMenuShow(chatid);
                                }
                            }
                            else //savede code not whole complete
                            {
                                string[] buttons = { "/main", "/help" };
                                SendMessageAndButtons(chatid, @"Bad code.
Enter a PIN(6 digit)", buttons);
                            }
                        }
                        break;
                    }

                case SELECTED_SLAVE:
                    {
                        await DeviceMenuShow(chatid, status.Data, status.Name);
                        break;
                    }
                case DELETE_SLAVE:
                    {
                        Delete(chatid, message);
                        break;
                    }
                case RENAME_SLAVE:
                    {
                        Rename(chatid, message);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void ShowDeviceStatus(long chatid)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int res = 0;
            string result = string.Empty;
            const int OK = 1;
            InitSqlPath();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bGetSlaveStatus");
                MyCom.AddParam(chatid);

                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    SendMessage(chatid, "status is undefined!");
                }
                else
                {
                    //TODO: make good status string for answer
                    string mess = string.Format("{0}{1}{2}{3}{4}{5}", dic["call"], dic["batt"], dic["lastGeoTime"], dic["last"], dic["geo"], dic["statusdevice"]);
                    SendMessage(chatid, mess);
                }

                CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                Task.Delay(300);
                DeviceMenuShow(chatid, status.Data, status.Name);
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "ShowDeviceStatus(): " + ex.Message);
            }
        }

        public static Task<T> Complete<T>(T result)
        {
            var r = new TaskCompletionSource<T>();
            r.SetResult(result);
            return r.Task;
        }


        private DataTable GetMenu(string deviceid)
        {
            InitSqlPath();
            DataTable dt = new DataTable();
            MyCom = new MLDBUtils.SQLCom(connectionString, "");
            MyCom.setCommand("bGETMenuDevice");
            MyCom.AddParam(deviceid);
            dt = MyCom.GetResult();
            return dt;

        }

        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = 60, VaryByParam = "deviceid")]
        private async Task<Message> DeviceMenuShow(long chatid, string deviceid, string deviceName)
        {
            Message mess = new Message();
            /*
            Stopwatch sw = Stopwatch.StartNew();
            var delay = Task.Delay(1000).ContinueWith(_ =>
            {
                sw.Stop();
                return sw.ElapsedMilliseconds;
            });
            delay.Result
                */
            ///!TODO: need save to cash and not get DB every time
            try
            {
                //var cacheKey = string.Format("DeviceMenuShow{0}_{1}_{2}", chatid, deviceid, deviceName);
                //this.HttpContext.Cache[cacheKey] = this.HttpContext.Cache[cacheKey] ?? Part.GetCount();
                //MyCom = new MLDBUtils.SQLCom(connectionString, "");
                //MyCom.setCommand("bGETMenuDevice");
                //MyCom.AddParam(deviceid);
                //DataTable dt = MyCom.GetResult();
                var dt = GetMenu(deviceid);
                if (dt != null && dt.Rows.Count != 0)
                {
                    var keyboard = GetInlineKeybord("menu", dt, 2);
                    //var keyboard = GetCustomKeybord(dt, 2);  //remove it!!!!!!!!                  
                    if (keyboard != null)
                    {
                        //TimeSpan span = DateTime.Now - start;
                        //int ms = (int)span.TotalMilliseconds;                        
                        mess = await Bot.Api.SendTextMessageAsync(chatid, string.Format("'{0}'", deviceName), replyMarkup: keyboard); //, {1}, DateTime.Now
                    }
                    else
                    {
                        mess = await Bot.Api.SendTextMessageAsync(chatid, "Error create Device Menu in GetInlineKeybord()");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "DeviceMenuShow(): " + ex.Message);
            }
            return mess;
        }

        private void AddDigitToCode(long chatid, string digit)
        {
            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
            if (status.Data == null)
            {
                return;
            }
            if (status.StatusID == ENTER_CODE)
            {
                if (status.Data.Equals("-"))
                {
                    SetStatus(new CurrentStatus { ChatID = chatid, Data = digit, Name = status.Name, StatusID = ENTER_CODE });
                }
                else
                {
                    if (status.Data.Length == 5)//FINISH PAIR
                    {
                        string code = status.Data + digit;
                        if (PairDevice(code, new Master { ChatId = chatid, TypeDeviceID = (int)TypeDevicePW.TelegramBotMaster, Name = (string.Format("{0} {1} ({2})", status.FirstName, status.LastName, status.Username)).Truncate(45) }, status.Name))
                        {
                            SendMessage(chatid, "Added a device success");
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = status.Name, StatusID = MAIN_MENU });
                            MainMenuShow(chatid);
                        }
                        else
                        {
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = status.Name, StatusID = ENTER_CODE });
                            ShowDigitKeyboard(chatid, "Enter pairing code");
                        }
                    }
                    else
                    {
                        if (status.Data.Length > 5)
                        {
                            SendMessage(chatid, @"Bad code
Enter a PIN(6 digit)");
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = status.Name, StatusID = ENTER_CODE });
                        }
                        else
                        {
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = status.Data + digit, Name = status.Name, StatusID = ENTER_CODE });
                        }
                    }
                }
            }
        }

        private CurrentStatus GetStatus(CurrentStatus status)
        {
            InitSqlPath();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bGetStatus");
                MyCom.AddParam(status.ChatID);
                dic = MyCom.GetResultD();

                if (dic == null || dic.Count == 0)
                {
                    return status;
                }
                status.StatusID = Convert.ToInt32(dic["s"]);
                status.Data = dic["d"].ToString();
                status.Name = dic["n"].ToString();
                status.Username = dic["u"].ToString();
                status.FirstName = dic["f"].ToString();
                status.LastName = dic["l"].ToString();
            }
            catch (Exception ex)
            {
                SaveErrorLog(status.ChatID, "GetStatus(): " + ex.Message);
            }
            return status;
        }

        private async void ShowDigitKeyboard(long chatId, string text)
        {
            //  text
            await Bot.Api.SendChatActionAsync(chatId, ChatAction.Typing);
            string[] buttons = { "/main", "/help" };
            SendMessageAndButtons(chatId, "******", buttons);

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                    new[] // first row
                    {
                        InlineKeyboardButton.WithCallbackData("1", "_1"),
                        InlineKeyboardButton.WithCallbackData("2", "_2"),
                        InlineKeyboardButton.WithCallbackData("3", "_3"),
                        InlineKeyboardButton.WithCallbackData("4", "_4"),
                        InlineKeyboardButton.WithCallbackData("5", "_5"),
                    },
                    new[] // second row
                    {
                        InlineKeyboardButton.WithCallbackData("6", "_6"),
                        InlineKeyboardButton.WithCallbackData("7", "_7"),
                        InlineKeyboardButton.WithCallbackData("8", "_8"),
                        InlineKeyboardButton.WithCallbackData("9", "_9"),
                        InlineKeyboardButton.WithCallbackData("0", "_0")

                    }
                });

            //  var keyboard = new KeyboardButton("myButton");

            //  await Task.Delay(300); // simulate longer running task

            await Bot.Api.SendTextMessageAsync(chatId, text,
                replyMarkup: keyboard);
        }

        private void Delete(long chatid, string message)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int res = 0;
            string result = string.Empty;
            const int OK = 1;
            const int WRONG_NAME = 3;
            InitSqlPath();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bDelete");
                MyCom.AddParam(chatid);
                MyCom.AddParam(message);

                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    SendMessage(chatid, "error - return null or empty");
                }
                int.TryParse(dic["d"].ToString(), out res);
                SendMessage(chatid, dic["m"].ToString());
                switch (res)
                {
                    case WRONG_NAME:
                        {
                            Task.Delay(500);
                            SendMessage(chatid, "Enter Device NAME for DELETE or /CANCEL");
                            break;
                        }
                    case OK:
                        {
                            Task.Delay(500);
                            MainMenuShow(chatid);
                            break;
                        }
                    default:
                        {
                            SendMessage(chatid, "The command has been cancelled.", true);
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                            Task.Delay(500);
                            MainMenuShow(chatid);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "Delete(): " + ex.Message);
                SendMessage(chatid, ex.Message);
                SendMessage(chatid, "The command has been cancelled.", true);
                SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                MainMenuShow(chatid);
            }
        }

        private void Rename(long chatid, string message)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int res = 0;
            string result = string.Empty;
            const int OK = 1;
            InitSqlPath();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bRename");
                MyCom.AddParam(chatid);
                MyCom.AddParam(message);

                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    SendMessage(chatid, "error - return null or empty");
                }
                int.TryParse(dic["d"].ToString(), out res);
                SendMessage(chatid, dic["m"].ToString());
                switch (res)
                {
                    case OK:
                        {
                            CurrentStatus status = GetStatus(new CurrentStatus { ChatID = chatid, Name = string.Empty, Data = string.Empty, StatusID = 0 });
                            DeviceMenuShow(chatid, status.Data, status.Name);
                            break;
                        }
                    default:
                        {
                            SendMessage(chatid, "The command has been cancelled.", true);
                            SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                            Task.Delay(300);
                            MainMenuShow(chatid);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "Rename()" + ex.Message);
                SendMessage(chatid, ex.Message);
                SendMessage(chatid, "The command has been cancelled.", true);
                SetStatus(new CurrentStatus { ChatID = chatid, Data = "-", Name = "-", StatusID = MAIN_MENU });
                MainMenuShow(chatid);
            }
        }

        private async void SendMessage(Message incomingMessage, string messageForSend)
        {
            try
            {
                if (incomingMessage.From.FirstName != null)
                {
                    await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, string.Format("{0}, {1}", messageForSend, incomingMessage.From.FirstName));
                }
                else
                {
                    await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, messageForSend);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(incomingMessage.Chat.Id, "SendMessage(): " + ex.Message);
            }
        }

        private async void SendHiMessage(Message incomingMessage)
        {
            string[] buttons = { "/add", "/main", "/help", "/about" };
            var keyboard = GetCustomKeybord(buttons, 2);
            string msg = "Welcome to @lenaPWbot - Personal Watcher Master";
            string msg2 = @"/add - Add new slave device
/main - Main menu
/help - Manual for use by the PW bot
/about - About PW bot";
            try
            {
                if (incomingMessage.From.FirstName != null)
                {
                    await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, string.Format(@"{0}, {1} 
{2}", msg, incomingMessage.From.FirstName, msg2), replyMarkup: keyboard);
                }
                else
                {
                    await Bot.Api.SendTextMessageAsync(incomingMessage.Chat.Id, string.Format(@"{0} 
{1}", msg, msg2), replyMarkup: keyboard);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(incomingMessage.Chat.Id, "SendMessage(): " + ex.Message);
            }
        }

        private async void SendMessageAndButtons(long chatID, string msg, string[] mainmenu)
        {
            var keyboard = GetCustomKeybord(mainmenu, 2);
            try
            {
                await Bot.Api.SendTextMessageAsync(chatID, msg,
                            replyMarkup: keyboard);
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatID, "SendMessage(): " + ex.Message);
            }
        }

        #region Auto Send message to Master Bot from Table bMessForSending -------------------------------------------------------------------------------
        private async void AutoSendMessageFromBotAsync()
        {
            var nextmessage = GetNextMessage(new NextMessage { MessForSendingID = 0, Error = 0 });
            if (nextmessage.Error != 0 || nextmessage.Work || nextmessage.Empty || nextmessage.ChatID == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(nextmessage.Message) && nextmessage.MessForSendingID == 0)
            {
                return;
            }
            //empty message set Error!
            if (string.IsNullOrEmpty(nextmessage.Message) && nextmessage.MessForSendingID != 0)
            {
                nextmessage = GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 1 });
            }

            try
            {
                var message = await Bot.Api.SendTextMessageAsync(nextmessage.ChatID, nextmessage.Message);               
                for (int i = 0; i < 30; i++)
                {
                    if (message != null)
                    {
                        if (nextmessage.HaveImage && nextmessage.ScreenShot != null)
                        {
                            await SendPhoto(nextmessage.ChatID, nextmessage.ScreenShot, string.Format("made: {0: hh:mm tt (d MMM)} ", nextmessage.DateCreate), nextmessage.MessForSendingID.ToString());
                            //, screenshot.synchronizeTime.ToShortTimeString()
                            //SendPhoto(long chatid, byte[] buffer, string capture, string time)
                        }
                        await SendNextMessageFromBotAsync(GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 0 }));
                        return;
                    }
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(nextmessage.ChatID, "AutoSendMessageFromBotAsync(): " + ex.Message);
            }
            await SendNextMessageFromBotAsync(GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 1 }));
        }
        private async Task SendNextMessageFromBotAsync(NextMessage nextmessage)
        {
            if (nextmessage.Error != 0 || nextmessage.Work || nextmessage.Empty || nextmessage.ChatID == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(nextmessage.Message) && nextmessage.MessForSendingID == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(nextmessage.Message) && nextmessage.MessForSendingID != 0)
            {
                nextmessage = GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 1 });
            }

            try
            {
                var message = await Bot.Api.SendTextMessageAsync(nextmessage.ChatID, nextmessage.Message); //, replyMarkup: new ReplyKeyboardHide()     ---------hide reply keyboard not use!                
                for (int i = 0; i < 30; i++)
                {
                    //wait why message is sending
                    if (message != null)
                    {
                        if (nextmessage.HaveImage && nextmessage.ScreenShot != null)
                        {
                            await SendPhoto(nextmessage.ChatID, nextmessage.ScreenShot, string.Format("made: {0: hh:mm tt (d MMM)} ", nextmessage.DateCreate), nextmessage.MessForSendingID.ToString());
                        }
                        await SendNextMessageFromBotAsync(GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 0 }));
                        return;
                    }
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(nextmessage.ChatID, "SendNextMessageFromBotAsync(NextMessage nextmessage): " + ex.Message);
            }
            await SendNextMessageFromBotAsync(GetNextMessage(new NextMessage { MessForSendingID = nextmessage.MessForSendingID, Error = 1 }));
        }

        private NextMessage GetNextMessage(NextMessage nextMessage)
        {
            InitSqlPath();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bSendNextMessage");
                MyCom.AddParam(nextMessage.MessForSendingID);
                MyCom.AddParam(nextMessage.Error);
                dic = MyCom.GetResultD();
                if (dic == null || dic.Count == 0)
                {
                    nextMessage.Error = 7;
                    return nextMessage;
                }

                nextMessage.Error = 0;
                nextMessage.Work = dic["work"].ToString().Equals("1") ? true : false;
                nextMessage.Empty = dic["empty"].ToString().Equals("1") ? true : false;
                nextMessage.MessForSendingID = Convert.ToInt64(dic["messForSendingID"].ToString());
                nextMessage.ChatID = Convert.ToInt64(dic["chatID"].ToString());
                nextMessage.Message = dic["message"].ToString();
                nextMessage.HaveImage = dic["imagescreen"].ToString().Equals("1") ? true : false;
                nextMessage.DateCreate = Convert.ToDateTime(dic["DateCreate"]);
                if (!nextMessage.Work && !nextMessage.Empty && nextMessage.HaveImage && dic["image"] != null)
                {
                    nextMessage.ScreenShot = (byte[])(dic["image"]);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(0, "GetNextMessage(): " + ex.Message);
                nextMessage.Error = 5;
            }
            return nextMessage;
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        #endregion------------------------------------------------------------------------------

        private async void SendMessage(long messId, string messageForSend, bool hideKeyboard = false)
        {
            try
            {
                if (hideKeyboard)
                {
                    await Bot.Api.SendTextMessageAsync(messId, messageForSend);
                }
                else
                {
                    await Bot.Api.SendTextMessageAsync(messId, messageForSend);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(messId, "SendMessage()(): " + ex.Message);
            }
        }
        private void SetStatus(CurrentStatus status)
        {
            // Dictionary<string, object> dic = new Dictionary<string, object>();
            InitSqlPath();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bSetStatusNow");
                MyCom.AddParam(status.ChatID);
                MyCom.AddParam(status.StatusID);
                MyCom.AddParam(status.Data);
                MyCom.AddParam(status.Name.Truncate(45));
                MyCom.ExecuteCommand();
            }
            catch (Exception ex)
            {
                SaveErrorLog(status.ChatID, "SetStatus(): " + ex.Message);
            }
        }

        private void SetFullStatus(CurrentStatus status)
        {
            // Dictionary<string, object> dic = new Dictionary<string, object>();
            InitSqlPath();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bSetFullStatusNow");
                MyCom.AddParam(status.ChatID);
                MyCom.AddParam(status.StatusID);
                MyCom.AddParam(status.Data);
                MyCom.AddParam(status.Name.Truncate(45));

                MyCom.AddParam(status.Username.Truncate(45));
                MyCom.AddParam(status.FirstName.Truncate(45));
                MyCom.AddParam(status.LastName.Truncate(45));

                MyCom.ExecuteCommand();
            }
            catch (Exception ex)
            {
                SaveErrorLog(status.ChatID, "SetStatus(): " + ex.Message);
            }
        }

        private async void MainMenuShow(long chatid)
        {
            InitSqlPath();
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");
                MyCom.setCommand("bGETDevices");
                MyCom.AddParam(chatid);
                DataTable dt = MyCom.GetResult();
                if (dt != null && dt.Rows.Count != 0)
                {
                    var keyboard = GetInlineKeybord("dev", dt, 3);
                    await Bot.Api.SendTextMessageAsync(chatid, "Choice your slave device:",
                        replyMarkup: keyboard);
                }
                else
                {
                    string[] mainmenu = { "/add", "/help" };
                    var keyboard = GetCustomKeybord(mainmenu, 2);
                    string msg = "You have no paired devices, please /add new device or read /help for more details";
                    await Bot.Api.SendTextMessageAsync(chatid, msg,
                       replyMarkup: keyboard);
                }
            }
            catch (Exception ex)
            {
                SaveErrorLog(chatid, "MainMenuShow(): " + ex.Message);
            }
        }

        private InlineKeyboardMarkup GetInlineKeybord(string type, DataTable dt, int column)
        {
            int ID = 0;
            int NAME = 1;

            int all = dt.Rows.Count;
            int rows = 0;
            if (all % column == 0)
            {
                rows = all / column;
            }
            else
            {
                rows = (all / column) + 1;
            }

            int item = 0;

            InlineKeyboardButton[][] menuButtons = new InlineKeyboardButton[rows][];

            for (int i = 0; i < rows; i++)
            {
                if ((all - item) < column) //not all item for last row
                {
                    menuButtons[i] = new InlineKeyboardButton[all - item];
                }
                else
                {
                    menuButtons[i] = new InlineKeyboardButton[column];
                }
                for (int j = 0; j < column; j++)
                {
                    if (item < all)
                    {
                        menuButtons[i][j] = InlineKeyboardButton.WithCallbackData(dt.Rows[item].ItemArray[NAME].ToString(), string.Format("{0}|{1}|{2}", type, dt.Rows[item].ItemArray[ID], dt.Rows[item].ItemArray[NAME]));//new InlineKeyboardButton();
                    }
                    item++;
                }
                if (item >= all)
                {
                    break;
                }
            }

            return new InlineKeyboardMarkup(menuButtons);
        }

        private ReplyKeyboardMarkup GetCustomKeybord(DataTable dt, int column)
        {
            int NAME = 1;

            int all = dt.Rows.Count;
            int rows = 0;
            if (all % column == 0)
            {
                rows = all / column;
            }
            else
            {
                rows = (all / column) + 1;
            }

            int item = 0;

            KeyboardButton[][] menuButtons = new KeyboardButton[rows][];

            for (int i = 0; i < rows; i++)
            {
                if ((all - item) < column) //not all item for last row
                {
                    menuButtons[i] = new KeyboardButton[all - item];
                }
                else
                {
                    menuButtons[i] = new KeyboardButton[column];
                }
                for (int j = 0; j < column; j++)
                {
                    if (item < all)
                    {
                        menuButtons[i][j] = new KeyboardButton(dt.Rows[item].ItemArray[NAME].ToString());
                    }
                    else
                    {

                    }
                    item++;
                }
                if (item >= all)
                {
                    break;
                }
            }

            return new ReplyKeyboardMarkup(menuButtons);
        }


        private ReplyKeyboardMarkup GetCustomKeybord(string[] keys, int column)
        {
            int all = keys.Length;
            int rows = 0;
            if (all % column == 0)
            {
                rows = all / column;
            }
            else
            {
                rows = (all / column) + 1;
            }

            int item = 0;

            KeyboardButton[][] menuButtons = new KeyboardButton[rows][];

            for (int i = 0; i < rows; i++)
            {
                if ((all - item) < column) //not all item for last row
                {
                    menuButtons[i] = new KeyboardButton[all - item];
                }
                else
                {
                    menuButtons[i] = new KeyboardButton[column];
                }
                for (int j = 0; j < column; j++)
                {
                    if (item < all)
                    {
                        menuButtons[i][j] = new KeyboardButton(keys[item]);
                    }
                    else
                    {

                    }
                    item++;
                }
                if (item >= all)
                {
                    break;
                }
            }

            return new ReplyKeyboardMarkup(menuButtons);
        }

        private bool PairDevice(string mess, Master master, string slavename)
        {
            int codeA = 0;
            int.TryParse(mess, out codeA);
            string scode = GetCodeB(codeA, master, slavename);
            int code = 0;
            int.TryParse(scode, out code);
            if (code < 100000)
            {
                //-11 not valid code A
                //-12 code A is Expired 2min
                //-22 for code A pair EXIST before!!!
                //-4 unexpected error		
                //-3 exception sql
                //-1 data return empty
                switch (code)
                {
                    case -11:
                        {
                            SendMessage(master.ChatId, @"Invalid code A
Enter a PIN(6 digit)");
                            break;
                        }
                    case -12:
                        {
                            SendMessage(master.ChatId, @"code A is Expiry
Enter a new PIN(6 digit)");
                            break;
                        }
                    case -22:
                        {
                            SendMessage(master.ChatId, "Device has been paired before");
                            SetStatus(new CurrentStatus { ChatID = master.ChatId, Data = "-", Name = "-", StatusID = MAIN_MENU });
                            break;
                        }
                    case -4:
                        {
                            SendMessage(master.ChatId, "Unexpected error");
                            break;
                        }
                    case -3:
                        {
                            SendMessage(master.ChatId, "SQL error");
                            break;
                        }
                    case -1:
                        {
                            SendMessage(master.ChatId, "Empty data");
                            break;
                        }
                    default:
                        {
                            SendMessage(master.ChatId, "ERROR! code = " + code);
                            break;
                        }
                }
                return false;
            }
            SendMessage(master.ChatId, "Session ID " + code);
            return true;
        }

        private string GetCodeB(int codeA, Master master, string slavename)
        {
            //-11 not valid code A
            //-12 code A is Expired 2min
            //-22 for code A pair EXIST before!!!
            //-4 unexpected error		
            //-3 exception sql
            //-1 data return empty
            InitSqlPath();
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
                MyCom.AddParam(slavename);
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
                SaveErrorLog(master.ChatId, "GetCodeB(): " + ex.Message);
                result = SQL_ERROR;
            }
            return result;
        }

        private bool IsStringDigit(string mess)
        {

            bool result = true;
            if (mess == null || mess.ToCharArray().Length == 0)
            {
                return false;
            }

            Match m;
            foreach (var vol in mess.ToCharArray())
            {
                m = Regex.Match(vol.ToString(), @"[0-9]+(\.[0-9]+)?");
                if (string.IsNullOrEmpty(m.Value))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }


        private static bool IsArrayDigit(string[] array)
        {
            bool result = true;
            if (array == null || array.Length == 0)
            {
                return false;
            }

            Match m;
            foreach (var vol in array)
            {
                m = Regex.Match(vol, @"[0-9]+(\.[0-9]+)?");
                if (string.IsNullOrEmpty(m.Value))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private static bool IsArrayDigit(string array)
        {
            bool result = true;
            if (array == null || array.Length == 0)
            {
                return false;
            }

            Match m;
            for (int i = 0; i < array.Length; i++)
            {
                m = Regex.Match(array[i].ToString(), @"[0-9]+(\.[0-9]+)?");
                if (string.IsNullOrEmpty(m.Value))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        #endregion

        #region not used metod for future
        private async void ShowReplyDigitKeyboard(long chatid, string text)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new [] // first row
                     {
                        new KeyboardButton("1"),
                        new KeyboardButton("2"),
                        new KeyboardButton("3"),
                        new KeyboardButton("4"),
                        new KeyboardButton("5"),
                    },
                    new[] // second row
                    {
                        new KeyboardButton("6"),
                        new KeyboardButton("7"),
                        new KeyboardButton("8"),
                        new KeyboardButton("9"),
                        new KeyboardButton("0")

                    }
                });

            await Bot.Api.SendTextMessageAsync(chatid, text,
            replyMarkup: keyboard);
        }

        private async void SendPhoto(long chatid)
        {
            try
            {
                await Bot.Api.SendChatActionAsync(chatid, ChatAction.UploadPhoto);
                //string targetFolder = HttpContext.Current.Server.MapPath("~/uploads");
                string file = @"d:\DEV\pwlenasite\lenapw.test\uploads\hash.jpg";//Path.Combine(targetFolder, "hash.jpg");

                var fileName = file.Split('\\').Last();
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var fts = new FileToSend(fileName, fileStream);

                    await Bot.Api.SendPhotoAsync(chatid, fts, "Screenshot");
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }          
        }

        private async Task<int> SendPhoto(long chatid, byte[] buffer, string capture, string time)
        {
            int result = 0;
            try
            {
                await Bot.Api.SendChatActionAsync(chatid, ChatAction.UploadPhoto);               
                using (var memoryStream = new MemoryStream(buffer))
                {
                    var fts = new FileToSend(time, memoryStream);

                    await Bot.Api.SendPhotoAsync(chatid, fts, capture);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                result = 0;
            }
            return await Utils.Complete(result);
        }
        //private async void SendPhoto(long chatid)
        //{
        //    await Bot.Api.SendChatActionAsync(chatid, ChatAction.UploadPhoto);

        //    const string file = @"<FilePath>";

        //    var fileName = file.Split('\\').Last();

        //    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        var fts = new FileToSend(fileName, fileStream);

        //        await Bot.Api.SendPhotoAsync(chatid, fts, "Nice Picture");
        //    }
        //}

        //private async Task SendPhoto(long chatid, byte[] data, string capturetext)
        //{            
        //    try
        //    {
        //        await Bot.Api.SendChatActionAsync(chatid, ChatAction.UploadPhoto);

        //        using (Stream stream = new MemoryStream(data))
        //        {
        //            var fts = new FileToSend("screenshot", stream);

        //            await Bot.Api.SendPhotoAsync(chatid, fts, capturetext);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //    }
        //}

        private async void RequestLocationOrContact(long chatid)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
              {
                    new KeyboardButton("Location")
                    {
                        RequestLocation = true
                    },
                    new KeyboardButton("Contact")
                    {
                        RequestContact = true
                    },
                });

            await Bot.Api.SendTextMessageAsync(chatid, "Who or Where are you?", replyMarkup: keyboard);
        }

        #endregion
    }

    //public class CacheFilterAttribute : ActionFilterAttribute
    //{
    //    public int Duration
    //    {
    //        get;
    //        set;
    //    }

    //    public CacheFilterAttribute()
    //    {
    //        Duration = 10;
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        if (Duration <= 0) return;

    //        HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
    //        TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

    //        cache.SetCacheability(HttpCacheability.Public);
    //        cache.SetExpires(DateTime.Now.Add(cacheDuration));
    //        cache.SetMaxAge(cacheDuration);
    //        cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
    //    }
    //}

    #region external class
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

 
    public class NextMessage
    {
        public long MessForSendingID { get; set; }
        public long ChatID { get; set; }
        public string Message { get; set; }
        public bool Work { get; set; }
        public bool Empty { get; set; }
        public int Error { get; set; }

        public DateTime DateCreate { get; set; }

        public byte[] ScreenShot { get; set; }

        public bool HaveImage { get; set; }

    }

    #endregion
}