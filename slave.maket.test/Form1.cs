using pw.lena.Core.Business.ViewModels;
using pw.lena.Core.Data.Models;
using pw.lena.CrossCuttingConcerns.Helpers;
using slave.maket.test.Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slave.maket.test
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        int i = 120;
        PairViewModel pairViewModel;


        public Form1()
        {
            InitializeComponent();
            pairViewModel = FactorySingleton.Factory.Get<PairViewModel>();
        }

        private void button_codeA_Click(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void StartTimer()
        {
            if (!t.Enabled)
            {
                i = 120;
                t.Interval = 1000; // specify interval time as you want
                t.Tick += new EventHandler(timer_Tick);
                t.Start();
                button_codeA.Text = "stop";
                GetCodeA();
            }
            else
            {
                t.Stop();
                t.Tick -= new EventHandler(timer_Tick);
                label_count.Text = string.Empty;
                button_codeA.Text = "get code A";
            }
        }

        private async void GetCodeA()
        {
            TEST test = new TEST();
            await test.GetCodeA(new DeviceModel
            {
                AndroidIDmacHash = "dfdsfreSFDFas",
                TypeDeviceID = 1,
                codeA = 0,
                codeB = 0,
                Name = "android",
                Token = "token"
            }, textBox_codeA);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            i--;
            label_count.Text = string.Format("remain {0}", i);
            if (i == 0)
            {
                Restart();
            }
        }

        private void Restart()
        {
            textBox_codeA.Text = string.Empty;
            i = 120;
        }

        private void button_viewModel_Click(object sender, EventArgs e)
        {
            if (pairViewModel == null)
            {
                return;
            }
            GetCodeAfromViewModel();
        }

        private async void GetCodeAfromViewModel()
        {
            var device = new DeviceModel
            {
                AndroidIDmacHash = "dfdsfreSFDFas",
                TypeDeviceID = 1,
                codeA = 0,
                codeB = 0,
                Name = "android",
                Token = "token"
            };
            
            Pair pair = await pairViewModel.GetCodeATestingOnly(device);
            label_code.Text = pair.CodeA.ToString();
        }

        private void button_exp_Click(object sender, EventArgs e)
        {
            textBox_exp.Text = ConverterHelper.ConvertDateTimeToMillisec(DateTime.Now).ToString();
            textBox_data.Text = ConverterHelper.ConvertMillisecToDateTime(Convert.ToInt64(textBox_exp.Text)).ToLongTimeString();
            
        }

        private void label_count_Click(object sender, EventArgs e)
        {

        }

        private void textBox_codeA_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
