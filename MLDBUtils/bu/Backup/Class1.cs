using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace MLDBUtils
{

    
	/// <summary>
	/// ����� ��� ������ � MS SQL Server
	/// </summary>
	public class SQLCom
	{
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="constr">������ ����������� � �������</param>
		/// <param name="comtxt">��� ��������� ��� ����� ��������</param>
		public SQLCom(string constr,string comtxt)
		{
			// 
			// TODO: Add constructor logic here
			//
            mConStr = constr+";Connect Timeout=360;";
            mCom.CommandTimeout = 540;
			mCom.CommandText=comtxt;
            
            try
            {
                GetParamForServer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        public void setCommand(string comText)
        {
            retValue = 0;
            mCom.CommandText = comText;
            mCom.Parameters.Clear();
            mParams.Clear();
            mSrvParams.Clear();
            mSrvParamsType.Clear();
            GetParamForServer();
        }

        public void clearParams()
        {
            retValue = 0;
            mCom.Parameters.Clear();
            mParams.Clear();
        }

		
		private SqlCommand mCom=new SqlCommand();
		private ArrayList mParams=new ArrayList();
		private ArrayList mSrvParams=new ArrayList();
        private ArrayList mSrvParamsType = new ArrayList();
		private string mConStr;

		/// <summary>
		/// ��������� ��������� ��� ��������� �� ������
		/// </summary>
		/// <param name="param">�������� ���������</param>
		public void AddParam(object param)
		{
			mParams.Add(param);
		}
		/// <summary>
		/// ��������� ��������� ��� ��������� ������� � ������
		/// </summary>
		/// <param name="s">������ �� ���������� ����������, 
		/// ����������� |
		/// </param>
        /// 

        
        public void AddParams(object[] v)
        {
            foreach (object obj in v)
                AddParam(obj);
        }

		public void AddParam(string s)
		{

			string[] param=s.Split('|');
            if (mSrvParams.Count < param.Length) throw new Exception("�� ������������� ����� ���������� ���������� � ����������� ����������� � �������\n" + mSrvParams.Count.ToString() + "-" + param.Length.ToString()+
            "������� "+mCom.CommandText);
            for (int i = 0; i < param.Length; i++)
			{
				mParams.Add(param[i].ToString());
			}
		}

        public string GetSrvParamCount()
        {
            return mSrvParams.Count.ToString();
        }

        public void AddParam(string s,byte[] img,string pName)
        {

            string[] param = s.Split('|');
            if (mSrvParams.Count != param.Length+1) throw new Exception("�� ������������� ����� ���������� ���������� � ����������� ����������� � �������\n" + mSrvParams.Count.ToString() + "-" + param.Length.ToString());
            
                for (int i = 0; i < mSrvParams.Count - 1; i++)
                {
                    mParams.Add(param[i].ToString());
                }
            
            mCom.Parameters.Add(new SqlParameter(pName, SqlDbType.Image, img.Length));
            
        }
        /// <summary>
        /// �������� ��� ���������
        /// </summary>
        /// <param name="pNum">����� � �������</param>
        /// <returns></returns>
        public string getParamName(int pNum)
        {
            string ret = "";
            try
            {
                ret = mSrvParams[pNum].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }
		/// <summary>
		/// ������� ��������� ������������ � ���������
		/// </summary>
		private void GetParamForServer()
		{
			//mSrvParams=new ArrayList();
			using (SqlConnection con=new SqlConnection(mConStr))
			{
				//�� ������� ������ ������������ ��������� GetProcParams
				SqlCommand com=new SqlCommand("GetProcParams",con);
				//com.Parameters.Add(new SqlParameter("@Name", mCom.CommandText));
                com.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                com.Parameters["@Name"].Value = mCom.CommandText;
				com.CommandType=CommandType.StoredProcedure;
				
				try
				{
					con.Open();
					SqlDataReader reader=com.ExecuteReader();
					while(reader.Read())
					{
						mSrvParams.Add(reader.GetString(0));
                        mSrvParamsType.Add(reader.GetValue(1));
					}
					reader.Close();
                    
				}
				catch(Exception ex)
				{
					throw new Exception(ex.Message);
				}
				finally
				{
					con.Close();
				}
			}
		}
		/// <summary>
		/// ���������� ������� � �����������
		/// </summary>
		private void BuildCommand()
		{
            //mCom.Parameters.Clear();
			if(mSrvParams.Count<mParams.Count) throw new Exception("�� ������������� ����� ���������� ���������� � ����������� ����������� � �������. DllProc BuilDCommand\n"+
                   "� ������� - "+mSrvParams.Count.ToString() + " �� Dll " + mParams.Count);
            try
            {
                for (int i = 0; i < mSrvParams.Count; i++)
                {
                    //mCom.Parameters.Add(new SqlParameter(mSrvParams[i].ToString(), mParams[i]));
                    mCom.Parameters.Add(BuildSqlParametr(i));
                }
            }
            catch(Exception ex)
            {
                throw new Exception("������ ���������� ���������� � SQL �������\n"+ex.Message);
            }
			mCom.CommandType=CommandType.StoredProcedure;
            mCom.Parameters.Add(new SqlParameter("@ret", SqlDbType.Int));
            mCom.Parameters["@ret"].Direction = ParameterDirection.ReturnValue;
		}

        /// <summary>
        /// ��������� SQL ��������� �� ������ ������ � ������� � ���������� � ���������� �����
        /// ����������
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private SqlParameter BuildSqlParametr(int i)
        {
            SqlParameter sqlParam;
            
            try
            {
                switch (int.Parse(mSrvParamsType[i].ToString()))
                {
                    //int
                    case 56:
                        int number=0;
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.Int);
                        int.TryParse(mParams[i].ToString(), out number);
                        sqlParam.Value = number;// int.Parse(mParams[i].ToString());
                        break;
                    //smalldatetime
                    case 58:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.DateTime);
                        if (mParams[i].GetType() == System.Type.GetType("System.DateTime"))
                            sqlParam.Value = mParams[i];
                        sqlParam.Value = DateTime.Parse(mParams[i].ToString());
                        break;
                    //datetime
                    case 61:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.DateTime);
                        if(mParams[i].GetType()==System.Type.GetType("System.DateTime"))
                            sqlParam.Value = mParams[i];
                        else
                        sqlParam.Value = DateTime.Parse(mParams[i].ToString());
                        break;
                    //Numeric
                    case 108:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.Decimal);
                        sqlParam.Value = Decimal.Parse(mParams[i].ToString().Replace('.',','));
                        break;
                    //Decimal
                    case 106:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.Decimal);
                        sqlParam.Value = Decimal.Parse(mParams[i].ToString());
                        break;
                    //money
                    case 60:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.Money);
                        sqlParam.Value = Decimal.Parse(mParams[i].ToString());
                        break;
                    //varchar
                    case 167:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.VarChar);
                        sqlParam.Value = mParams[i].ToString();
                        break;
                    //nvarchar
                    case 231:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.VarChar);
                        sqlParam.Value = mParams[i].ToString();
                        break;
                    //uniqueidentifier
                    case 36:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), SqlDbType.UniqueIdentifier);
                        sqlParam.Value =new Guid(mParams[i].ToString());
                        break;
                    default:
                        sqlParam = new SqlParameter(mSrvParams[i].ToString(), mParams[i]);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("������ ������������� ��������� BuildSqlParam\n" + mSrvParams[i].ToString()+"\n"+mSrvParamsType[i].ToString()+"\n" +
                    "�������� " + mParams[i].ToString()+"\n "+"����� ��������� i="+i.ToString()+"\n������� "+mCom.CommandText+"\n"+ ex.Message);
            }

            return sqlParam;
        }
		/// <summary>
		/// �������� ��������������� sql �������
		/// </summary>
		/// <returns>SQL �������</returns>
		public SqlCommand GetCommand()
		{
			try
			{
				BuildCommand();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return mCom;
		}
        /// <summary>
        /// ��������� ������� � ������� ���������
        /// </summary>
        /// <returns>DataTable</returns>
		public DataTable GetResult()
		{
			DataTable dt=new DataTable("Result");
			try
			{
				BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    da.Fill(dt);
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if ( retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                    }
                }
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message+"\n������ GetResult()");
			}
			return dt;

		}
        
        public void FillDataDS(DataSet ds)
        {
            try
            {
                BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    da.Fill(ds);
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if (retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                    }
                }
                for (int i = 0; i < ds.Tables.Count; i++)
                    ds.Tables[i].TableName = "Table" + i.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n������ FillDataDS()");
            }


        }
        
        /// <summary>
        /// ��������� ������� � ������� ���������
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetResultDS()
        {
            DataSet ds=new DataSet("DSResult");

            try
            {
                BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    da.Fill(ds);
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if (retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                    }
                }
                for (int i = 0; i < ds.Tables.Count; i++)
                    ds.Tables[i].TableName = "Table" + i.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n������ GetResultDS()");
            }
            
            
            
            return ds;

        }

        /// <summary>
        /// ��������� ������� � ������ ������ � Dictionary
        /// </summary>
        /// <returns>DataTable</returns>
        public Dictionary<string,object> GetResultD()
        {
            DataTable dt = new DataTable("Result");
            Dictionary<string,object> ret=new Dictionary<string,object>();
            try
            {
                BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    da.Fill(dt);
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if (retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                        
                    }
                    if(dt.Rows.Count>0)
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ret.Add(dt.Columns[i].ColumnName, dt.Rows[0][i]);
                        }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n������ GetResultD()");
            }
            return ret;

        }
        
        public int AddDataEx1(DataTable dt)
        {
            
            try
            {
                BuildCommand();
                
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    
                    da.Fill(dt);
                    
                    //if(da.SelectCommand.Parameters.Count>0)
                    //{
                    
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if (retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n������ GetResult()");
            }
            return retValue;
        }
        
        public int FillData(DataTable dt)
        {
            dt.Clear();
            return AddDataEx1(dt);
            //return ret;
            /*
            try
            {
                BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(mCom);
                    da.Fill(dt);
                    retValue = (int)da.SelectCommand.Parameters["@ret"].Value;
                    if (retValue < 0)
                    {
                        throw new Exception(GetError((int)da.SelectCommand.Parameters["@ret"].Value) +
                            "\n��������� - " + da.SelectCommand.CommandText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n������ GetResult()");
            }
            
            return retValue;
            */
        }
        /// <summary>
        /// ������������ ���������� ��������
        /// </summary>
        private int retValue;

        public int getRetValue()
        {
            return retValue;
        }
		/// <summary>
		/// ��������� �������
		/// </summary>
		public void ExecuteCommand()
		{
            
            
			try
			{
                BuildCommand();
                using (SqlConnection con = new SqlConnection(mConStr))
                {
                    mCom.Connection = con;
                    mCom.Connection.Open();
                    mCom.ExecuteNonQuery();
                    retValue = (int)mCom.Parameters["@ret"].Value;

                    if (retValue < 0)
                    {
                        throw new Exception(GetError(retValue) +
                            "\n��������� - " + mCom.CommandText);
                    }
                }
                
			}
			catch(Exception ex)
			{
				throw new Exception("������ MLDBUTILS.ExecuteCommand "+ex.Message+"\n retValue="+retValue.ToString());
			}
		}
        /// <summary>
        /// ������� ������ ���������� ���� ������ 
        /// </summary>
        /// <param name="error">����� ������</param>
        /// <returns>����� ������</returns>
        private string GetError(int error)
        {
          using(SqlConnection con = new SqlConnection(mConStr))
          {
              SqlCommand com = new SqlCommand("getError", con);
              com.CommandType = CommandType.StoredProcedure;
              com.Parameters.Add(new SqlParameter("@Error", SqlDbType.Int));
              com.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar,50));
              com.Parameters["@Value"].Direction = ParameterDirection.Output;

              com.Parameters["@Error"].Value = error;

              try
              {
                  con.Open();
                  com.ExecuteNonQuery();
              }
              catch (SqlException ex)
              {
                  throw ex;
              }
              con.Close();
              return com.Parameters["@Value"].Value.ToString();
          }
        }
        /// <summary>
        /// ��������� � ������� �������� ���� BLOB
        /// </summary>
        /// <param name="img">BLOB</param>
        /// <param name="pName">��� ���������</param>
        
        public void AddImageParam(byte[] img,string pName)
        {
            mCom.Parameters.Add(new SqlParameter(pName, SqlDbType.Image,img.Length));
        }
        /// <summary>
        /// ���������� ���������� �� �������
        /// </summary>
        /// <param name="?">������� � �����������</param>
        public void AddParams(Dictionary<string,object> dParams)
        {
            if (dParams.Count < mSrvParams.Count)
                throw new Exception(string.Format("�� ������������� ����� ���������� � ������� ({0}) � �� ������� dParams ({1})",
                    mSrvParams.Count,dParams.Count));
            for (int i = 0; i < mSrvParams.Count; i++)
            {

                if (dParams.ContainsKey(mSrvParams[i].ToString().Replace("@", "")))
                    AddParam(dParams[mSrvParams[i].ToString().Replace("@", "")]);
                else
                    AddParam(0);
                    //throw new Exception(string.Format("����������� �������� � ������� dParams ({0})", mSrvParams[i].ToString().Replace("@", "")));
            }
        }
    }

    
}
