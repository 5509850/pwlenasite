using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MLDBUtils
{
    public partial class ChPwd : Form
    {
        public ChPwd(string conStr)
        {
            InitializeComponent();

            this.conStr = conStr;
        }

        private string conStr;

        private void button1_Click(object sender, EventArgs e)
        {
            if (tNew2.Text != tNew1.Text)
            {
                MessageBox.Show("Неправильное подтверждение пароля.");
                return;
            }
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand com = new SqlCommand("sp_password", con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter("@Old", SqlDbType.NVarChar));
                com.Parameters.Add(new SqlParameter("@New", SqlDbType.NVarChar));
                com.Parameters["@Old"].Value=tOld.Text;
                com.Parameters["@New"].Value = tNew2.Text;

                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    this.conStr=conStr.Replace(tOld.Text, tNew1.Text);
                    MessageBox.Show("Пароль сменен успешно.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.Close();
        }

        public string GetConStr()
        {
            return conStr;
        }
    }
}