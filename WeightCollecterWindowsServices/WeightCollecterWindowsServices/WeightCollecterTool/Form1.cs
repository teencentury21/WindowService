using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Configuration;
using HslCommunication.ModBus;
using HslCommunication;
using System.Threading;
using System.Globalization;
using EasyModbus;

namespace WeightCollecterTool
{
    public partial class Form1 : Form
    {
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

        private ModbusTcpNet busTcpClient = null;
        private class WeightDifConstrants
        {
            public const string KeyErrorMessage = "Weight Collect Process Got Issue.";
            public const string TcpListener = "TcpListener_";
            public const string ModbusTCPClient = "ModbusTCPClient_";
            public const string IptWeightService = "IPT Weight Service";
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


        public Form1()
        {
            InitializeComponent();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            AddressListBox.Items.Clear();
            var address = ipTextBox.Text;
            var port = Convert.ToInt32(PortTextBox.Text);
            int startAddress = Convert.ToInt32(StartAddTextBox.Text);
            int length = Convert.ToInt32(LengthTextBox.Text);
            if (!byte.TryParse(StationTextBox.Text, out byte station))
            {
                MessageBox.Show("Station input is wrong！");
                return;
            }
            
            ModbusClient modbusClient = new ModbusClient(address, port);    //Ip-Address and Port of Modbus-TCP-Server
            modbusClient.IPAddress = address;
            modbusClient.Port = port;
            modbusClient.UnitIdentifier = station;
            modbusClient.Connect();

            var serverResponseByte = new List<byte>();
            var hexString = "";
            try
            {                
                int[] serverResponse = modbusClient.ReadHoldingRegisters(startAddress, length);
                for (int i = 0; i < serverResponse.Length; i++)
                {
                    var indexString = serverResponse[i].ToString("X4");
                    if (indexString.Length > 4 && indexString.Substring(0, 4) == "FFFF")
                    {
                        indexString = indexString.Substring(4, indexString.Length - 4);
                    }

                    hexString = hexString + indexString;
                    AddressListBox.Items.Add(i.ToString() + " : " + serverResponse[i].ToString() + " : " + serverResponse[i].ToString("X4"));
                }
            }
            catch (Exception exc)
            {
                modbusClient.Disconnect();
                MessageBox.Show(exc.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            modbusClient.Disconnect();
           

            var optionSnAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionSnAddress"].ToString();
            var optionSnLength = System.Configuration.ConfigurationSettings.AppSettings["OptionSnLength"].ToString();
            var optionWeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWeightAddress"].ToString();
            var optionLengthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionLengthAddress"].ToString();
            var optionWidthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWidthAddress"].ToString();
            var optionHeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionHeightAddress"].ToString();
            

            var snResult = "";
            float weightResult = 0;
            int widthResult = 0;
            int heightResult = 0;
            int lengthResult = 0;            

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


            //message = message + "SN:" + snResult + Environment.NewLine;
            //message = message + "重量:" + weightResult.ToString() + Environment.NewLine;
            //message = message + "長:" + lengthResult.ToString() + Environment.NewLine;
            //message = message + "寬:" + widthResult.ToString() + Environment.NewLine;
            //message = message + "高:" + heightResult.ToString() + Environment.NewLine;
            
            snResult = snResult.Replace("\0", "");
            snResult = snResult.Trim();


            var textFileContent = "";
            textFileContent = string.Format("SN:{0}~Weight:{1}~Length:{2}~Width:{3}~Height:{4}~HexString:{5}"
                , snResult, weightResult.ToString(), lengthResult.ToString(), widthResult.ToString(), heightResult.ToString(), hexString);

            //MessageBox.Show(textFileContent);
            //this.textBox1.Text = textFileContent;
            //MessageBox.Show("SN:" + snResult);
            //MessageBox.Show("重量:" + weightResult.ToString());
            //MessageBox.Show("長:" + lengthResult.ToString());
            //MessageBox.Show("寬:" + widthResult.ToString());
            //MessageBox.Show("高:" + heightResult.ToString());            

            this.HexStringTextBox.Text = hexString;
            this.SnTextBox.Text = snResult;
            this.WeightTextBox.Text = weightResult.ToString();
            this.LenTextBox.Text = lengthResult.ToString();
            this.WidthTextBox.Text = widthResult.ToString();
            this.HeightTextBox.Text = heightResult.ToString();
            
        }



        /// <summary>
        /// byte[] 轉 Hex String
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>Hex String</returns>
        protected string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder str = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    str.Append(bytes[i].ToString("X2"));
                }
                hexString = str.ToString();
            }
            return hexString;
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





        private void button1_Click(object sender, EventArgs e)
        {
            var insertData = new WeighResultEntity();
            insertData.RawData = "RobinTest";
            insertData.SN = "RobinTest";
            insertData.GrossWeight = 1;
            insertData.Length = 2;
            insertData.Width = 3;
            insertData.Height = 4;

            var weightIp = "Robin";

            this.InsertWeightData(insertData, weightIp);

        }


        private void InsertWeightData(WeighResultEntity inputData, string weightIp)
        {
            var resullts = new List<FileStockEntity>();
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


        private void TestFuntion()
        {
            try
            {
                var weightFileStocks = this.GetFileStockDatas(FileStockCategories.WeighFile);
                var weightFileStock = weightFileStocks.Where(x => x.Type == "UNC" && x.Site == "Local" && x.Active == true).FirstOrDefault();
                var sourceFileStocks = this.GetFileStockDatas(FileStockCategories.Weigh);
                sourceFileStocks = sourceFileStocks.Where(x => x.Remark == WeightDifConstrants.IptWeightService && x.Active == true).ToList();


                var workingPath = Directory.GetCurrentDirectory();
                workingPath = Path.Combine(workingPath, "Log");
                if (Directory.Exists(workingPath) == false)
                {
                    Directory.CreateDirectory(workingPath);
                }

                //var weightIp = "192.168.10.50";
                //var errorMessage = "Robin Test";
                //var fileName = WeightDifConstrants.TcpListener + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + "@" + weightIp + ".txt";                
                //var txtfile = Path.Combine(workingPath, fileName);
                //File.WriteAllText(txtfile, errorMessage, Encoding.Unicode);


                foreach (var sourceFileStock in sourceFileStocks)
                {
                    if (sourceFileStock.Type == FileStockTypes.Tcp)
                    {
                        //叉車 Weight Machine 資料收集
                        //找出需要處理的秤重機
                        //連線對應秤重機 取得重量資訊 並轉成文字檔 放到特定 目錄
                        this.GetWeighResultFromTcpListener(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock);
                    }
                    else if (sourceFileStock.Type == FileStockTypes.ModbusTCP)
                    {
                        //Option Weight Machine 資料收集
                        this.GetWeighResultFromModbusTCPClient(sourceFileStock.Host, sourceFileStock.Port.ToString(), weightFileStock.RemoteDir);
                    }
                    else
                    {
                        Console.WriteLine("Not Support Type:" + sourceFileStock.Type);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "";
                errorMessage = ex.Message + Environment.NewLine + ex.StackTrace;

                var workingPath = Directory.GetCurrentDirectory();
                workingPath = Path.Combine(workingPath, "Log");

                if (Directory.Exists(workingPath) == false)
                {
                    Directory.CreateDirectory(workingPath);
                }

                var fileName = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".txt";
                var txtfile = Path.Combine(workingPath, fileName);
                File.WriteAllText(txtfile, errorMessage, Encoding.Unicode);
            }
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

            this.WriteLog("開始連線");

            //取直取四次 , 3秒沒回副就跳掉
            IPAddress ip = IPAddress.Parse(dataIp);//服务器端ip
            this.WriteLog("IPAddress ip = IPAddress.Parse(dataIp);//服务器端ip");
            var myListener = new TcpListener(ip, port);//创建TcpListener实例            
            this.WriteLog(" new TcpListener(ip, port);//创建TcpListener实例      ");

            myListener.Start();//start
            this.WriteLog("myListener.Start();//start");
            TcpClient newClient = new TcpClient();
            Task clientTask = Task.Run(() =>
            {
                newClient = myListener.AcceptTcpClient();
            });

            TimeSpan ts = TimeSpan.FromMilliseconds(5000);
            if (!clientTask.Wait(ts))
            {
                this.WriteLog("The timeout interval elapsed.");
                newClient.Close();
                myListener.Stop();
                throw new Exception(string.Format("Client {0} timeout.", weightIp));
            }


            using (newClient)
            {
                this.WriteLog("using (var newClient = myListener.AcceptTcpClient())");

                try
                {
                    for (var i = 0; i <= 4; i++)
                    {
                        this.WriteLog("NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流");
                        NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流
                        this.WriteLog("var br = new BinaryReader(clientStream);");
                        var br = new BinaryReader(clientStream);
                        this.WriteLog("string receive = null;");
                        string receive = null;
                        this.WriteLog("receive = br.ReadString();//读取");
                        receive = br.ReadString();//读取
                        this.WriteLog("content = content  + receive;");
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
                    newClient.Close();
                }
            }

            myListener.Stop();


            var textFileContent = "";
            if (string.IsNullOrEmpty(message) == false)
            {
                var sb = new StringBuilder();
                sb.AppendLine(WeightDifConstrants.KeyErrorMessage);
                sb.AppendLine(message);
                textFileContent = sb.ToString();
            }
            else
            {
                textFileContent = content;
            }

            var workingPath = weightFileStock.RemoteDir;
            var fileName = WeightDifConstrants.TcpListener + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".txt";
            var txtfile = Path.Combine(workingPath, fileName);

            File.WriteAllText(txtfile, textFileContent, Encoding.ASCII);

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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void GetWeighResultFromModbusTCPClient(string weightIp, string weightPort, string remoteDir)
        {
            var optionSnAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionSnAddress"].ToString();
            var optionSnLength = System.Configuration.ConfigurationSettings.AppSettings["OptionSnLength"].ToString();
            var optionWeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWeightAddress"].ToString();
            var optionLengthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionLengthAddress"].ToString();
            var optionWidthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWidthAddress"].ToString();
            var optionHeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionHeightAddress"].ToString();


            var address = "";
            var lenth = "";
            // 连接
            if (!int.TryParse(weightPort, out int port))
            {
                throw new Exception(string.Format("Port setting is wrong."));
            }

            if (!byte.TryParse("0", out byte station))
            {
                throw new Exception(string.Format("Station setting is wrong."));
            }

            busTcpClient = new ModbusTcpNet(weightIp, port, station);
            busTcpClient.ConnectClose();
            busTcpClient.DataFormat = HslCommunication.Core.DataFormat.ABCD;
            busTcpClient.IsStringReverse = true;

            try
            {
                var snResult = "";
                float weightResult = 0;
                int widthResult = 0;
                int heightResult = 0;
                int lengthResult = 0;
                var hexString = "";

                OperateResult connect = busTcpClient.ConnectServer();
                if (connect.IsSuccess)
                {

                    //  ==> SN      : Address 0 , Length: 7        r.string
                    address = optionSnAddress;
                    lenth = optionSnLength;
                    var result1 = busTcpClient.ReadString(address, ushort.Parse(lenth));
                    if (result1.IsSuccess)
                    {
                        snResult = result1.Content;
                    }
                    else
                    {
                        throw new Exception("SN 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result1.ToMessageShowString()}");
                    }

                    //outPutMessage = outPutMessage + "開始讀取 重量" + Environment.NewLine;
                    //  ==> 重量      : Address 9 , Length: 2        r - float
                    address = optionWeightAddress;
                    lenth = "2";
                    var result2 = busTcpClient.ReadFloat(address);
                    if (result2.IsSuccess)
                    {
                        weightResult = result2.Content;
                    }
                    else
                    {
                        throw new Exception("重量 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result2.ToMessageShowString()}");
                    }

                    hexString = this.BulkReadRenderResult(busTcpClient, "0", "20");
                    hexString = hexString.Substring(0, 64);
                    var sortHexStringList = new List<string>();
                    for (int i = 0; i < hexString.Length; i = i + 4)
                    {
                        var cutString = hexString.Substring(i, 4);
                        sortHexStringList.Add(cutString);
                    }

                    //長 sortHexStringList[10]
                    lengthResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionLengthAddress)], 16);

                    //寬 sortHexStringList[12]
                    widthResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionWidthAddress)], 16);

                    //高 sortHexStringList[14]
                    heightResult = Convert.ToInt32(sortHexStringList[Convert.ToInt32(optionHeightAddress)], 16);
                }

                var textFileContent = "";
                textFileContent = string.Format("SN:{0}~Weight:{1}~Length:{2}~Width:{3}~Height:{4}~HexString:{5}"
                    , snResult, weightResult.ToString(), lengthResult.ToString(), widthResult.ToString(), heightResult.ToString(), hexString);
                busTcpClient.ConnectClose();
                var workingPath = remoteDir;
                var fileName = "ModbusTCPClient" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + "@" + weightIp + ".txt";
                var txtfile = Path.Combine(workingPath, fileName);

                File.WriteAllText(txtfile, textFileContent, Encoding.ASCII);


            }
            catch (Exception ex)
            {
                busTcpClient.ConnectClose();
                throw ex;
            }
        }
        public string BulkReadRenderResult(HslCommunication.Core.IReadWriteNet readWrite, string address, string lenth)
        {
            try
            {
                OperateResult<byte[]> read = readWrite.Read(address, ushort.Parse(lenth));
                if (read.IsSuccess)
                {
                    return HslCommunication.BasicFramework.SoftBasic.ByteToHexString(read.Content);
                }
                else
                {
                    throw new Exception("Read Failed：" + read.ToMessageShowString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Read Failed：" + ex.Message);
            }
        }

        private void WriteLog(string inputMessage)
        {
            var workingPath = Directory.GetCurrentDirectory();
            workingPath = Path.Combine(workingPath, "Log");

            if (Directory.Exists(workingPath) == false)
            {
                Directory.CreateDirectory(workingPath);
            }

            var fileName = DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss ") + ".txt";
            var txtfile = Path.Combine(workingPath, fileName);
            File.WriteAllText(txtfile, inputMessage, Encoding.Unicode);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var optionSnAddress = "0";
            var optionSnLength = "7";
            var optionWeightAddress = "9";
            var optionLengthAddress = "10";
            var optionWidthAddress = "12";
            var optionHeightAddress = "14";

            var snResult = "";
            float weightResult = 0;
            int widthResult = 0;
            int heightResult = 0;
            int lengthResult = 0;
            var hexString = "";


            //hexString = this.BulkReadRenderResult(busTcpClient, "0", "20");
            hexString = HexStringTextBox.Text;

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

        private void label1_Click(object sender, EventArgs e)
        {

        }



        public string BulkReadRenderResult()
        {
            double tempValue = 0;
            // Port
            int port = 8000;
            // IP Address
            var address = "192.168.10.50";

            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(address), port);
            var newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //ClientSetting
            byte[] clientSettingarray =
            {
                0,0,        // transaction id , no use in this case
                0,0,        // id no use in this case
                0,0x14,     // length of data
                0,          // device number, no use in this case
                0x03,        //function code
                0x00,       // Start Address
                0x0,        //Number of registers
            };


            //将套接字与远程服务器地址相连
            try
            {
                newclient.Connect(ie);
                newclient.Send(clientSettingarray);
                var receiveMsg = this.ReceiveMsg(newclient);
                HexStringTextBox.Text = "讀取服务器成功:" + receiveMsg;
            }
            catch (SocketException e)
            {
                HexStringTextBox.Text = "连接服务器失败  " + e.Message;
            }

            newclient.Close();

            return "";
        }

        public string ReceiveMsg(Socket newclient)
        {
            //while (true)
            //{

            byte[] data = new byte[1024];//定义数据接收数组
            newclient.Receive(data);//接收数据到data数组
            int length = data[5];//读取数据长度
            Byte[] datashow = new byte[length + 6];//定义所要显示的接收的数据的长度
            for (int i = 0; i <= length + 5; i++)//将要显示的数据存放到数组datashow中
                datashow[i] = data[i];
            string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串
            return stringdata;

            //if (data[7] == 0x01) { showMsg01(stringdata + "\r\n"); };
            //if (data[7] == 0x02) { showMsg02(stringdata + "\r\n"); };
            //if (data[7] == 0x03) { showMsg03(stringdata + "\r\n"); };
            //if (data[7] == 0x05) { showMsg05(stringdata + "\r\n"); };
            //if (data[7] == 0x06) { showMsg06(stringdata + "\r\n"); };
            //if (data[7] == 0x0F) { showMsg0F(stringdata + "\r\n"); };
            //if (data[7] == 0x10) { showMsg10(stringdata + "\r\n"); };
            //}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var workingPath = @"C:\Upload";
            var host = "172.0.0.1";
            var errorMessage = "Robin TEST eRRORmEESAGE";

            this.WriteFailFile(host, workingPath, errorMessage);
        }

        private void WriteFailFile(string host, string workingPath, string errorMessage)
        {
            var fileName = this.GetFailFileName("Exception", "FAIL", host);
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
            var fileName = title + DateTime.Now.ToString("yyyy-MM-dd") + "@" + result + "@" + host + ".txt";
            return fileName;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void AddressListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var inputData = Convert.ToInt32(IntInputTextBox.Text);

            HexStrAnsLabel.Text = inputData.ToString("X4");
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            var optionSnAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionSnAddress"].ToString();
            var optionSnLength = System.Configuration.ConfigurationSettings.AppSettings["OptionSnLength"].ToString();
            var optionWeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWeightAddress"].ToString();
            var optionLengthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionLengthAddress"].ToString();
            var optionWidthAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionWidthAddress"].ToString();
            var optionHeightAddress = System.Configuration.ConfigurationSettings.AppSettings["OptionHeightAddress"].ToString();


            var snResult = "";
            float weightResult = 0;
            int widthResult = 0;
            int heightResult = 0;
            int lengthResult = 0;

            var hexString = HexStringTextBox.Text;
            if (hexString.Length < 64)
            {
                MessageBox.Show("Hex String Length must over 64:");
                return;
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


            //message = message + "SN:" + snResult + Environment.NewLine;
            //message = message + "重量:" + weightResult.ToString() + Environment.NewLine;
            //message = message + "長:" + lengthResult.ToString() + Environment.NewLine;
            //message = message + "寬:" + widthResult.ToString() + Environment.NewLine;
            //message = message + "高:" + heightResult.ToString() + Environment.NewLine;


            snResult = snResult.Replace("\0", "");
            snResult = snResult.Trim();

            var textFileContent = "";
            textFileContent = string.Format("SN:{0}~Weight:{1}~Length:{2}~Width:{3}~Height:{4}~HexString:{5}"
                , snResult, weightResult.ToString(), lengthResult.ToString(), widthResult.ToString(), heightResult.ToString(), hexString);

            //MessageBox.Show(textFileContent);
            //this.textBox1.Text = textFileContent;
            //MessageBox.Show("SN:" + snResult);
            //MessageBox.Show("重量:" + weightResult.ToString());
            //MessageBox.Show("長:" + lengthResult.ToString());
            //MessageBox.Show("寬:" + widthResult.ToString());
            //MessageBox.Show("高:" + heightResult.ToString());            

           
            this.SnTextBox.Text = snResult;
            this.WeightTextBox.Text = weightResult.ToString();
            this.LenTextBox.Text = lengthResult.ToString();
            this.WidthTextBox.Text = widthResult.ToString();
            this.HeightTextBox.Text = heightResult.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddressListBox.Items.Clear();
            var address = ipTextBox.Text;
            var port = Convert.ToInt32(PortTextBox.Text);
            int startAddress = Convert.ToInt32(StartAddTextBox.Text);
            int length = Convert.ToInt32(LengthTextBox.Text);
            if (!byte.TryParse(StationTextBox.Text, out byte station))
            {
                MessageBox.Show("Station input is wrong！");
                return;
            }

            ModbusClient modbusClient = new ModbusClient(address, port);    //Ip-Address and Port of Modbus-TCP-Server
            modbusClient.IPAddress = address;
            modbusClient.Port = port;
            modbusClient.UnitIdentifier = station;
            modbusClient.Connect();

            var serverResponseByte = new List<byte>();
            var hexString = "";
            try
            {
                var sendIntDataint = int.Parse(SendIntTextBox.Text);
                var sendAddress = int.Parse(SendAddressTextBox.Text);

                int[] registersToSend = new int[1];
                registersToSend[0] = int.Parse(SendIntTextBox.Text);
                modbusClient.WriteMultipleRegisters(sendAddress, registersToSend);


                SendResultLabel.Text = "Data Send Successful." + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception exc)
            {
                modbusClient.Disconnect();
                MessageBox.Show(exc.Message, "Exception Reading values from Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            modbusClient.Disconnect();


        }

        private void btn_TCP_Click(object sender, EventArgs e)
        {

        }
    }
}
