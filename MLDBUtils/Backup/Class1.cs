using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

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
			mConStr=constr;
			mCom.CommandText=comtxt;
			GetParamForServer();
		}

		
		private SqlCommand mCom=new SqlCommand();
		private ArrayList mParams=new ArrayList();
		private ArrayList mSrvParams;
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
		public void AddParam(string s)
		{

			string[] param=s.Split('|');
			if(mSrvParams.Count!=param.Length) throw new Exception("�� ������������� ����� ���������� ���������� � ����������� ����������� � �������");		  
			for(int i=0;i<mSrvParams.Count;i++)
			{
				mParams.Add(param[i].ToString());
			}
		}
		/// <summary>
		/// ������� ��������� ������������ � ���������
		/// </summary>
		private void GetParamForServer()
		{
			mSrvParams=new ArrayList();
			using (SqlConnection con=new SqlConnection(mConStr))
			{
				//�� ������� ������ ������������ ��������� GetProcParams
				SqlCommand com=new SqlCommand("GetProcParams",con);
				com.Parameters.Add(new SqlParameter("@Name", mCom.CommandText));
				com.CommandType=CommandType.StoredProcedure;
				
				try
				{
					con.Open();
					SqlDataReader reader=com.ExecuteReader();
					while(reader.Read())
					{
						mSrvParams.Add(reader.GetString(0));	
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
			if(mSrvParams.Count!=mParams.Count) throw new Exception("�� ������������� ����� ���������� ���������� � ����������� ����������� � �������");
			for(int i=0;i<mSrvParams.Count;i++)
			{
				mCom.Parameters.Add(new SqlParameter(mSrvParams[i].ToString(),mParams[i]));
			}
			mCom.CommandType=CommandType.StoredProcedure;
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
				mCom.Connection=new SqlConnection(this.mConStr);
				SqlDataAdapter da=new SqlDataAdapter(mCom);
				
				da.Fill(dt);
			}
			catch(Exception ex)
			{
				throw new Exception("������ GetResult()");
			}
			return dt;

		}
		
		public void ExecuteCommand()
		{
			try
			{
				BuildCommand();
				mCom.Connection=new SqlConnection(this.mConStr);
				mCom.Connection.Open();
				mCom.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw new Exception("������ "+ex.Message);
			}
		}
	}
}
