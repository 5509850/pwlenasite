using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormsTestSQL
{
    public partial class Form1 : Form
    {

        string connectionString = @"Data Source=10.143.25.93;Initial Catalog=alexandr_gorbunov_; Persist Security Info=True;User ID=sa;Password=a#40284028;";
        private MLDBUtils.SQLCom MyCom;
        private const int ANDROID_SLAVE = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetCodeA(textBox2.Text, ANDROID_SLAVE);
        }

        private string GetCodeA(string hashDevice, int typeDevice)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string result = string.Empty;
            try
            {
                MyCom = new MLDBUtils.SQLCom(connectionString, "");

                MyCom.setCommand("bGETcodeA");
                MyCom.AddParam(Utils.getRandom());
                MyCom.AddParam(hashDevice); //hash
                MyCom.AddParam(typeDevice);

                dic = MyCom.GetResultD();

                if (dic == null || dic.Count == 0)
                {
                    return "-1";
                }
                result = dic["a"].ToString();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return "-3";
            }

            return result;
        }
    }
}
