using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MLDBUtils
{
    public partial class AParamAdd : UserControl
    {
        private int SystemID;
        private int IDinSystem;

        private string constr;
        private SQLCom com;
        private int sqlTask;
        
        public AParamAdd()
        {
            InitializeComponent();

            
        }

        public void Build(string constr, int sqlTask, int SystemID, int IDinSystem)
        {
            
            this.constr = constr;
            this.sqlTask = sqlTask;
            this.SystemID = SystemID; this.IDinSystem = IDinSystem;

            com = new SQLCom(constr, "");
            GetAnalytic();
        }

        private void GetAnalytic()
        {
            DataTable t;
            //if(vGridControl1.Rows.Count>0) vGridControl1.Rows.Clear();
            com.setCommand("getAnalityc");
            com.AddParam(sqlTask); com.AddParam(9);
            t=com.GetResult();
            
            try
            {
                for(int i=0;i<t.Rows.Count;i++)
                    AddRow((int)t.Rows[i][0], t.Rows[i][1].ToString(), 0, GetAValues(t.Rows[i][0]));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DataTable GetAValues(object AID)
        {
            DataTable t;
            com.setCommand("getAnalityc");
            com.AddParam(AID); com.AddParam(1);
            t=com.GetResult();
            return t;
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            
            for(int i=0;i<vGridControl1.Rows.Count;i++)
            {
              com.setCommand("mSetASParam");
              com.AddParam(SystemID); com.AddParam(IDinSystem);
              com.AddParam(int.Parse(vGridControl1.Rows[i].Name)); com.AddParam(vGridControl1.Rows[i].Properties.Value);

            try
            {
                com.ExecuteCommand();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }
            MessageBox.Show("Доп.параметры сохранены");
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetAValues();
        }


        private void AddRow(int pID, string pName, object value, DataTable ds)
        {
         
            DevExpress.XtraVerticalGrid.Rows.EditorRow newRow = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            //newRow.Properties.RowEdit = this.repositoryItemCheckEdit1;

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit leRI = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            leRI.ValueMember = "BeginINT"; leRI.DisplayMember = "Value";
            leRI.DataSource = ds;
            //leRI.Columns[0].Visible = false; leRI.Columns[1].Caption = "Значение";
            newRow.Properties.RowEdit = leRI;
            
            newRow.Name = pID.ToString();
            newRow.Properties.Caption = pName;
            //newRow.Tag = wppID;
            com.setCommand("mGetAParams");
            com.AddParam(pID); com.AddParam(this.SystemID); com.AddParam(IDinSystem);
            com.ExecuteCommand();
            newRow.Properties.Value = com.getRetValue();
            vGridControl1.Rows.Add(newRow);
        
        
        }

        /*
        private void CreateAnalyticRow(VGridControl vg, string caption, string field, int visibleindex, DevExpress.Utils.FormatType formatType, string formatString,DataTable ds)
        {
            DevExpress.XtraVerticalGrid.Rows.EditorRow row = new DevExpress.XtraVerticalGrid.Rows.EditorRow(field);
            
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit leRI = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            leRI.ValueMember = "ID"; leRI.DisplayMember = "Value";
            leRI.DataSource = ds;
            
            vg.Rows.Add(row);
            row.Properties.Caption = caption;
            row.Height = 30;
            row.Properties.Format.FormatType = formatType;
            if (formatType == DevExpress.Utils.FormatType.Custom)
                row.Properties.Format.Format = new BaseFormatter();
            row.Properties.Format.FormatString = formatString;
        }
        */



    }
}
