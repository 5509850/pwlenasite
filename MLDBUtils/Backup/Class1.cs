using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace MLDBUtils
{
	/// <summary>
	/// Класс для работы с MS SQL Server
	/// </summary>
	public class SQLCom
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="constr">строка подключения к серверу</param>
		/// <param name="comtxt">имя процедуры или текст комманды</param>
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
		/// Добавляем параметры для процедуры по одному
		/// </summary>
		/// <param name="param">Значение параметра</param>
		public void AddParam(object param)
		{
			mParams.Add(param);
		}
		/// <summary>
		/// Добавляем параметры для процедуры группой в строке
		/// </summary>
		/// <param name="s">строка со значениями параметров, 
		/// разделитель |
		/// </param>
		public void AddParam(string s)
		{

			string[] param=s.Split('|');
			if(mSrvParams.Count!=param.Length) throw new Exception("Не соответствует число переданных параметров с параметрами полученными с сервера");		  
			for(int i=0;i<mSrvParams.Count;i++)
			{
				mParams.Add(param[i].ToString());
			}
		}
		/// <summary>
		/// Получим параметры передаваемые в процедуру
		/// </summary>
		private void GetParamForServer()
		{
			mSrvParams=new ArrayList();
			using (SqlConnection con=new SqlConnection(mConStr))
			{
				//На сервере должна существовать процедура GetProcParams
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
		/// Генерируем команду с параметрами
		/// </summary>
		private void BuildCommand()
		{
			if(mSrvParams.Count!=mParams.Count) throw new Exception("Не соответствует число переданных параметров с параметрами полученными с сервера");
			for(int i=0;i<mSrvParams.Count;i++)
			{
				mCom.Parameters.Add(new SqlParameter(mSrvParams[i].ToString(),mParams[i]));
			}
			mCom.CommandType=CommandType.StoredProcedure;
		}
		/// <summary>
		/// Получить сгенерированную sql команду
		/// </summary>
		/// <returns>SQL команда</returns>
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
        /// Выполнить команду и вернуть результат
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
				throw new Exception("Ошибка GetResult()");
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
				throw new Exception("Ошибка "+ex.Message);
			}
		}
	}
}
