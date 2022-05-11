using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;

/// <summary>
/// 数据访问抽象基础类
/// Copyright (C) Devgis
/// </summary>
public abstract class DbHelperOra
{
    private static String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ToString();
    public DbHelperOra()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="SQLString">查询语句</param>
    /// <returns>DataSet</returns>
    public static DataSet Query(string SQLString)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                command.Fill(ds, "ds");
            }
            catch (System.Data.OracleClient.OracleException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
    }
}