using System;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EasyModbus;
using System.Net.Sockets;
using System.Net;

namespace WindowsService1
{
    partial class WeightCollectService_TCP : ServiceBase
    {
        private Timer MyTimer;
        private ModbusClient modbusClientCenter = null;
        //private ModbusTcpNet busTcpClient = null;
        //private OperateResult connect = null;

        public string KeepSno { get; set; }

        private class WeightDifConstrants
        {
            public const string KeyErrorMessage = "Weight Collect Process Got Issue.";
            public const string TcpListener = "TcpListener_";
            public const string ModbusTCPClient = "ModbusTCPClient_";
            public const string Exception = "Exception_";
            public const string IptWeightService = "IPT Weight Service";
            public const string PASS = "PASS";
            public const string FAIL = "FAIL";
            public const string MONITOR = "MONITOR";

        }
        private class FileStockTypes
        {
            public const string Tcp = "Tcp";
            public const string ModbusTCP = "ModbusTCP";

        }
        public class FileStockCategories
        {
            public const string WeighFile = "WeighFile";
            public const string Weigh = "Weigh";
        }
        private class FileStockEntity
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Site { get; set; }
            public string Category { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public string Domain { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string RemoteDir { get; set; }
            public bool Active { get; set; }
            public string Remark { get; set; }
            public string Creator { get; set; }
            public string Editor { get; set; }
            public DateTime Cdt { get; set; }
            public DateTime Udt { get; set; }
            public string EKey { get; set; }
        }
        public class WeighResultEntity
        {
            public WeighResultEntity()
            {
                Weight = 0;
                Error = "";
                Overload = "N";
            }

            /// <summary>
            /// 
            /// </summary>
            public double Weight { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Error { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Overload { get; set; }

            public string RawData { get; set; }
            public string SN { get; set; }
            public double GrossWeight { get; set; }
            public double NetWeight { get; set; }
            public int Length { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public WeightCollectService_TCP()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            this.KeepSno = "";
            var insterval = 3;
            MyTimer = new Timer();
            MyTimer.Elapsed += new ElapsedEventHandler(TCP_Elapsed);
            MyTimer.Interval = insterval * 1000;
            MyTimer.Start();
        }
        private void TCP_Elapsed(object sender, ElapsedEventArgs e)
        {
            //準備Log 的位置
            var weightFileStocks = this.GetFileStockDatas(FileStockCategories.WeighFile);
            var weightFileStock = weightFileStocks.Where(x => x.Type == "UNC" && x.Site == "Local" && x.Active == true).FirstOrDefault();
            var type = System.Configuration.ConfigurationSettings.AppSettings["Type"].ToString();
            var host = System.Configuration.ConfigurationSettings.AppSettings["Host"].ToString();
            var port = System.Configuration.ConfigurationSettings.AppSettings["Port"].ToString();

            //持續處理不要停
            try
            {
                var sourceFileStock = new FileStockEntity { Type = type, Host = host, Port = Convert.ToInt32(port) };
                this.GetWeighResultFromTCPClient(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock.RemoteDir);
            }
            catch (Exception ex)
            {
                var errorMessage = "";
                errorMessage = ex.Message + Environment.NewLine + ex.StackTrace;

                var workingPath = weightFileStock.RemoteDir;
                if (Directory.Exists(workingPath) == false)
                {
                    Directory.CreateDirectory(workingPath);
                }

                //this.KeepSno = "";
                this.WriteFailFile(host, workingPath, errorMessage);
                System.Threading.Thread.Sleep(5 * 1000);
            }
        }
        private void GetWeighResultFromTCPClient(string weightIp, string weightPort, string remoteDir)
        {            
            try
            {
                //開始連線
                //command : B 读毛重 , C	读皮重 , D	读净重
                var dataIp = weightIp;
                var dataPort = Convert.ToInt32(weightPort);
                var message = "";
                var content = "";

                //取直取四次 , 3秒沒回副就跳掉
                IPAddress ip = IPAddress.Parse(dataIp);//服务器端ip           
                var myListener = new TcpListener(ip, dataPort);//创建TcpListener实例   

                myListener.Start();//start
                TcpClient newClient = new TcpClient();
                Task clientTask = Task.Run(() => {
                    newClient = myListener.AcceptTcpClient();
                });

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
        }
        private void WriteFailFile(string host, string workingPath, string errorMessage)
        {
            var fileName = this.GetFailFileName(WeightDifConstrants.Exception, WeightDifConstrants.FAIL, host);
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
        private string GetFailFileName(string title, string result, string host)
        {
            var fileName = title + DateTime.Now.ToString("yyyy-MM-dd HH") + "@" + result + "@" + host + ".txt";
            return fileName;
        }
        private List<FileStockEntity> GetFileStockDatas(string category)
        {
            var resullts = new List<FileStockEntity>();
            var connectionString = ConfigurationManager.ConnectionStrings["CTO_SQL_ConnectionString"].ConnectionString;
            string queryString = string.Format("Select * from FILE_STOCK (nolock) Where Category in ('{0}')", category);
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


        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
