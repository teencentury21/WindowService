using Inventec.FIS.Model.Domiain.Constrants;
using Inventec.FIS.Model.Domiain.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Inventec.FIS.Model.Domiain.Services
{
    public static class WeightCollectService
    {
        public static List<FileStockEntity> GetFileStockDatas(string category)
        {
            var resullts = new List<FileStockEntity>();
            var connectionString = ConfigurationManager.ConnectionStrings["CTO_SQL_ConnectionString"].ConnectionString;
            string queryString = string.Format("Select * from FILE_STOCK (nolock) Where Category = '{0}'", category);
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var row = command.ExecuteReader())
                {
                    while (row.Read())
                    {
                        var o = new FileStockEntity();
                        o.Id = Convert.ToInt32(row["Id"]);
                        o.Type = Convert.ToString(row["Type"]);
                        o.Site = Convert.ToString(row["Site"]);
                        o.Category = Convert.ToString(row["Category"]);
                        o.Host = Convert.ToString(row["Host"]);
                        o.Port = Convert.ToInt32(row["Port"]);
                        o.Domain = Convert.ToString(row["Domain"]);
                        o.UserName = Convert.ToString(row["UserName"]);
                        o.Password = Convert.ToString(row["Password"]);
                        o.RemoteDir = Convert.ToString(row["RemoteDir"]);
                        o.Active = Convert.ToBoolean(row["Active"]);
                        o.Remark = Convert.ToString(row["Remark"]);
                        o.Creator = Convert.ToString(row["Creator"]);
                        o.Editor = Convert.ToString(row["Editor"]);
                        o.Cdt = Convert.ToDateTime(row["Cdt"]);
                        o.Udt = Convert.ToDateTime(row["Udt"]);
                        o.EKey = Convert.ToString(row["EKey"]);
                        resullts.Add(o);
                    }
                }
            }
            return resullts;
        }

        public static void WriteFailFile(string host, string workingPath, string errorMessage)
        {
            var fileName = string.Format("{0}{4} {1}@{2}@{3}.txt", WeightDifConstrants.Exception.Value, WeightDifConstrants.FAIL, 
                DateTime.Now.ToString("yyyy-MM-dd HH"), host, FileStockTypes.TCP.Value);
            var logTime = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + Environment.NewLine;
            var txtfile = Path.Combine(workingPath, fileName);
            if (File.Exists(txtfile))
            {
                var oldMessage = File.ReadAllText(txtfile);
                errorMessage = logTime + errorMessage + Environment.NewLine + Environment.NewLine + oldMessage;
                File.WriteAllText(txtfile, errorMessage, Encoding.Unicode);
            }
            else
            {
                errorMessage = logTime + errorMessage + Environment.NewLine + Environment.NewLine;
                File.WriteAllText(txtfile, errorMessage, Encoding.Unicode);
            }
        }

        public static void InsertWeightData(WeighResultEntity inputData)
        {
            try
            {
                if (inputData.SN.StartsWith("3S"))
                {
                    InsertWeightOrighData(inputData);
                    inputData.SN = inputData.SN.Substring(2, inputData.SN.Length - 2);
                }
                var connectionString = ConfigurationManager.ConnectionStrings["CTO_SQL_ConnectionString"].ConnectionString;
                var sql = string.Format(@"
                if not exists (Select top 1 Sno from [WEIGHT_UNIT] (nolock) Where Sno = '{0}')
                begin
	                INSERT INTO [dbo].[WEIGHT_UNIT]([Sno],[Type],[Weigh],[TotalWeigh],[Length],[Width],[Height],[Weigher],[Cdt],[Udt])
	                Select '{0}' , 'GROSS' , '{1}' , 0 , {2} , {3} , {4} , '{5}' , getdate() , getdate()
                end
                else
                begin
	                Update [WEIGHT_UNIT]
	                Set [Sno] = '{0}' 
                        ,[Type] = 'GROSS'
                        ,[Weigh] = '{1}'
                        ,[TotalWeigh] = 0
                        ,[Length] = {2}
                        ,[Width] = {3}
                        ,[Height] = {4}
                        ,[Weigher] = '{5}'
                        ,[Udt] = getdate()
	                Where Sno = '{0}' 
                end"
                , inputData.SN
                , inputData.GrossWeight.ToString()
                , inputData.Length.ToString()
                , inputData.Width.ToString()
                , inputData.Height.ToString()
                , inputData.Weigher
                );

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                WriteFailFile(inputData.Weigher, @"D:\FileStock\WeighFile", ex.Message + "\r\n" + ex.StackTrace);
            }
            
        }
        private static void InsertWeightOrighData(WeighResultEntity inputData)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CTO_SQL_ConnectionString"].ConnectionString;
            var sql = string.Format(@"
                if not exists (Select top 1 Sno from [WEIGHT_UNIT_ORIG] (nolock) Where Sno = '{0}')
                begin
	                INSERT INTO [dbo].[WEIGHT_UNIT_ORIG]([Sno],[Type],[Weigh],[TotalWeigh],[Length],[Width],[Height],[Weigher],[Cdt],[Udt])
	                Select '{0}' , 'GROSS' , '{1}' , 0 , {2} , {3} , {4} , '{5}' , getdate() , getdate()
                end
                else
                begin
	                Update [WEIGHT_UNIT_ORIG]
	                Set [Sno] = '{0}' 
                        ,[Type] = 'GROSS'
                        ,[Weigh] = '{1}'
                        ,[TotalWeigh] = 0
                        ,[Length] = {2}
                        ,[Width] = {3}
                        ,[Height] = {4}
                        ,[Weigher] = '{5}'
                        ,[Udt] = getdate()
	                Where Sno = '{0}' 
                end"
            , inputData.SN
            , inputData.GrossWeight.ToString()
            , inputData.Length.ToString()
            , inputData.Width.ToString()
            , inputData.Height.ToString()
            , inputData.Weigher
            );

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
