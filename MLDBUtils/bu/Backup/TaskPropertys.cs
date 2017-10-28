using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraEditors.Repository;


namespace MLDBUtils
{
    public partial class TaskPropertys : Form
    {
        private int taskID;
        private int wpID;
        private SQLCom com;

        public TaskPropertys(string conStr,int taskID,int wpID)
        {
            InitializeComponent();

            this.taskID = taskID; this.wpID = wpID;
            com = new SQLCom(conStr, "");

            // Create a repository item which represents an in-place CheckEdit control.
            //RepositoryItemCheckEdit riCheckEdit = new RepositoryItemCheckEdit();
            // Represent check boxes as radio buttons.
            //riCheckEdit.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            // Associate the Boolean data type with the created repository item.
            //propertyGridControl1.DefaultEditors.Add(typeof(Boolean), riCheckEdit);

            try
            {
                GetPropertyesByTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddRow(int pID,int rowType, string pName,object value,int wppID)
        {
            DevExpress.XtraVerticalGrid.Rows.EditorRow newRow = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            newRow.Properties.RowEdit = this.repositoryItemCheckEdit1;
            newRow.Name = pID.ToString();
            newRow.Properties.Caption = pName;
            newRow.Tag = wppID;
            newRow.Properties.Value = bool.Parse(value.ToString());
            vGridControl1.Rows.Add(newRow);
            
        }

        private void GetPropertyesByTask()
        {

            DataTable t; 
            com.setCommand("mGetWPPropertyes");
            com.AddParam(taskID); com.AddParam(wpID); com.AddParam(0);
            try
            {
                t=com.GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception("Function GetPropertyesByTask "+ex.Message);
            }

            for (int i = 0; i < t.Rows.Count; i++)
            {
                AddRow((int)t.Rows[i][0], (int)t.Rows[i][1], t.Rows[i][2].ToString(), t.Rows[i][4], (int)t.Rows[i][6]);
            }



        }

        private void repositoryItemCheckEdit1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("ssss");
//            editorRow1.Properties.Value = true;
            

        }

        private object GetRowValue(int i)
        {
            return vGridControl1.Rows[i].Properties.Value;
        }

        private int GetRowID(int i)
        {
            return int.Parse(vGridControl1.Rows[i].Name);
        }

        private object GetWPPID(int i)
        {
            return vGridControl1.Rows[i].Tag;
        }

        private void TaskPropertys_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show(GetRowID(0).ToString()+" "+GetRowValue(0).ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveUserParams();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveUserParams()
        {
            com.setCommand("mSaveUserProperty");
            for(int i=0;i<vGridControl1.Rows.Count;i++)
            {
             com.clearParams();
             com.AddParam(GetRowID(i));com.AddParam(GetWPPID(i));com.AddParam(GetRowValue(i));
             try
             {
                 com.ExecuteCommand();
             }
                catch(Exception ex)
             {
                    throw ex;
             }
            }
        }
    }
}