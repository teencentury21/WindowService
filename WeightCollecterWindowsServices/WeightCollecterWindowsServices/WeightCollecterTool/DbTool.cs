using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel;
using Inventec.FIS.Utilities.Data;

namespace WeightCollecterTool
{
	public class DbTool
	{
        private class DBConnectionConstants
        {            
            public const string CTO = "CTO_SQL_ConnectionString";         
        }

        public static QueryTool _queryTool;

		static DbTool()
		{
			var factory = new QueryToolFactory(DBConnectionConstants.CTO);

			_queryTool = factory.CreateInstance();
		}


		public static DataTable GetData(string sql)
		{
			return _queryTool.GetData(sql);
		}

		public static int ExecuteNoneQuery(string sql, List<SqlParameterMapper> mappers, IDbTransaction transaction)
		{
			return _queryTool.ExecuteNoneQuery(sql, mappers, transaction);
		}

		public static int ExecuteNoneQuery(string sql, List<SqlParameterMapper> mappers)
		{
			return _queryTool.ExecuteNoneQuery(sql, mappers);
		}

		public static int ExecuteNoneQuery(string sql)
		{
			return _queryTool.ExecuteNoneQuery(sql);
		}

		public static DataTable GetData(string sql, List<SqlParameterMapper> mappers, IDbTransaction transaction)
		{
			return _queryTool.GetData(sql, mappers, transaction);
		}

		public static DataTable GetData(string sql, List<SqlParameterMapper> mappers, IDbConnection connection)
		{
			return _queryTool.GetData(sql, mappers, connection);
		}

		public static DataTable GetData(string sql, List<SqlParameterMapper> mappers)
		{
			return _queryTool.GetData(sql, mappers);
		}

		public static object ExecuteScalar(string sql, List<SqlParameterMapper> mappers)
		{
			return _queryTool.ExecuteScalar(sql, mappers);
		}

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }


	}
}