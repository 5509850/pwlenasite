using lenapw.test.Helpers;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace lenapw.test
{
    public partial class sqlinstall : System.Web.UI.Page
    {        
        protected string connectionString = ConfigurationManager.ConnectionStrings["alexandr_gorbunov_ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

#if (!DEBUG)
            if (!Request.IsSecureConnection)
            {   
                //not work in CHROME or Firefox (use opera)
                Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"));   
            }           
#endif
            Button_submit.Click += Button_submit_Click;         
        }

        private void Button_submit_Click(object sender, EventArgs e)
        {            
            if (!TextBox_code.Text.Equals(ConfigurationManager.AppSettings["sqlaccesscode"]))
            {
                Label_mess.Text = "invalid access code!";
                return;
            }
         
            if (string.IsNullOrEmpty(TextBox_SQL.Text))
            {
                Label_mess.Text = "invalid sql!";
                return;
            }

            Label_mess.Text = string.Empty;
            
            string queryString = TextBox_SQL.Text;
            connectionString = Utils.InitSqlPath(connectionString);
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);                
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var count = reader.FieldCount;
                    var sb = new StringBuilder();
                    while (reader.Read())
                    {                        
                        for (int i = 0; i < count; i++)
                        {
                            sb.Append(string.Format("\t{0}", !string.IsNullOrEmpty(reader[i].ToString())  ? reader[i]: "NULL"));
                        }
                        sb.Append("\n");                       
                    }
                    TextBox_result.Text = sb.ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Label_mess.Text = ex.Message;
                }
            }
        }

        protected void Button_List_Tables_Click(object sender, EventArgs e)
        {
            if (!TextBox_code.Text.Equals(ConfigurationManager.AppSettings["sqlaccesscode"]))
            {
                Label_mess.Text = "invalid access code!";
                return;
            }                    

            Label_mess.Text = string.Empty;

            string queryString = "select TABLE_NAME from information_schema.tables where TABLE_NAME LIKE 'b%'";
            connectionString = Utils.InitSqlPath(connectionString);
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var count = reader.FieldCount;
                    var sb = new StringBuilder();
                    int i = 0;
                    sb.Append("TABLES({XXXXXXX}):\n");
                    while (reader.Read())
                    {
                        i++;
                        sb.Append(string.Format("{0}\t{1}", i, reader[0]));
                        sb.Append("\n");                       
                    }
                    TextBox_result.Text = sb.ToString().Replace("{XXXXXXX}", i.ToString());
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Label_mess.Text = ex.Message;
                }
            }
        }

        protected void Button_list_SP_Click(object sender, EventArgs e)
        {
            if (!TextBox_code.Text.Equals(ConfigurationManager.AppSettings["sqlaccesscode"]))
            {
                Label_mess.Text = "invalid access code!";
                return;
            }           

            Label_mess.Text = string.Empty;

            string queryString = "SELECT  QUOTENAME( name ) as 'sp'  FROM sys.objects WHERE type = 'P' ORDER BY QUOTENAME( name )";
            connectionString = Utils.InitSqlPath(connectionString);
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var sb = new StringBuilder();
                    int i = 0;
                    sb.Append("Stored Procedures({XXXXXXX}):\n");
                    while (reader.Read())
                    {
                        i++;
                        sb.Append(string.Format("{0}\t{1}", i, reader[0]));
                        sb.Append("\n");
                    }
                    TextBox_result.Text = sb.ToString().Replace("{XXXXXXX}", i.ToString()); ;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Label_mess.Text = ex.Message;
                }
            }
        }

        protected void Button_clear_Click(object sender, EventArgs e)
        {
            TextBox_SQL.Text = string.Empty;            
        }
    }
}