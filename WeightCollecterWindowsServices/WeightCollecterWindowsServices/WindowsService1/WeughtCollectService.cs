using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net.Sockets;
using System.Net;
using Inventec.FIS.Web.Tools;
using System.Configuration;
using System.Data.SqlClient;
//using HslCommunication.ModBus;
//using HslCommunication;
using Inventec.FIS.Utilities.Data;
using System.Globalization;
using EasyModbus;

namespace WindowsService1
{    
    public partial class WeughtCollectService : ServiceBase
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

        public WeughtCollectService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.KeepSno = "";
            var insterval = 3;            
            MyTimer = new Timer();
            MyTimer.Elapsed += new ElapsedEventHandler(MyTimer_Elapsed);
            MyTimer.Interval = insterval * 1000;
            MyTimer.Start();
        }


        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
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

                if (sourceFileStock.Type == FileStockTypes.Tcp)
                {
                    //叉車 Weight Machine 資料收集                        
                    //連線對應秤重機 取得重量資訊 並上傳到資料庫 後放檔案
                    //Robin 還沒寫
                    //this.GetWeighResultFromTcpListener(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock);

                }
                else if (sourceFileStock.Type == FileStockTypes.ModbusTCP)
                {
                    //Option Weight Machine 資料收集                                                
                    //連線對應秤重機 取得重量資訊 並上傳到資料庫 後放檔案    
                    this.GetWeighResultFromModbusTCPClient(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock.RemoteDir);
                }
                else
                {
                    throw new Exception("Not Support Type:" + sourceFileStock.Type);
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
                this.WriteFailFile(host, workingPath, errorMessage);
                System.Threading.Thread.Sleep(5 * 1000);
            }

        }

        private string GetFileName(string title, string result, string host)
        {
            var fileName = title + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + "@" + result + "@" + host + ".txt";
            return fileName;
        }

        private string GetFailFileName(string title, string result, string host)
        {
            var fileName = title + DateTime.Now.ToString("yyyy-MM-dd HH") + "@" + result + "@" + host + ".txt";
            return fileName;
        }


        private void GetWeighResultFromModbusTCPClient(string weightIp, string weightPort, string remoteDir)
        {
            var optionSnAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionSnAddress"].ToString();
            var optionSnLength = System.Configuration.ConfigurationSettings.AppSettings["OptionSnLength"].ToString();
            var optionWeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWeightAddress"].ToString();
            var optionLengthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionLengthAddress"].ToString();
            var optionWidthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWidthAddress"].ToString();
            var optionHeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionHeightAddress"].ToString();
            var accuracy = System.Configuration.ConfigurationSettings.AppSettings["Accuracy"].ToString();
            var sendSignalAdress = System.Configuration.ConfigurationSettings.AppSettings["SendSignalAdress"].ToString();
            var sendSignalValue = System.Configuration.ConfigurationSettings.AppSettings["SendSignalValue"].ToString();
            var monitorMode = System.Configuration.ConfigurationSettings.AppSettings["MonitorMode"].ToString();

            
            // 连接
            if (!int.TryParse(weightPort, out int port))
            {
                throw new Exception(string.Format("Port setting is wrong."));
            }

            if (!byte.TryParse("0", out byte station))
            {
                throw new Exception(string.Format("Station setting is wrong."));
            }
            
            var matchingCount = 0;
            var matchingHexString = "";
            var monitorWeightProcessLogList = new List<string>();


            try
            {                
                var keepProcess = true;
                while (keepProcess)
                {
                    if (matchingCount == 0)
                    {
                        monitorWeightProcessLogList.Add(String.Format("開始 確保收到的重量數據已經穩定  at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }

                    var sb = new StringBuilder();
                    sb.AppendLine(String.Format("開始 重量數據分析上傳 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    sb.AppendLine(String.Format("開始 連線秤重機 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    var modbusClient = new ModbusClient(weightIp, port);    //Ip-Address and Port of Modbus-TCP-Server
                    modbusClient.IPAddress = weightIp;
                    modbusClient.Port = port;
                    modbusClient.UnitIdentifier = station;                    
                    modbusClient.Connect();
                    
                    var snResult = "";
                    float weightResult = 0;
                    int widthResult = 0;
                    int heightResult = 0;
                    int lengthResult = 0;
                    var hexString = "";

                    sb.AppendLine(String.Format("開始收集 秤重機 裡的重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    //讀取資料出來
                    //hexString = this.BulkReadRenderResult(busTcpClient, "0", "20");
                    if (modbusClient.Connected)
                    {
                        int[] serverResponse = modbusClient.ReadHoldingRegisters(0, 20);
                        for (int i = 0; i < serverResponse.Length; i++)
                        {
                            var indexString = serverResponse[i].ToString("X4");
                            if (indexString.Length > 4 && indexString.Substring(0, 4) == "FFFF")
                            {
                                indexString = indexString.Substring(4, indexString.Length - 4);
                            }

                            hexString = hexString + indexString;                            
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Connect Lost or fail."));
                    }

                    //完成一次就斷掉重連
                    modbusClient.Disconnect();
                    //modbusClient = null;

                    sb.AppendLine(String.Format("結束收集 秤重機 裡的重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    sb.AppendLine(String.Format("開始分析 取得 的 重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    //確定 hexString 的內容 穩定
                    if (matchingHexString != hexString)
                    {
                        matchingHexString = hexString;
                        matchingCount = 0;
                    }
                    else
                    {
                        matchingCount++;
                    }

                    if (matchingCount < Convert.ToInt32(accuracy))
                    {
                        //如果 HexString 相同 的次數 沒有超過 規定的數量 不處理
                        continue;
                    }
                    else
                    {
                        //如果 HexString 相同 的次數 超過 規定的數量 更新數據                        
                        matchingHexString = "";
                        matchingCount = 0;

                        monitorWeightProcessLogList.Add(String.Format("收到的重量數據已經穩定   at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        monitorWeightProcessLogList.Add(String.Format("開始分析 收到的重量數據 at {0} ,{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), matchingHexString));

                    }

                    hexString = hexString.Substring(0, 64);
                    var sortHexStringList = new List<string>();
                    for (int i = 0; i < hexString.Length; i = i + 4)
                    {
                        var cutString = hexString.Substring(i, 4);
                        sortHexStringList.Add(cutString);
                    }

                    //  ==> SN      : Address 0 , Length: 7        r.string
                    var index = Convert.ToInt32(optionSnAddress);
                    var snTureHexString = "";
                    for (int i = index; i < index + 8; i++)
                    {
                        var tempString = sortHexStringList[i].Substring(2, 2) + sortHexStringList[i].Substring(0, 2);
                        snTureHexString = snTureHexString + tempString;
                    }
                    byte[] data = FromHex(snTureHexString);
                    snResult = Encoding.ASCII.GetString(data);

                    //重量 sortHexStringList[9] + sortHexStringList[8]
                    //Float - Mid-Little Endian (CDAB) 排列方式
                    var weightInt = Convert.ToInt32(optionWeightAddress) - 1;
                    var indexHestString = sortHexStringList[weightInt + 1] + sortHexStringList[weightInt];
                    string HexRep = indexHestString;
                    Int32 IntRep = Int32.Parse(HexRep, NumberStyles.AllowHexSpecifier);
                    weightResult = BitConverter.ToSingle(BitConverter.GetBytes(IntRep), 0);


                    //長 sortHexStringList[10]
                    lengthResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionLengthAddress)], 16);

                    //寬 sortHexStringList[12]
                    widthResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionWidthAddress)], 16);

                    //高 sortHexStringList[14]
                    heightResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionHeightAddress)], 16);


                    snResult = snResult.Replace("\0", "");
                    snResult = snResult.Trim();

                    sb.AppendLine(String.Format("結束分析 取得 的 重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    monitorWeightProcessLogList.Add(String.Format("重量數據已經分析完成 序號 :  {0} at {1}", snResult, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    if (this.KeepSno != snResult && string.IsNullOrEmpty(snResult) == false)
                    {
                        //當保留的序號與 機器的不一樣代表 機器Input 了新的序號出來就開始實行 紀錄與上傳作業
                        //紀錄文字檔內容
                        var textFileContent = "";
                        textFileContent = string.Format("SN:{0}~Weight:{1}~Length:{2}~Width:{3}~Height:{4}~HexString:{5}"
                            , snResult, weightResult.ToString(), lengthResult.ToString(), widthResult.ToString(), heightResult.ToString(), hexString);
                        var workingPath = remoteDir;
                        var fileName = this.GetFileName(WeightDifConstrants.ModbusTCPClient, WeightDifConstrants.PASS, weightIp);

                        //將數據上傳到資料庫                        
                        var insertData = new WeighResultEntity();
                        insertData.RawData = textFileContent;
                        insertData.SN = snResult;
                        insertData.GrossWeight = weightResult;
                        insertData.Length = lengthResult;
                        insertData.Width = widthResult;
                        insertData.Height = heightResult;

                        if (insertData.GrossWeight >=9999 || insertData.Length >= 9999 || insertData.Width >= 9999 || insertData.Height >= 9999)
                        {
                            //对于 重量/长/宽/高>=9999的值 都当做非法数据忽略掉，重新讀數據 
                            continue;
                        }
                        else if (string.IsNullOrEmpty(insertData.SN) || insertData.GrossWeight <= 0 || insertData.Length <= 0 || insertData.Width <= 0 || insertData.Height <= 0)
                        {
                            textFileContent = "數據有問題 不上傳到資料庫" + "," + textFileContent;
                            fileName = this.GetFileName(WeightDifConstrants.ModbusTCPClient, WeightDifConstrants.FAIL, weightIp);
                        }
                        else
                        {
                            sb.AppendLine(String.Format("開始 上傳 重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                            this.InsertWeightData(insertData, weightIp);
                            sb.AppendLine(String.Format("結束 上傳 重量數據 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        }

                        //發送完成訊息給 秤重機
                        sb.AppendLine(String.Format("結束 發送完成訊息給 秤重機 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        modbusClient.Connect();
                        var serverResponseByte = new List<byte>();
                        try
                        {
                            var sendIntDataint = int.Parse(sendSignalValue);
                            var sendAddress = int.Parse(sendSignalAdress);

                            int[] registersToSend = new int[1];
                            registersToSend[0] = int.Parse(sendSignalValue);
                            modbusClient.WriteMultipleRegisters(sendAddress, registersToSend);
                        }
                        catch (Exception ex)
                        {
                            modbusClient.Disconnect();
                            throw ex;                            
                        }                        
                        sb.AppendLine(String.Format("結束 發送完成訊息給 秤重機 at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));


                        //寫入文字檔備份                        
                        var txtfile = Path.Combine(workingPath, fileName);
                        sb.AppendLine(String.Format("開始 備份 重量數據 到{0} at {1}", txtfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        File.WriteAllText(txtfile, textFileContent, Encoding.Unicode);
                        sb.AppendLine(String.Format("結束 備份 重量數據 到{0} at {1}", txtfile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                        

                        //將分析結果附到 結果檔案上
                        textFileContent = textFileContent + Environment.NewLine + sb.ToString();
                        File.WriteAllText(txtfile, textFileContent, Encoding.Unicode);

                        this.KeepSno = snResult;
                        monitorWeightProcessLogList.Add(String.Format("重量數據已經上傳完成 序號 : {0} at {1}", snResult, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else
                    {
                        //當保留的序號與機器的一樣代表 尚未有新的序號進來就不做任何處理
                        monitorWeightProcessLogList.Add(String.Format("檢查 該 重量數據 不需要上傳 序號 : {0} at {1}", snResult, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }

                    if (monitorWeightProcessLogList.Count > 0 && monitorMode == "On")
                    {
                        var workingPath = remoteDir;
                        this.WriteMonitorFile(weightIp, workingPath, monitorWeightProcessLogList);
                        monitorWeightProcessLogList.Clear();
                    }
                    else
                    {
                        monitorWeightProcessLogList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {                
                throw new Exception(string.Format("{0} {1}" , ex.Message, ex.StackTrace));
            }              
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

        private void InsertWeightData(WeighResultEntity inputData, string weightIp)
        {
            if (inputData.SN.StartsWith("3S"))
            {
                InsertWeightOrighData(inputData, weightIp);
                inputData.SN = inputData.SN.Substring(2, inputData.SN.Length-2);
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
            , weightIp
            );

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void InsertWeightOrighData(WeighResultEntity inputData, string weightIp)
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
            , weightIp
            );

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
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

        private void WriteMonitorFile(string host, string workingPath, List<string> messageList)
        {
            var fileName = this.GetFailFileName(WeightDifConstrants.Exception, WeightDifConstrants.MONITOR, host);
            var logTime = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + Environment.NewLine;
            var txtfile = Path.Combine(workingPath, fileName);
            var sb = new StringBuilder();
            foreach (var messageItem in messageList)
            {
                sb.AppendLine(messageItem);                
            }

            var message = sb.ToString();

            if (File.Exists(txtfile))
            {
                var oldMessage = File.ReadAllText(txtfile);
                message = logTime + message + Environment.NewLine + Environment.NewLine + oldMessage;
                File.WriteAllText(txtfile, message, Encoding.Unicode);
            }
            else
            {
                message = logTime + message + Environment.NewLine + Environment.NewLine;
                File.WriteAllText(txtfile, message, Encoding.Unicode);
            }
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private void SendOkSignal(ModbusClient modbusClient, string weightPort)
        {
            var sendSignalAdress = System.Configuration.ConfigurationSettings.AppSettings["SendSignalAdress"].ToString();
            var sendSignalValue = System.Configuration.ConfigurationSettings.AppSettings["SendSignalValue"].ToString();
       
            var serverResponseByte = new List<byte>();            
            try
            {
                var sendIntDataint = int.Parse(sendSignalValue);
                var sendAddress = int.Parse(sendSignalAdress);

                int[] registersToSend = new int[1];
                registersToSend[0] = int.Parse(sendSignalValue);
                modbusClient.WriteMultipleRegisters(sendAddress, registersToSend);
            }
            catch (Exception exc)
            {
                //modbusClient.Disconnect();
                //MessageBox.Show(exc.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //modbusClient.Disconnect();


        }



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        protected override void OnStop()
        {
        }

        private void GetWeighResultFromTcpListener(string weightIp, string weightPort, FileStockEntity weightFileStock)
        {
            var dataIp = weightIp;
            var dataPort = weightPort;

            //開始連線
            //command : B 读毛重 , C	读皮重 , D	读净重
            //var ip = dataIp;
            var port = Convert.ToInt32(dataPort);
            var message = "";
            var content = "";
            

            //取直取四次 , 3秒沒回副就跳掉
            IPAddress ip = IPAddress.Parse(dataIp);//服务器端ip           
            var myListener = new TcpListener(ip, port);//创建TcpListener实例                        

            myListener.Start();//start
            TcpClient newClient = new TcpClient();
            Task clientTask = Task.Run(() => {
                newClient = myListener.AcceptTcpClient();
            });

            TimeSpan ts = TimeSpan.FromMilliseconds(5000);
            if (!clientTask.Wait(ts))
            {
                newClient.Close();
                myListener.Stop();
                throw new Exception(string.Format("Client {0} timeout.", weightIp));
            }

            using (newClient)
            {                
                try
                {
                    for (var i = 0; i <= 4; i++)
                    {                        
                        NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流                        
                        var br = new BinaryReader(clientStream);                        
                        string receive = null;                        
                        receive = br.ReadString();//读取                        
                        content = content + "," + receive;
                    }
                }
                catch (Exception ex)
                {
                    newClient.Close();
                    myListener.Stop();
                    message = ex.Message;
                }
                finally
                {
                    myListener.Stop();
                    newClient.Close();
                }
            }

            newClient.Close();
            myListener.Stop();

            var textFileContent = "";
            if (string.IsNullOrEmpty(message) == false)
            {                
                throw new Exception(message);
            }
            else
            {
                textFileContent = content;
            }

            var workingPath = weightFileStock.RemoteDir;
            //var fileName = WeightDifConstrants.TcpListener + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + "@" + weightIp + ".txt";
            var fileName = this.GetFileName(WeightDifConstrants.TcpListener, WeightDifConstrants.PASS, weightIp);
            var txtfile = Path.Combine(workingPath, fileName);
            
            File.WriteAllText(txtfile, textFileContent, Encoding.ASCII);
        }

        

        
        
        

    }
}
