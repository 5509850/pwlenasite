using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MLDBUtils;

namespace MLDBUtils
{
    public partial class UserAdmin : Form
    {
        private string conStr;
        private SQLCom com;
        public UserAdmin(string conStr)
        {
            InitializeComponent();

            this.conStr = conStr;

            com = new SQLCom(conStr, "mGetRepRarts");

            try
            {
                GetOfficers();
                GetTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            
        }

        private void GetOfficers()
        {
            int curOfficer;
            if (comboBox1.SelectedValue == null) curOfficer = -1;
            else curOfficer = (int)comboBox1.SelectedValue;

                try
                {
                    com.setCommand("mGetRepRarts");
                    com.AddParam("12|0");
                    comboBox1.DataSource = com.GetResult();
                    com.setCommand("mGetRepRarts");
                    com.AddParam("12|0");
                    comboBox2.DataSource = com.GetResult();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " функция GetOfficer");
                }
            if (curOfficer >= 0)
                comboBox1.SelectedValue = curOfficer;
        }

        private void GetTasks()
        {
            

            try
            {
                com.setCommand("mGetRepRarts");
                com.AddParam("14|0");
                listBox2.DataSource = com.GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " функция GetTasks");
            }
            
        }

        private void GetUserTasks()
        {
            
            listBox1.DataSource = null;
            listBox1.ValueMember = "TaskID";
            listBox1.DisplayMember = "Name";
            
            try
            {
                com.setCommand("mGetRepRarts");
                com.AddParam(15); com.AddParam(comboBox1.SelectedValue);
                listBox1.DataSource = com.GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " функция GetUserTasks");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetUserTasks();
                GetOfficerRelation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void SetUserTasks(int action)
        {

            
            try
            {
                com.setCommand("mSetTaskToOfficer");
                if(action==1) com.AddParam(listBox2.SelectedValue);
                else com.AddParam(listBox1.SelectedValue);
                com.AddParam(comboBox1.SelectedValue);
                com.AddParam(action);
                listBox1.DataSource = com.GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " функция SetUserTasks");
            }
            GetUserTasks();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SetUserTasks(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUserTasks(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NewUser f = new NewUser();

            if (
                MessageBox.Show("Пользователь выполняющий данное действие должен обладать ролью sa_role или sso_role.\nПродолжить?", "ВНИМАНИЕ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        com.setCommand("mAddUser");
                        com.AddParam(f.GetLastName());
                        com.AddParam(f.GetFirstName());
                        com.AddParam(f.GetMidName());
                        com.AddParam(f.GetLogin());
                        com.AddParam(f.GetPhones());
                        com.AddParam(f.GetPost());
                        com.AddParam(f.GetPostForPrint());
                        com.AddParam(f.GetFIOForPrint());
                        com.AddParam(f.GetEMAIL());
                        

                        com.ExecuteCommand();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    GetOfficers();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                com.setCommand("mAddOfficerRelation");
                com.AddParam(comboBox1.SelectedValue);
                com.AddParam(comboBox2.SelectedValue);
               
                com.ExecuteCommand();
                GetOfficerRelation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void GetOfficerRelation()
        {
            listBox3.DataSource = null;
            listBox3.ValueMember = "ID";
            listBox3.DisplayMember = "OfficerName";
            try
            {
                com.setCommand("mGetRepRarts");
                com.AddParam(19); com.AddParam(comboBox1.SelectedValue);
                listBox3.DataSource = com.GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " функция GetOfficerRelation");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                com.setCommand("mDelRepPart");
                com.AddParam(listBox3.SelectedValue);
                com.AddParam(6);

                com.ExecuteCommand();
                GetOfficerRelation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NewUser f = new NewUser(this.conStr,comboBox1.SelectedValue);

                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        com.setCommand("mAddUser");
                        com.AddParam(f.GetLastName());
                        com.AddParam(f.GetFirstName());
                        com.AddParam(f.GetMidName());
                        com.AddParam(f.GetLogin());
                        com.AddParam(f.GetPhones());
                        com.AddParam(f.GetPost());
                        com.AddParam(f.GetPostForPrint());
                        com.AddParam(f.GetFIOForPrint());
                        com.AddParam(f.GetEMAIL());

                        com.ExecuteCommand();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    GetOfficers();
                }
            
        }

    }
}