using System;
using System.Collections.Generic;
using System.Text;

namespace MLDBUtils
{
    public class DXUtils
    {
        public static void ExplandFirstRow(DevExpress.XtraGrid.Views.Grid.GridView gw)
        {
            gw.CollapseAllGroups();
            gw.CollapseAllGroups();
            /*int rowHandle = -1;
            do
            {
                rowHandle = rowHandle - 1;


            } while (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle);
             * */
            gw.SetRowExpanded(-1, true);
        }

        public static string getStrElement(char separator,int index,string str)
        {
            string[] strArray=str.Split(separator);

            if (strArray.Length < index) return "";

            return strArray[index];
           
        }

        public static object stringToObject(Type type,string s)
        {
            
            switch (type.ToString())
            {
                case "System.Windows.Forms.FormBorderStyle":
                    if(s=="FixedDialog")
                     return System.Windows.Forms.FormBorderStyle.FixedDialog;
                    else return System.Windows.Forms.FormBorderStyle.Sizable;
                    break;
                

            }
            return s;
        }
    }
}
