using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MLDBUtils
{
    public partial class NewUser : Form
    {
        public NewUser()
        {
            InitializeComponent();
        }

        public NewUser(string conStr,object UserID)
        {
            InitializeComponent();

            SQLCom com=new SQLCom(conStr,"getAnalityc");
            com.AddParam(UserID); com.AddParam(12);
            try
            {
                DataTable t = com.GetResult();
                textBox4.Text = t.Rows[0][0].ToString();
                textBox1.Text = t.Rows[0][3].ToString();
                textBox2.Text = t.Rows[0][1].ToString();
                textBox3.Text = t.Rows[0][2].ToString();
                textBox5.Text = t.Rows[0][5].ToString();
                textBox6.Text = t.Rows[0][6].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public object GetLastName()
        {
            return (object)textBox1.Text.Trim();
        }

        public object GetFirstName()
        {
            return (object)textBox2.Text.Trim();
        }

        public object GetMidName()
        {
            return (object)textBox3.Text.Trim();
        }

        public object GetLogin()
        {
            return (object)textBox4.Text.Trim();
        }

        public object GetPhones()
        {
            return (object)textBox5.Text;
        }

        public object GetPost()//должность
        {
            return (object)textBox6.Text;
        }

 
    }
}