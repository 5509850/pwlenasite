using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MLDBUtils
{
    public partial class WaitForm : Form
    {
        delegate void MyDelegate(string s);
        delegate void MyDelegate1();
        public WaitForm()
        {
            InitializeComponent();
            label1.Text = "Выполнение процесса ...";
            
        }
        
        public string fileName;
        public int fileID;
        public int lastValue;
        private int currentValue;
        private int isStop = 0;

        public int setCurrentValue(int v)
        {
            if (v == lastValue-1) ExClose();
            else
            {
                currentValue = v;
                setInfo(string.Format("Обработано {0} из {1}", currentValue, lastValue));
            }
            return isStop;
        }

        public void setInfo(string text)
        {
            if (this.label2.InvokeRequired)
            {
                MyDelegate d = new MyDelegate(setInfo);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label2.Text = text;
            }
            //label2.Text = s;
            //Update();
        }

        public void ExClose()
        {
            if (this.InvokeRequired)
            {
                MyDelegate1 d = new MyDelegate1(Close);
                Invoke(d);
            }
            else
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isStop = 1;
        }
        
        

        //private int i=0;
        /*
        delegate void MyDelegate2(object obj,EventArgs ea);

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (this.label2.InvokeRequired)
            {
                MyDelegate2 d = new MyDelegate2(timer1_Tick);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                MessageBox.Show("TTT");
                timer1.Stop();
                i++;
                setInfo("Время (сек.): " + i.ToString());
                timer1.Enabled = true;
                timer1.Start();    
            }
            
            
        }

        public void startT()
        {
            //MessageBox.Show("TTTT");
            timer1.Enabled = true;
            timer1.Start();
        }
         */       
        
    }
}