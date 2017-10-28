using Microsoft.Win32;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace pw.lena.slave.winpc
{
    public partial class MainForm : Form
    {
        bool invisibleApp = false;

        #region var
        System.Windows.Forms.Timer codeaTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer powertimeTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer synchroTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer commandTimer = new System.Windows.Forms.Timer();
        int i = 2 * 60;
        int j = 5 * 60;
        int k = 20 * 60;
        int c = 10;
        mServices services = null;
        int remain = 5;
        string GUID = string.Empty;
        bool loading = true;
        #endregion

        public MainForm()
        {
            load();
        }
        public MainForm(bool _minimize)
        {
            if (_minimize)
            {
                invisibleApp = true;
            }
            load();
        }

        private async void load()
        {
            InitializeComponent();
            GUID = Guid.NewGuid().ToString();
            dateTimePicker_from.Value = dateTimePicker_to.Value = DateTime.Now.Date;
            await GetListMastersFromLocalSaved();
            loading = false;
            startPowertimeTimer();
            startSynchroTimer();
            startCommandTimer();
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SetAutoRun();
        }

        private void startCommandTimer()
        {            
            if (!commandTimer.Enabled)
            {
                c = 10;
                commandTimer.Interval = 1000; // specify interval time as you want
                commandTimer.Tick += new EventHandler(timerCommand_Tick);
                commandTimer.Start();
            }
        }       

        private void SetAutoRun()
        {
#if !DEBUG
            if (!utils.GetAuturunValue())
                {
                    if (!utils.SetAutorunValue(true))
                        MessageBox.Show("error autorun set!");
                }
#endif
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend) PauseTimer();
            else if (e.Mode == PowerModes.Resume) ResumeTimer();
        }

        private void ResumeTimer()
        {
            GUID = Guid.NewGuid().ToString();
            startPowertimeTimer();
            startSynchroTimer();
            startCommandTimer();
        }

        private void PauseTimer()
        {
            if (codeaTimer.Enabled)
            {
                codeaTimer.Stop();
                codeaTimer.Tick -= new EventHandler(timer_codeA_Tick);
                label_count.Text = "Closing";
                Thread.Sleep(1000);
            }
            if (powertimeTimer.Enabled)
            {
                powertimeTimer.Stop();
                powertimeTimer.Tick -= new EventHandler(timerPowerTime_Tick);
                label_powertimetimer.Text = "Closing";
                Thread.Sleep(1000);
            }
            if (synchroTimer.Enabled)
            {
                synchroTimer.Stop();
                synchroTimer.Tick -= new EventHandler(timerSynchro_Tick);
                label_synch_result.Text = "Closing";
                Thread.Sleep(1000);
            }
            if (commandTimer.Enabled)
            {
                commandTimer.Stop();
                commandTimer.Tick -= new EventHandler(timerCommand_Tick);
                label_command_result.Text = "Closing";
                Thread.Sleep(1000);
            }
        }

        private void startSynchroTimer()
        {
            if (!synchroTimer.Enabled)
            {
                k = 20 * 60;
                synchroTimer.Interval = 1000; // specify interval time as you want
                synchroTimer.Tick += new EventHandler(timerSynchro_Tick);
                synchroTimer.Start();
            }
        }

        private async void startPowertimeTimer()
        {
            if (!powertimeTimer.Enabled)
            {
                j = 5 * 60;
                powertimeTimer.Interval = 1000; // specify interval time as you want
                powertimeTimer.Tick += new EventHandler(timerPowerTime_Tick);
                powertimeTimer.Start();
            }
        }

        private async void SaveTimeToSql()
        {
            if (!string.IsNullOrEmpty(GUID))
            {
                if (services == null)
                {
                    services = new mServices();
                }
                int result = await services.UpdateOrCreatePowerPCtoSql(GUID);
                switch (result)
                {
                    case 0:
                        {
                            label_savepowertime_result.Text = "not saved";
                            break;
                        }
                    case 1:
                        {
                            label_savepowertime_result.Text = DateTime.Now.ToLongTimeString();
                            await services.GetListPowerTimeTODAYFromLocalSql(listBox_powertime);
                            break;
                        }
                    case -1:
                        {
                            label_savepowertime_result.Text = "error!";
                            break;
                        }
                    default:
                        {
                            label_savepowertime_result.Text = result.ToString();
                            break;
                        }
                }

            }
        }

        private void button_codeA_Click(object sender, EventArgs e)
        {
            StartCodeATimer();
        }

        private async void StartCodeATimer()
        {
            if (!codeaTimer.Enabled)
            {
                i = 120;
                codeaTimer.Interval = 1000; // specify interval time as you want
                codeaTimer.Tick += new EventHandler(timer_codeA_Tick);
                codeaTimer.Start();
                button_codeA.Text = "stop";
                GetCodeA();
            }
            else
            {
                codeaTimer.Stop();
                codeaTimer.Tick -= new EventHandler(timer_codeA_Tick);
                label_count.Text = string.Empty;
                button_codeA.Text = "get code A";
                await GetListMasters();
                remain = 5;
            }
        }

        void timer_codeA_Tick(object sender, EventArgs e)
        {
            i--;
            label_count.Text = string.Format("code 'A', expired time is {0}", utils.GetMinSecFromInt(i));
            if (i == 0)
            {
                GetListMasters();
                textBox_codeA.Text = "UP";
                remain--;
                if (remain > 0)
                {
                    Restart();
                }
                else
                {
                    codeaTimer.Stop();
                    codeaTimer.Tick -= new EventHandler(timer_codeA_Tick);
                    textBox_codeA.Text = "UP";
                    label_count.Text = "time is up";
                    button_codeA.Text = "get code A";
                    remain = 5;
                }
            }
        }

        void timerPowerTime_Tick(object sender, EventArgs e)
        {
            if (j == (5 * 60) - 5)
            {
                SaveTimeToSql();
            }
            j--;
            label_powertimetimer.Text = string.Format("remain {0}", utils.GetMinSecFromInt(j));
            if (j == 0)
            {
                label_powertimetimer.Text = string.Empty;
                j = 5 * 60;
            }
        }

        void timerSynchro_Tick(object sender, EventArgs e)
        {
            if (k == (20 * 60) - 10)
            {
                Synchro(true);
            }
            k--;
            label_synch.Text = string.Format("remain {0}", utils.GetMinSecFromInt(k));
            if (k == 0)
            {
                label_synch.Text = string.Empty;
                k = 20 * 60;
            }
        }

        void timerCommand_Tick(object sender, EventArgs e)
        {
            if (c == 5)
            {
                CheckNewCommand();
            }
            c--;
            label_command.Text = string.Format("remain {0}", utils.GetMinSecFromInt(c));
            if (c == 0)
            {
                label_command.Text = string.Empty;
                c = 10;
            }
        }

        

        private async void GetCodeA()
        {
            if (services == null)
            {
                services = new mServices();
            }

            try
            {
                string hash = utils.Truncate(utils.GetHashString(utils.GetMacAdres() + Environment.UserName + Environment.MachineName), 49);
                await services.GetCodeA(new DeviceModel
                {
                    AndroidIDmacHash = hash,
                    TypeDeviceID = (int)TypeDevicePW.WindowsPCslave,
                    codeA = 0,
                    codeB = 0,
                    Name = string.Format("{0}({1})", Environment.UserName, Environment.MachineName),
                    Token = "token"
                }, textBox_codeA);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Restart()
        {
            i = 120;
            GetCodeA();
        }

        #region only for test
        //test2
        private async void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(utils.Truncate(
            //    utils.GetHashString(utils.GetMacAdres() + Environment.UserName + Environment.MachineName), 49));
            //MessageBox.Show(ConverterHelper.ConvertDateWithoutTimeToMillisec(DateTime.Now).ToString());
            // SaveTimeToSql();
            if (services == null)
            {
                services = new mServices();
            }
            await services.testttttt(dateTimePicker_from.Value);
        }

        //test1
        private async void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(string.Format("{0: hh:mm tt (d MMM)}", (DateTime.Now)));
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            string name = p.ProcessName;
            MessageBox.Show(name);
        }

        #endregion only for test

        private async void button_ListMasters_Click(object sender, EventArgs e)
        {
            await GetListMasters();
        }

        private async Task GetListMasters()
        {
            if (services == null)
            {
                services = new mServices();
            }

            try
            {
                string hash = utils.Truncate(utils.GetHashString(utils.GetMacAdres() + Environment.UserName + Environment.MachineName), 49);
                await services.GetListMasters(new DeviceModel
                {
                    AndroidIDmacHash = hash,
                    TypeDeviceID = (int)TypeDevicePW.WindowsPCslave,
                    codeA = 0,
                    codeB = 0,
                    Name = string.Format("{0}({1})", Environment.UserName, Environment.MachineName),
                    Token = "token"
                }, listBox_masters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task GetListMastersFromLocalSaved()
        {
            if (services == null)
            {
                services = new mServices();
            }
            await services.GetListMastersLocalSql(listBox_masters);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PauseTimer();
        }

        private void button_refreshPT_Click(object sender, EventArgs e)
        {
            RefreshPowerTime();
        }

        private async Task RefreshPowerTime()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                MessageBox.Show("GUID is EMTPY!!!");
                return;
            }
            if (services == null)
            {
                services = new mServices();
            }
            await services.SaveAndGetListPowerTimeFromLocalSql(listBox_powertime, dateTimePicker_from.Value, dateTimePicker_to.Value, GUID);
            label_savepowertime_result.Text = DateTime.Now.ToLongTimeString();
        }

        private void dateTimePicker_to_ValueChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }
            ShowPowertimeList();
        }

        private void dateTimePicker_from_ValueChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }
            ShowPowertimeList();
        }

        private async Task ShowPowertimeList()
        {
            await services.GetListPowerTimeFromLocalSql(listBox_powertime, dateTimePicker_from.Value, dateTimePicker_to.Value);

        }

        private async void button_synch_powertime_Click(object sender, EventArgs e)
        {
            await Synchro(false);
        }

        private async Task CheckNewCommand()
        {           
            if (services == null)
            {
                services = new mServices();
            }
            int i = 0;
            string hash = utils.Truncate(utils.GetHashString(utils.GetMacAdres() + Environment.UserName + Environment.MachineName), 49);
            var device = new DeviceModel
            {
                AndroidIDmacHash = hash,
                TypeDeviceID = (int)TypeDevicePW.WindowsPCslave,
                codeA = 0,
                codeB = 0,
                Name = string.Format("{0}({1})", Environment.UserName, Environment.MachineName),
                Token = "token"
            };
            var commands = await services.SyncCommandWithRest(device);
            label_command_result.Text = DateTime.Now.ToLongTimeString();
            if (commands != null && commands.Count > 0)
            {
                await CommandOperate(commands);                
            }
        }       

        private async Task Synchro(bool silentmode)
        {
            if (services == null)
            {
                services = new mServices();
            }
            int i = 0;
            string hash = utils.Truncate(utils.GetHashString(utils.GetMacAdres() + Environment.UserName + Environment.MachineName), 49);
            var device = new DeviceModel
            {
                AndroidIDmacHash = hash,
                TypeDeviceID = (int)TypeDevicePW.WindowsPCslave,
                codeA = 0,
                codeB = 0,
                Name = string.Format("{0}({1})", Environment.UserName, Environment.MachineName),
                Token = "token"
            };
            i = await services.SyncSqlWithRest(device, DateTime.Now);
            if (!silentmode)
            {
                if (i >= 0)
                {
                    MessageBox.Show(string.Format("synchronized {0} records", i));
                    label_synch_result.Text = DateTime.Now.ToLongTimeString();
                }
                else
                {
                    switch (i)
                    {
                        case -1:
                            {
                                MessageBox.Show("error inside PowerPcService");
                                break;
                            }
                        case -2:
                            {
                                MessageBox.Show("Device not found in Rest");
                                break;
                            }
                        case -3:
                            {
                                MessageBox.Show("SQL error in Rest");
                                break;
                            }
                        case -777:
                            {
                                MessageBox.Show("not active device - no paired masters?");
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("not expected error");
                                break;
                            }
                    }
                }
            }
            else
            {
                if (i > 0)
                {
                    label_synch_result.Text = DateTime.Now.ToLongTimeString();
                }
                if (i < 0)
                {
                    label_synch_result.Text = "error";
                }
                if (i == 0)
                {
                    label_synch_result.Text = "no data";
                }
            }
            await ShowPowertimeList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (invisibleApp)
            {
                utils.HideFromAltTab(this.Handle);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (invisibleApp)
            {
                Visible = false;
            }
        }

        private void button_hide_Click(object sender, EventArgs e)
        {
            utils.HideFromAltTab(this.Handle);
            Visible = false;
        }

        private async void button_screenshot_Click(object sender, EventArgs e)
        {
            await SaveScreenShot();
        }

        private async Task SaveScreenShot(long queueCommandID = 0)
        {          
            if (services == null)
            {
                services = new mServices();
            }
            await services.SaveAndGetListScreenShotFromLocalSql(listBox_screenshots, dateTimePicker_from.Value, dateTimePicker_to.Value, queueCommandID);

        }


        private async Task CommandOperate(List<Command> commands)
        {
            foreach (var command in commands)
            {
                switch (command.commandID)
                {
                    case (int)Commands.GetScreenshot :
                    {
                            await SaveScreenShot(command.QueueCommandID);
                            await Synchro(true);                            
                            break;
                    }
                    case (int)Commands.StartupPCtime:
                        {
                            //await SaveScreenShot(command.QueueCommandID);
                            await RefreshPowerTime();
                            await Synchro(true);
                            break;
                        }

                    default:
                        {
                            label_command_result.Text = "unknow command!";
                            break;
                        }

                }
            }
        }
    }
}
