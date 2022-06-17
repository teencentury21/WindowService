using Inventec.FIS.Model.Domiain.Constrants;
using Inventec.FIS.Model.Domiain.Entity;
using Inventec.FIS.Model.Domiain.Services;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace WeightCollecterTCPWindowsServices
{
    public partial class WeightCollectTCPService : ServiceBase
    {
        public WeightCollectTCPService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            //prepare log
            var weightFileStocks = WeightCollectService.GetFileStockDatas(FileStockCategories.WeighFile.Value);
            var weightFileStock = weightFileStocks.Where(x => x.Type == "UNC" && x.Site == "Local" && x.Active == true).FirstOrDefault();
            var type = ConfigurationSettings.AppSettings["Type"].ToString();
            var host = ConfigurationSettings.AppSettings["Host"].ToString();
            var port = ConfigurationSettings.AppSettings["Port"].ToString();

            // keep job process
            try
            {
                var sourceFileStock = new FileStockEntity { Type = type, Host = host, Port = Convert.ToInt32(port) };

                if (type == FileStockTypes.TCP.Value)
                {
                    //叉車 Weight Machine 資料收集                        
                    //連線對應秤重機 取得重量資訊 並上傳到資料庫 後放檔案
                    this.GetWeighResultFromTcpListener(host, port, weightFileStock);
                }
                else if (type == FileStockTypes.ModbusTCP.Value)
                {
                    //Option Weight Machine 資料收集                                                
                    //連線對應秤重機 取得重量資訊 並上傳到資料庫 後放檔案, 詳細在WeightCollecterWindowsServices 專案    
                    //this.GetWeighResultFromModbusTCPClient(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock.RemoteDir);
                    throw new Exception(string.Format("Not Support Type:{0}. Please use project WeightCollecterWindowsServices", sourceFileStock.Type));
                }
                else
                {
                    throw new Exception(string.Format("Not Support Type:{0}", sourceFileStock.Type));
                }
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
                WeightCollectService.WriteFailFile(host, workingPath, errorMessage);
                System.Threading.Thread.Sleep(5 * 1000);
            }
        }

        private void GetWeighResultFromTcpListener(string weightIp, string weightPort, FileStockEntity weightFileStock)
        {
            SimpleTcpServer server = new SimpleTcpServer();
            server.Delimiter = 0x13;// enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += ServerDataReceived;

            IPAddress ip = IPAddress.Parse(weightIp);
            var port = Convert.ToInt32(weightPort);
            // start listen port
            server.Start(ip, port);
        }

        private void ServerDataReceived(object sender, Message e)
        {
            var weightFileStocks = WeightCollectService.GetFileStockDatas(FileStockCategories.WeighFile.Value);
            var workingPath = weightFileStocks.Where(x => x.Type == "UNC" && x.Site == "Local" && x.Active == true).FirstOrDefault().RemoteDir;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("\r\n{0} => DataReceived \"{1}\" start process..."
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                , e.MessageString));

            //準備Log 的位置

            sb.AppendLine(string.Format("Start collect source IP at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            var host = e.TcpClient.Client.RemoteEndPoint.ToString().Split(':')[0];
            sb.AppendLine(string.Format("Source IP {0}", host));

            try
            {
                var fileName = this.GetFileName(WeightDifConstrants.TcpClient.Value, WeightDifConstrants.PASS.Value,
                    host, "");

                var txtfile = Path.Combine(workingPath, fileName);

                var input = e.MessageString.Split(',');
                var now = DateTime.Now;

                if (input.Length != 5)
                {
                    sb.AppendLine(string.Format("Invalid input data '{0}', stop collection process.", e.MessageString));
                    File.AppendAllText(txtfile, sb.ToString(), Encoding.Unicode);
                    return;
                }

                var insertData = new WeighResultEntity();
                // analysis input
                insertData.SN = input[3].Trim();                
                insertData.GrossWeight = Convert.ToDouble(Regex.Match(input[4].Trim(), "[+-]?([0-9]*[.])?[0-9]+").Value);
                insertData.Length = 0;
                insertData.Width = 0;
                insertData.Height = 0;
                insertData.Weigher = host;
                // save to database
                sb.AppendLine(string.Format("SN: '{0}', GrossWeight: {1}, start insert into database", insertData.SN, insertData.GrossWeight));
                File.AppendAllText(txtfile, sb.ToString(), Encoding.Unicode);
                sb.Clear();
                WeightCollectService.InsertWeightData(insertData);
                //寫入文字檔備份                        
                
                sb.AppendLine(string.Format("Start backup {2} weight data to {0} at {1}", txtfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), insertData.SN));
                File.AppendAllText(txtfile, sb.ToString(), Encoding.Unicode);
                File.AppendAllText(txtfile, string.Format("End backup {2} weight data to {0} at {1} \r\n", txtfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), insertData.SN), Encoding.Unicode);
            }
            catch (Exception ex)
            {
                var errorMessage = "";
                errorMessage = ex.Message + Environment.NewLine + ex.StackTrace;

                //var workingPath = weightFileStock.RemoteDir;
                if (Directory.Exists(workingPath) == false)
                {
                    Directory.CreateDirectory(workingPath);
                }

                //this.KeepSno = "";
                WeightCollectService.WriteFailFile(host, workingPath, errorMessage);                
            }
        }

        private string GetFileName(string title, string result, string host, string type)
        {
            var fileName = string.Format("{0}{4} {1}@{2}@{3}.txt", title, result,
                DateTime.Now.ToString("yyyy-MM-dd HH"), host, type);
            return fileName;
        }

        protected override void OnStop()
        {
        }
    }
}
