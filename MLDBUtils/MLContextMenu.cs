using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MLDBUtils
{
    public partial class MLContextMenu : Form
    {
        public MLContextMenu()
        {
            InitializeComponent();
        }

        private string conStr;

        private MLDBUtils.SQLCom com;

        private DataTable Menus;

        public MLContextMenu(Dictionary<string,object> par)
        {
            InitializeComponent();
            Menus = new DataTable();

            conStr = par["conStr"].ToString();

            com = new SQLCom(conStr, "mGetRepRarts");

            DataTable t;
            com.AddParam(14); com.AddParam(0);
            try
            {
                t = com.GetResult();
                comboBox2.DataSource = t;

                com.setCommand("getAnalityc");
                com.AddParam(26); com.AddParam(1);
                comboBox3.DataSource = com.GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetMenu();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetMenuParams(comboBox1.SelectedValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object cur = comboBox1.SelectedValue;

            if ((int)comboBox1.SelectedValue == 0 & newMenuName.Text.Trim() == "")
            {
                MessageBox.Show("Не введено имя нового меню");
                return;
            }
            string pString = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
             dllName.Text,
             className.Text,
             methodName.Text,
             methodTypes.Text,
             checkBox1.Checked.ToString(),
             constructorParam.Text,
             docType.Text,
             checkBox2.Checked.ToString(),
             checkBox3.Checked.ToString(),
             textBox2.Text
             );

            com.setCommand("mSaveContexMenu");
            com.AddParam(comboBox1.SelectedValue);
            com.AddParam(newMenuName.Text as object);
            com.AddParam(pString as object);
            com.AddParam(comboBox2.SelectedValue);
            com.AddParam(comboBox3.SelectedValue);
            com.AddParam(numericUpDown1.Value as object);



            try
            {
                com.ExecuteCommand();
                MessageBox.Show("Данные сохранены...");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            GetMenu();

            comboBox1.SelectedValue = cur;

        }

        private void GetMenu()
        {
            //DataTable t;
            com.setCommand("mGetRepRarts");
            com.AddParam(25); com.AddParam(0);
            
            try
            {
                Menus.Reset();
                Menus = com.GetResult();
                comboBox1.DataSource = Menus;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetMenuParams(comboBox1.SelectedValue);
        }

        private void GetMenuParams(object menuID)
        {
            if ((int)menuID == 0) return;

            DataTable t;
            com.setCommand("mGetRepRarts");
            com.AddParam(22); com.AddParam(menuID);
            string[] p;
            try
            {
                t = com.GetResult();
                p=t.Rows[0][0].ToString().Split(';');
                comboBox2.SelectedValue =(int)t.Rows[0][1];
                comboBox3.SelectedValue = (int)t.Rows[0][2];
                numericUpDown1.Value = decimal.Parse(t.Rows[0][3].ToString());

                dllName.Text=p[0];
                className.Text=p[1];
                methodName.Text=p[2];
                methodTypes.Text=p[3];
                checkBox1.Checked=bool.Parse(p[4]);
                constructorParam.Text=p[5];
                docType.Text = p[6];
                checkBox2.Checked = bool.Parse(p[7]);
                newMenuName.Text = comboBox1.Text.Split('|')[1];
                checkBox3.Checked = bool.Parse(p[8]);
                textBox2.Text = p[9];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataView dw = new DataView(Menus);
            dw.RowFilter = "Name like '%"+textBox3.Text+"%' or Name='0|Создать новое...'";
            comboBox1.DataSource = dw;
        }

        
    }
}