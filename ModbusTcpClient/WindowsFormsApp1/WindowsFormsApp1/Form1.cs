using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using HslCommunication.ModBus;
using HslCommunication;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ModbusTcpNet busTcpClient = null;

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.ResultTextBox.Text = "";            

            // 连接
            if (!int.TryParse(PortTextBox.Text, out int port))
            {
                this.ResultTextBox.Text = "Port format is wrong";                
                return;
            }
            
            if (!byte.TryParse("0", out byte station))
            {
                MessageBox.Show("Station input is wrong！");
                return;
            }


            busTcpClient = new ModbusTcpNet(IpTextBox.Text, port, station);
            busTcpClient.ConnectClose();

            //設定備選
            //busTcpClient.AddressStartWithZero = checkBox1.Checked;
            //busTcpClient.SetLoginAccount(textBox14.Text, textBox12.Text);
            switch (ModeComboBox.SelectedIndex)
            {
                case 0: busTcpClient.DataFormat = HslCommunication.Core.DataFormat.ABCD; break;
                case 1: busTcpClient.DataFormat = HslCommunication.Core.DataFormat.BADC; break;
                case 2: busTcpClient.DataFormat = HslCommunication.Core.DataFormat.CDAB; break;
                case 3: busTcpClient.DataFormat = HslCommunication.Core.DataFormat.DCBA; break;
                default: busTcpClient.DataFormat = HslCommunication.Core.DataFormat.ABCD; break;                    
            }

            busTcpClient.IsStringReverse = this.IsStringReverseCheckBox.Checked;

            try
            {
                OperateResult connect = busTcpClient.ConnectServer();
                if (connect.IsSuccess)
                {
                    this.ResultTextBox.Text = HslCommunication.StringResources.Language.ConnectedSuccess;
                    ReadDataButton.Enabled = true;
                    SignalReadTestbutton.Enabled = true;
                    WeightTestbutton.Enabled = true;                    
                    StatusLabel.Text = "Connect";
                }
                else
                {
                    this.ResultTextBox.Text = HslCommunication.StringResources.Language.ConnectedFailed + connect.Message + Environment.NewLine +
                        "Error: " + connect.ErrorCode;
                }
            }
            catch (Exception ex)
            {                
                this.ResultTextBox.Text = ex.Message;
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                busTcpClient.ConnectClose();
                this.ResultTextBox.Text = "Disconnect Success";
                StatusLabel.Text = "Disconnect";
                ReadDataButton.Enabled = false;
                SignalReadTestbutton.Enabled = false;
                WeightTestbutton.Enabled = false;
            }
            catch (Exception ex)
            {
                this.ResultTextBox.Text = ex.Message;
            }
        }

        private void ReadDataButton_Click(object sender, EventArgs e)
        {
            //if busTcpClient.
            this.ResultTextBox.Text = "";
            var lenth = this.LengthTextBox.Text;
            var address = this.AdressTextBox.Text;
            this.BulkReadRenderResult(busTcpClient, address, lenth, this.ResultTextBox);
        }

        public void BulkReadRenderResult(HslCommunication.Core.IReadWriteNet readWrite, string address, string lenth, TextBox resultTextBox)
        {
            try
            {
                OperateResult<byte[]> read = readWrite.Read(address, ushort.Parse(lenth));
                if (read.IsSuccess)
                {
                    var snResult = "";
                    float weightResult = 0;
                    int lengthResult = 0;
                    int widthResult = 0;
                    int heightResult = 0;
                    
                    var receiveBytesDatas = read.Content;

                    var snAdress = Convert.ToInt32(this.SnAdresstextBox.Text);
                    var snLength = Convert.ToInt32(this.SnLengthtextBox.Text);
                    var weightAdress = Convert.ToInt32(this.WeightAdresstextBox.Text);
                    var lengthAdress = Convert.ToInt32(this.LengthAdresstextBox.Text);
                    var widthAdress = Convert.ToInt32(this.WidthAdresstextBox.Text);
                    var heightAdress = Convert.ToInt32(this.HeightAdresstextBox.Text);

                    //HslCommunication.BasicFramework.SoftBasic.to(read.Content);

                    //==> SN      : Address 0 , Length: 7        r.string
                    snResult = BitConverter.ToString(receiveBytesDatas, snAdress, snLength);
                    //==> 重量      : Address 8 , Length: 2        r - float
                    weightResult = BitConverter.ToSingle(receiveBytesDatas, weightAdress);
                    //==> 長   : Address 10 , Length: 2       r - int
                    lengthResult = BitConverter.ToInt32(receiveBytesDatas, lengthAdress);
                    //==> 寬       : Address 12 , Length: 2       r - int
                    widthResult = BitConverter.ToInt32(receiveBytesDatas, widthAdress);
                    //==> 高       : Address 14 , Length: 2       r - int
                    heightResult = BitConverter.ToInt32(receiveBytesDatas, heightAdress);

                    var result = string.Format("{0},{1},{2},{3},{4}", snResult, weightResult.ToString(), lengthResult.ToString(), widthResult.ToString(), heightResult.ToString());
                    ResultTextBox.Text = "Result：" + Environment.NewLine + result 
                        + Environment.NewLine + HslCommunication.BasicFramework.SoftBasic.ByteToHexString(read.Content);
                }
                else
                {
                    ResultTextBox.Text = "Read Failed：" + read.ToMessageShowString();
                }
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = "Read Failed：" + ex.Message;                
            }
        }     

        private void SignalReadTestbutton_Click(object sender, EventArgs e)
        {
            this.ResultTextBox.Text = "";
            var lenth = this.LengthTextBox.Text;
            var address = this.AdressTextBox.Text;
            var outPutMessage = "";

            if (OutPutTypeComboBox.SelectedItem.ToString() == "String")
            {
                var result = busTcpClient.ReadString(address, ushort.Parse(lenth));
                if (result.IsSuccess)
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] String :") + result.Content + Environment.NewLine;
                }
                else
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result.ToMessageShowString()}";

                }
            }
            else if (OutPutTypeComboBox.SelectedItem.ToString() == "Int")
            {
                var result = busTcpClient.ReadInt32(address);
                if (result.IsSuccess)
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] INT :") + result.Content + Environment.NewLine;
                }
                else
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] INT :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result.ToMessageShowString()}";

                }
            }
            else if (OutPutTypeComboBox.SelectedItem.ToString() == "Float")
            {
                var result = busTcpClient.ReadFloat(address);
                if (result.IsSuccess)
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] Float :") + result.Content + Environment.NewLine;
                }
                else
                {
                    outPutMessage = DateTime.Now.ToString("[HH:mm:ss] Float :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result.ToMessageShowString()}";

                }
            }
            else
            {
                outPutMessage = "Not Support Type";
            }


            this.ResultTextBox.Text = outPutMessage;

        }

        private void WeightTestbutton_Click(object sender, EventArgs e)
        {
            this.ResultTextBox.Text = "";            
            var outPutMessage = "";
            var address = "";
            var lenth = "";

            outPutMessage = outPutMessage + "開始讀取 重量" + Environment.NewLine;
            //  ==> 重量      : Address 9 , Length: 2        r - float
            address = "9";
            lenth = "2";
            var result2 = busTcpClient.ReadFloat(address);
            if (result2.IsSuccess)
            {
                outPutMessage = outPutMessage +  "重量 讀取結果:" + result2.Content + Environment.NewLine;
            }
            else
            {
                outPutMessage = outPutMessage +  "重量 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result2.ToMessageShowString()}";

            }

            outPutMessage = outPutMessage + "開始讀取 寬" + Environment.NewLine;
            // ==> 寬       : Address 11 , Length: 2       r - int
            address = "11";
            lenth = "2";
            var result3 = busTcpClient.ReadInt32(address);
            if (result3.IsSuccess)
            {
                outPutMessage = outPutMessage +  "寬 讀取結果:" + result3.Content + Environment.NewLine;
            }
            else
            {
                outPutMessage = outPutMessage +  "寬 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result3.ToMessageShowString()}";

            }

            outPutMessage = outPutMessage + "開始讀取 高" + Environment.NewLine;
            //==> 高       : Address 13 , Length: 2       r - int
            address = "13";
            lenth = "2";
            var result4 = busTcpClient.ReadInt32(address);
            if (result4.IsSuccess)
            {
                outPutMessage = outPutMessage +  "高 讀取結果:" + result4.Content + Environment.NewLine;
            }
            else
            {
                outPutMessage = outPutMessage +  "高 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result4.ToMessageShowString()}";
            }


            outPutMessage = outPutMessage + "開始讀取SN" + Environment.NewLine;
            //  ==> SN      : Address 0 , Length: 7        r.string
            address = "0";
            lenth = "7";
            var result1 = busTcpClient.ReadString(address, ushort.Parse(lenth));
            if (result1.IsSuccess)
            {
                outPutMessage = outPutMessage + "SN 讀取結果:" + result1.Content + Environment.NewLine;
            }
            else
            {
                outPutMessage = outPutMessage + "SN 讀取結果:" + DateTime.Now.ToString("[HH:mm:ss] String :") + $"[{address}] 读取失败{Environment.NewLine}原因：{result1.ToMessageShowString()}";
            }

            this.ResultTextBox.Text = outPutMessage;

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "";
            var localProcessPath = @"C:\inetpub\wwwroot\CtoNew\Maintain\ioconfig\HTML_Del";
            var localProcessPathBak = @"C:\inetpub\wwwroot\CtoNew\Maintain\ioconfig\HTML_Temp";
            if (Directory.Exists(localProcessPathBak) == false)
            {
                Directory.CreateDirectory(localProcessPathBak);
            }
            //檢查 目前的檔案 清單 不是今天的 就移到備份區
            var nowDate = Convert.ToDateTime("2021/03/01");            
            foreach (var processFile in Directory.GetFiles(localProcessPath))
            {
                var fileDate = File.GetCreationTime(processFile);
                var fileName = Path.GetFileName(processFile);
                var needRemove = false;

                if (fileName.Substring(1, 1) != "G" && fileName.Substring(0, 4) != "1598")
                {
                    needRemove = true;
                }
                else if (DateTime.Compare(fileDate, DateTime.Now.AddDays(-90)) > 0)
                {
                    needRemove = true;
                }

                if (needRemove)
                {
                    var moveFileName = Path.Combine(localProcessPathBak, Path.GetFileName(processFile));

                    if (File.Exists(moveFileName) == false)
                    {                        
                        File.Move(processFile, moveFileName);
                    }

                    button2.Text = Path.GetFileName(processFile);
                    //break;
                }
            }

            button2.Text = "Done";
        }

        private byte[] GetPartialBytes(byte[] orignBytes , int start , int length)
        {
            var results = new List<byte>();
            var sortDatas = orignBytes.ToList();
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine(i);
                results.Add(sortDatas[start + i]);                
            }

            return results.ToArray();
        }

        public byte[] HexToByte(string hexString)
        {
            //運算後的位元組長度:16進位數字字串長/2
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                //每2位16進位數字轉換為一個10進位整數
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
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
                var fileName = "ModbusTCPClient" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".txt";
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

        private void button2_Click(object sender, EventArgs e)
        {
            var localProcessPath = @"C:\inetpub\wwwroot\CtoNew\Maintain\ioconfig\HTML";
            var writeProcessPath = @"C:\inetpub\wwwroot\CtoNew\Maintain\ioconfig";
            var needCheckDatas = this.GetTestDatas();
            var sb = new StringBuilder();
            foreach (var needCheckData in needCheckDatas)
            {
                var tempfileName = Path.Combine(localProcessPath, needCheckData.Trim() + ".html");
                if (File.Exists(tempfileName))
                {
                    sb.AppendLine(needCheckData.Trim());
                    //break;
                }
            }

            var textFileContent = "";
            textFileContent = sb.ToString();                        
            var fileName = "CheckData" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".txt";
            var txtfile = Path.Combine(writeProcessPath, fileName);
            File.WriteAllText(txtfile, textFileContent, Encoding.ASCII);

            button2.Text = "Done2";
        }


        private List<string> GetTestDatas()
        {
            var results = new List<string>();
            results.Add("1598B0039129");
            results.Add("WG2002006226");
            results.Add("WG2491013690");
            results.Add("WG2493001414");
            results.Add("WG2491013689");
            results.Add("WG1872087398");
            results.Add("WG1872087397");
            results.Add("WG1907015589");
            results.Add("WG2491013688");
            results.Add("WG2491013687");
            results.Add("WG1893009658");
            results.Add("WG2491013686");
            results.Add("WG2491013685");
            results.Add("WG2149003518");
            results.Add("WG1872087396");
            results.Add("WG1872087395");
            results.Add("WG1872087394");
            results.Add("WG2491013684");
            results.Add("WG1872087393");
            results.Add("WG2491013683");
            results.Add("WG1907015588");
            results.Add("WG2491013682");
            results.Add("WG1872087390");
            results.Add("WG1907015584");
            results.Add("WG1907015586");
            results.Add("WG1907015585");
            results.Add("WG1872087388");
            results.Add("WG2149003517");
            results.Add("WG1872087387");
            results.Add("WG1872087386");
            results.Add("WG1872087385");
            results.Add("WG2491013681");
            results.Add("WG2149003516");
            results.Add("WG2491013679");
            results.Add("WG1893009654");
            results.Add("WG1893009652");
            results.Add("WG1893009650");
            results.Add("WG1872087376");
            results.Add("WG1872087370");
            results.Add("WG2491013670");
            results.Add("WG2491013669");
            results.Add("WG2149003514");
            results.Add("WG2149003513");
            results.Add("WG2149003512");
            results.Add("WG1872087368");
            results.Add("WG1872087369");
            results.Add("WG2491013666");
            results.Add("WG2491013667");
            results.Add("WG2149003509");
            results.Add("WG1872087359");
            results.Add("WG2491013638");
            results.Add("WG2491013634");
            results.Add("WG1872087357");
            results.Add("WG1872087356");
            results.Add("WG2491013632");
            results.Add("WG2491013631");
            results.Add("WG2491013630");
            results.Add("WG2491013629");
            results.Add("WG2149003506");
            results.Add("WG2491013628");
            results.Add("WG2491013627");
            results.Add("WG1893009649");
            results.Add("WG1872087355");
            results.Add("WG1872087354");
            results.Add("WG2491013625");
            results.Add("WG1893009648");
            results.Add("WG2491013626");
            results.Add("WG1893009647");
            results.Add("WG2491013622");
            results.Add("WG2491013621");
            results.Add("WG1872087353");
            results.Add("1598B0039102");
            results.Add("WG2002006214");
            results.Add("WG2759001964");
            results.Add("WG2491013615");
            results.Add("WG1872087347");
            results.Add("WG1907015561");
            results.Add("WG2002006211");
            results.Add("WG2491013612");
            results.Add("WG1907015563");
            results.Add("WG1907015562");
            results.Add("WG2002006212");
            results.Add("WG1907015556");
            results.Add("WG1907015559");
            results.Add("WG1907015558");
            results.Add("WG1907015557");
            results.Add("WG1907015553");
            results.Add("WG2759001962");
            results.Add("WG1907015554");
            results.Add("WG2759001963");
            results.Add("WG1907015555");
            results.Add("WG2002006210");
            results.Add("WG2759001961");
            results.Add("WG1872087345");
            results.Add("1598B0039088");
            results.Add("1598B0039089");
            results.Add("1598B0039090");
            results.Add("1598B0039091");
            results.Add("WG2002006209");
            results.Add("WG2002006208");
            results.Add("1598B0039084");
            results.Add("1598B0039085");
            results.Add("1598B0039086");
            results.Add("1598B0039087");
            results.Add("1598B0039080");
            results.Add("1598B0039081");
            results.Add("1598B0039082");
            results.Add("1598B0039083");
            results.Add("WG2002006207");
            results.Add("WG1872087344");
            results.Add("WG1872087343");
            results.Add("WG1872087342");
            results.Add("WG2759001960");
            results.Add("WG1872087340");
            results.Add("WG2759001959");
            results.Add("WG2042003803");
            results.Add("WG2759001958");
            results.Add("WG1872087341");
            results.Add("WG1872087339");
            results.Add("WG2759001957");
            results.Add("WG2491013606");
            results.Add("WG2759001956");
            results.Add("WG1872087338");
            results.Add("WG2491013603");
            results.Add("WG1872087335");
            results.Add("WG2759001955");
            results.Add("WG1872087334");
            results.Add("WG1872087333");
            results.Add("WG1872087332");
            results.Add("XG2093000734");
            results.Add("XG2093000733");
            results.Add("XG2093000732");
            results.Add("XG2093000731");
            results.Add("XG2093000730");
            results.Add("XG2093000729");
            results.Add("XG2093000728");
            results.Add("XG2093000727");
            results.Add("WG2149003505");
            results.Add("WG1872087326");
            results.Add("1598B0039071");
            results.Add("WG2002006204");
            results.Add("WG1872087329");
            results.Add("WG1872087328");
            results.Add("WG2759001954");
            results.Add("WG2491013579");
            results.Add("WG2491013577");
            results.Add("WG2759001953");
            results.Add("WG1872087323");
            results.Add("WG2759001952");
            results.Add("WG1872087322");
            results.Add("WG2042003801");
            results.Add("WG2491013572");
            results.Add("WG2491013571");
            results.Add("WG2491013570");
            results.Add("WG1872087319");
            results.Add("WG1872087318");
            results.Add("WG2759001949");
            results.Add("WG1893009643");
            results.Add("WG2491013569");
            results.Add("WG2149003504");
            results.Add("WG1872087314");
            results.Add("WG1907015543");
            results.Add("WG1907015542");
            results.Add("WG2042003800");
            results.Add("WG2491013562");
            results.Add("WG1872087313");
            results.Add("WG2491013565");
            results.Add("WG2491013564");
            results.Add("WG2491013563");
            results.Add("WG2149003503");
            results.Add("WG1872087311");
            results.Add("WG1907015536");
            results.Add("WG2493001413");
            results.Add("WG1909000969");
            results.Add("WG2149003501");
            results.Add("WG2491013561");
            results.Add("WG2149003500");
            results.Add("WG2149003499");
            results.Add("WG1872087309");
            results.Add("WG1893009642");
            results.Add("WG1872087306");
            results.Add("WG1872087305");
            results.Add("WG1872087307");
            results.Add("WG1872087304");
            results.Add("WG1872087303");
            results.Add("WG1872087302");
            results.Add("1598B0039055");
            results.Add("WG2002006195");
            results.Add("WG2002006194");
            results.Add("1598B0039052");
            results.Add("1598B0039053");
            results.Add("1598B0039054");
            results.Add("1598B0039048");
            results.Add("1598B0039049");
            results.Add("1598B0039050");
            results.Add("1598B0039051");
            results.Add("WG2002006193");
            results.Add("WG2002006196");
            results.Add("1598B0039056");
            results.Add("WG2491013556");
            results.Add("WG2491013553");
            results.Add("WG2491013554");
            results.Add("WG1872087300");
            results.Add("WG2491013550");
            results.Add("WG1872087297");
            results.Add("WG2759001948");
            results.Add("WG2491013549");
            results.Add("WG1872087296");
            results.Add("WG2149003498");
            results.Add("WG2491013545");
            results.Add("WG2491013546");
            results.Add("WG1872087295");
            results.Add("WG1907015531");
            results.Add("WG2759001946");
            results.Add("WG2759001945");
            results.Add("WG1907015530");
            results.Add("WG1907015526");
            results.Add("WG1872087291");
            results.Add("WG1872087290");
            results.Add("WG1872087289");
            results.Add("WG1907015528");
            results.Add("WG1907015527");
            results.Add("WG2002006192");
            results.Add("WG2491013534");
            results.Add("WG2491013532");
            results.Add("WG2491013520");
            results.Add("WG2491013436");
            results.Add("WG2491013435");
            results.Add("WG1907015495");
            results.Add("WG1907015494");
            results.Add("WG2149003494");
            results.Add("WG1907015521");
            results.Add("WG1907015520");
            results.Add("WG2491013531");
            results.Add("WG1872087279");
            results.Add("WG2002006191");
            results.Add("WG2002006190");
            results.Add("WG2002006189");
            results.Add("WG2002006188");
            results.Add("WG2002006187");
            results.Add("WG2002006186");
            results.Add("WG1907015522");
            results.Add("WG2491013530");
            results.Add("WG1872087278");
            results.Add("WG2491013528");
            results.Add("WG2491013529");
            results.Add("WG2491013527");
            results.Add("WG2491013526");
            results.Add("WG1872087277");
            results.Add("WG2497000089");
            results.Add("WG1872087272");
            results.Add("WG1872087271");
            results.Add("WG1872087274");
            results.Add("WG1872087273");
            results.Add("WG2002006080");
            results.Add("WG2491013525");
            results.Add("WG2491013524");
            results.Add("WG2002006185");
            results.Add("WG2759001944");
            results.Add("WG2759001943");
            results.Add("WG2491013519");
            results.Add("WG2491013518");
            results.Add("WG1872087250");
            results.Add("WG1907015518");
            results.Add("WG1872087243");
            results.Add("WG1893009636");
            results.Add("WG2759001941");
            results.Add("WG1872087238");
            results.Add("WG1907015515");
            results.Add("WG1907015514");
            results.Add("YG1641000105");
            results.Add("WG2491013498");
            results.Add("WG2491013497");
            results.Add("WG2491013496");
            results.Add("WG2491013495");
            results.Add("WG1872087233");
            results.Add("WG2491013494");
            results.Add("WG2491013493");
            results.Add("WG2491013492");
            results.Add("WG2491013491");
            results.Add("WG2491013490");
            results.Add("WG2491013489");
            results.Add("WG1872087232");
            results.Add("WG2491013488");
            results.Add("WG2491013487");
            results.Add("WG2491013486");
            results.Add("WG2491013485");
            results.Add("WG2042003796");
            results.Add("WG2759001940");
            results.Add("WG1872087228");
            results.Add("WG1893009632");
            results.Add("WG2491013483");
            results.Add("WG2493001412");
            results.Add("WG1907015509");
            results.Add("WG2491013469");
            results.Add("WG2491013468");
            results.Add("WG1872087223");
            results.Add("WG1893009631");
            results.Add("WG1872087220");
            results.Add("WG1872087219");
            results.Add("WG1872087218");
            results.Add("WG1872087217");
            results.Add("WG1872087216");
            results.Add("WG2491013467");
            results.Add("WG1872087222");
            results.Add("WG1872087221");
            results.Add("WG2491013466");
            results.Add("WG2491013465");
            results.Add("WG2491013464");
            results.Add("WG2491013463");
            results.Add("WG2491013462");
            results.Add("WG2491013461");
            results.Add("WG1907015502");
            results.Add("WG1872087213");
            results.Add("WG2149003497");
            results.Add("WG2493001410");
            results.Add("WG2493001411");
            results.Add("WG2491013453");
            results.Add("WG2491013452");
            results.Add("WG2491013451");
            results.Add("WG1872087200");
            results.Add("WG1872087199");
            results.Add("WG2002006160");
            results.Add("WG2491013444");
            results.Add("WG2491013443");
            results.Add("WG2491013442");
            results.Add("WG2491013441");
            results.Add("WG2491013440");
            results.Add("WG2491013439");
            results.Add("WG2491013438");
            results.Add("WG2149003496");
            results.Add("WG2149003495");
            results.Add("WG1872087180");
            results.Add("WG2493001409");
            results.Add("WG1907015491");
            results.Add("WG1872086915");
            results.Add("WG2002006065");
            results.Add("YG1641000101");
            results.Add("WG2002006155");
            results.Add("WG1872087153");
            results.Add("WG2491013419");
            results.Add("WG1872087152");
            results.Add("WG2491013421");
            results.Add("WG2491013420");
            results.Add("WG2759001934");
            results.Add("WG2002006146");
            results.Add("WG2149003492");
            results.Add("WG2491013416");
            results.Add("WG2491013411");
            results.Add("WG1893009622");
            results.Add("WG1872087136");
            results.Add("WG1872087135");
            results.Add("1598B0038782");
            results.Add("1598B0038783");
            results.Add("1598B0038784");
            results.Add("WG2042003773");
            results.Add("WG2042003774");
            results.Add("WG1909000963");
            results.Add("WG2759001920");
            results.Add("WG1872086905");
            results.Add("WG2042003772");
            results.Add("WG2002006047");
            results.Add("WG1872086896");
            results.Add("WG1872086898");
            results.Add("WG2042003771");
            results.Add("WG2759001918");
            results.Add("WG1872086884");
            results.Add("WG2491013254");
            results.Add("WG1893009582");
            results.Add("WG1872086876");
            results.Add("WG1872086871");
            results.Add("WG2491013248");
            results.Add("WG2042003770");
            results.Add("WG2149003464");
            results.Add("WG1872086863");
            results.Add("WG2491013243");
            results.Add("WG2759001917");
            results.Add("WG1872086857");
            results.Add("WG2759001916");
            results.Add("WG2002006049");
            results.Add("WG2002006046");
            results.Add("WG1893009569");
            results.Add("WG2149003462");
            results.Add("WG2149003461");
            results.Add("WG1872086846");
            results.Add("WG1872086848");
            results.Add("WG3282000093");
            results.Add("WG2491013229");
            results.Add("WG2759001914");
            results.Add("WG1872086838");
            results.Add("WG2491013206");
            results.Add("WG2491013205");
            results.Add("WG2491013197");
            results.Add("WG2042003765");
            results.Add("WG1872086817");
            results.Add("WG1907015357");
            results.Add("WG2759001912");
            results.Add("YG1641000100");
            results.Add("WG2491013184");
            results.Add("WG2491013183");
            results.Add("WG2149003457");
            results.Add("YG1641000098");
            results.Add("YG1641000097");
            results.Add("WG2759001909");
            results.Add("WG2491013181");
            results.Add("WG2491013179");
            results.Add("WG1907015349");
            results.Add("WG1907015348");
            results.Add("WG1907015347");
            results.Add("WG2491013173");
            results.Add("WG2491013177");
            results.Add("WG1872086771");
            results.Add("WG2002006029");
            results.Add("WG2042003763");
            results.Add("WG1893009559");
            results.Add("WG1907015340");
            results.Add("WG2759001908");
            results.Add("WG2149003456");
            results.Add("WG2149003454");
            results.Add("WG1907015338");
            results.Add("WG2002006028");
            results.Add("WG2042003761");
            results.Add("WG2042003760");
            results.Add("WG2491013168");
            results.Add("WG2491013167");
            results.Add("WG2149003451");
            results.Add("WG2759001905");
            results.Add("WG1907015322");
            results.Add("WG1907015321");
            results.Add("WG1907015320");
            results.Add("WG1872086701");
            results.Add("WG2497000076");
            results.Add("WG2759001904");
            results.Add("WG1872086697");
            results.Add("WG2149003447");
            results.Add("WG2759001903");
            results.Add("YG1641000096");
            results.Add("1598B0038697");
            results.Add("1598B0038698");
            results.Add("WG2002006017");
            results.Add("WG1907015315");
            results.Add("WG1872086686");
            results.Add("WG2759001901");
            results.Add("WG1872086682");
            results.Add("WG1872086683");
            results.Add("WG2002006014");
            results.Add("XG2093000720");
            results.Add("WG1872086676");
            results.Add("1598B0038692");
            results.Add("1598B0038693");
            results.Add("WG2002006013");
            results.Add("WG2002006012");
            results.Add("1598B0038690");
            results.Add("1598B0038691");
            results.Add("1598B0038688");
            results.Add("1598B0038689");
            results.Add("WG2002006011");
            results.Add("WG2002006010");
            results.Add("1598B0038686");
            results.Add("1598B0038687");
            results.Add("1598B0038683");
            results.Add("1598B0038684");
            results.Add("1598B0038685");
            results.Add("WG2002006009");
            results.Add("WG1872086667");
            results.Add("WG1872086663");
            results.Add("WG2042003755");
            results.Add("WG2759001900");
            results.Add("WG1872086649");
            results.Add("WG2491013135");
            results.Add("WG2491013134");
            results.Add("WG2042003752");
            results.Add("WG2149003443");
            results.Add("WG2759001898");
            results.Add("WG2759001897");
            results.Add("WG2759001896");
            results.Add("WG2149003441");
            results.Add("WG2149003440");
            results.Add("WG2042003751");
            results.Add("WG2149003442");
            results.Add("WG2759001895");
            results.Add("WG2149003439");
            results.Add("WG2149003438");
            results.Add("WG2493001391");
            results.Add("WG2491013115");
            results.Add("WG2491013116");
            results.Add("WG2002006002");
            results.Add("WG2002006003");
            results.Add("1598B0038648");
            results.Add("1598B0038649");
            results.Add("WG1907015296");
            results.Add("WG1907015297");
            results.Add("WG1907015295");
            results.Add("WG2491013112");
            results.Add("WG1872086616");
            results.Add("WG1872086615");
            results.Add("WG2002006000");
            results.Add("WG1907015291");
            results.Add("WG2759001891");
            results.Add("WG3282000092");
            results.Add("WG2002005999");
            results.Add("WG1907015289");
            results.Add("WG2759001890");
            results.Add("WG1907015288");
            results.Add("WG2149003436");
            results.Add("WG2491013104");
            results.Add("WG1872086611");
            results.Add("WG2149003430");
            results.Add("WG2042003750");
            results.Add("WG2149003429");
            results.Add("WG1907015271");
            results.Add("WG1907015272");
            results.Add("XG2093000717");
            results.Add("WG1893009540");
            results.Add("WG2149003428");
            results.Add("WG1893009541");
            results.Add("WG1872086591");
            results.Add("WG2149003425");
            results.Add("WG1872086586");
            results.Add("WG1872086584");
            results.Add("WG1872086582");
            results.Add("WG1872086581");
            results.Add("WG1872086580");
            results.Add("WG2491013075");
            results.Add("WG2491013074");
            results.Add("WG2149003419");
            results.Add("WG2149003420");
            results.Add("WG1893009537");
            results.Add("WG1872086567");
            results.Add("WG1872086566");
            results.Add("WG1872086565");
            results.Add("WG2759001888");
            results.Add("YG2996000215");
            results.Add("WG1872086559");
            results.Add("WG1872086560");
            results.Add("WG1907015269");
            results.Add("WG1872086556");
            results.Add("WG1907015267");
            results.Add("WG1907015268");
            results.Add("WG2149003417");
            results.Add("WG1907015266");
            results.Add("WG2491013073");
            results.Add("WG1907015265");
            results.Add("WG2149003416");
            results.Add("WG2149003415");
            results.Add("WG2149003414");
            results.Add("WG2149003413");
            results.Add("WG1907015262");
            results.Add("WG2491013055");
            results.Add("WG1872086548");
            results.Add("WG2491013054");
            results.Add("WG2759001887");
            results.Add("WG1872086546");
            results.Add("WG1872086545");
            results.Add("WG2042003744");
            results.Add("WG1907015256");
            results.Add("WG1872086539");
            results.Add("WG1872086530");
            results.Add("WG1872086527");
            results.Add("WG1872086526");
            results.Add("WG2491013049");
            results.Add("WG2491013047");
            results.Add("WG2491013046");
            results.Add("WG2493001389");
            results.Add("WG2491013035");
            results.Add("WG2491013034");
            results.Add("WG1872086507");
            results.Add("WG2491013031");
            results.Add("YG1641000094");
            results.Add("WG2759001882");
            results.Add("WG1872086505");
            results.Add("WG1872086495");
            results.Add("XG2093000716");
            results.Add("WG2491013026");
            results.Add("WG1907015245");
            results.Add("WG2759001880");
            results.Add("WG2759001879");
            results.Add("WG2759001878");
            results.Add("WG2491013023");
            results.Add("WG2002005968");
            results.Add("WG1872086490");
            results.Add("YG2996000214");
            results.Add("WG3282000090");
            results.Add("WG2759001876");
            results.Add("WG1907015236");
            results.Add("WG2002005967");
            results.Add("1598B0038608");
            results.Add("WG1872086481");
            results.Add("WG2002005963");
            results.Add("WG3276000102");
            results.Add("WG1907015231");
            results.Add("WG1872086476");
            results.Add("WG2002005959");
            results.Add("WG2491013007");
            results.Add("WG1907015229");
            results.Add("WG2149003398");
            results.Add("WG2491013000");
            results.Add("YG1641000092");
            results.Add("YG1641000091");
            results.Add("WG2149003396");
            results.Add("WG1907015220");
            results.Add("WG2149003394");
            results.Add("WG2149003393");
            results.Add("WG2491012987");
            results.Add("WG2491012986");
            results.Add("WG2491012984");
            results.Add("WG1907015215");
            results.Add("WG2493001382");
            results.Add("WG2493001381");
            results.Add("WG1907015211");
            results.Add("WG2491012969");
            results.Add("WG1872086407");
            results.Add("WG2759001864");
            results.Add("WG2759001863");
            results.Add("WG2149003381");
            results.Add("WG1907015201");
            results.Add("WG1907015200");
            results.Add("WG1909000959");
            results.Add("WG1909000958");
            results.Add("WG2002005906");
            results.Add("WG1872086344");
            results.Add("WG2759001871");
            results.Add("WG2149003392");
            results.Add("WG2149003391");
            results.Add("WG1872086400");
            results.Add("WG1907015209");
            results.Add("WG1907015208");
            results.Add("WG1907015207");
            results.Add("WG2042003733");
            results.Add("WG1907015203");
            results.Add("WG2149003385");
            results.Add("WG1893009490");
            results.Add("WG1872086382");
            results.Add("WG1893009492");
            results.Add("WG3276000101");
            results.Add("WG2149003388");
            results.Add("WG2149003384");
            results.Add("WG2491012951");
            results.Add("WG1872086374");
            results.Add("WG1907015202");
            results.Add("WG2759001865");
            results.Add("WG1907015193");
            results.Add("WG1872086294");
            results.Add("WG2759001856");
            results.Add("WG3282000088");
            results.Add("WG2149003373");
            results.Add("WG1907015184");
            results.Add("XG2093000714");
            results.Add("WG2759001854");
            results.Add("WG1872086270");
            results.Add("WG1872086269");
            results.Add("WG1907015182");
            results.Add("WG1872086260");
            results.Add("WG1872086237");
            results.Add("WG2149003368");
            results.Add("WG2497000055");
            results.Add("WG2491012830");
            results.Add("WG2002005861");
            results.Add("WG1872086154");
            results.Add("WG2491012784");
            results.Add("WG1893009446");
            results.Add("WG2002005845");
            results.Add("WG2002005844");
            results.Add("1598B0038449");
            results.Add("WG1872086087");
            results.Add("WG1872086086");
            results.Add("WG2491012766");
            results.Add("1598B0038448");
            results.Add("WG2002005843");
            results.Add("WG1872086034");
            results.Add("WG1872086014");
            results.Add("WG2491012665");
            results.Add("1598B0038320");
            results.Add("WG2002005817");
            results.Add("WG2759001833");
            results.Add("WG1907015094");
            results.Add("WG1907015093");
            results.Add("WG2149003346");
            results.Add("WG1907015089");
            results.Add("WG3276000086");
            results.Add("WG3276000084");
            results.Add("WG3276000085");
            results.Add("WG1872085959");
            results.Add("WG3276000083");
            results.Add("WG1893009421");
            results.Add("WG2759001821");
            results.Add("WG2759001814");
            results.Add("WG2759001812");
            results.Add("WG3276000080");
            results.Add("WG1893009409");
            results.Add("WG1907015057");
            results.Add("WG2002005755");
            results.Add("WG1872085859");
            results.Add("WG2497000031");
            results.Add("WG1907015040");
            results.Add("WG2002005738");
            results.Add("1598B0038154");
            results.Add("1598B0038155");
            results.Add("WG2002005739");
            results.Add("WG2759001805");
            results.Add("YG1641000085");
            results.Add("WG2149003324");
            results.Add("WG2149003320");
            results.Add("WG1872085758");
            results.Add("WG2759001794");
            results.Add("WG2149003319");
            results.Add("WG2002005718");
            results.Add("1598B0038116");
            results.Add("WG1893009384");
            results.Add("YG1641000084");
            results.Add("YG1641000083");
            results.Add("WG1872085670");
            results.Add("WG2149003313");
            results.Add("WG3282000074");
            results.Add("YG1641000082");
            results.Add("WG2002005688");
            results.Add("1598B0037996");
            results.Add("YG1641000081");
            results.Add("WG1872085564");
            results.Add("WG1893009361");
            results.Add("WG2491012376");
            results.Add("WG2497000022");
            results.Add("WG2497000021");
            results.Add("WG1907014966");
            results.Add("WG1907014965");
            results.Add("WG2002005683");
            results.Add("WG1872085483");
            results.Add("WG1872085471");
            results.Add("WG1907014947");
            results.Add("WG2497000019");
            results.Add("WG3282000061");
            results.Add("WG2491012299");
            results.Add("WG2497000017");
            results.Add("WG2497000018");
            results.Add("WG3282000055");
            results.Add("WG3282000057");
            results.Add("WG1872085357");
            results.Add("WG2493001341");
            results.Add("WG1907014915");
            results.Add("WG3276000050");
            results.Add("WG2493001339");
            results.Add("WG2491012221");
            results.Add("WG2042003638");
            results.Add("WG1872085295");
            results.Add("WG3282000051");
            results.Add("WG3275000067");
            results.Add("1598B0037810");
            results.Add("1598B0037811");
            results.Add("WG2002005625");
            results.Add("WG2002005624");
            results.Add("1598B0037808");
            results.Add("1598B0037809");
            results.Add("WG2491012174");
            results.Add("WG3275000066");
            results.Add("WG3276000045");
            results.Add("WG3275000063");
            results.Add("WG1907014877");
            results.Add("WG1907014876");
            results.Add("WG1907014874");
            results.Add("WG1907014875");
            results.Add("WG3275000062");
            results.Add("WG1907014872");
            results.Add("WG3275000060");
            results.Add("WG3275000061");
            results.Add("WG1907014871");
            results.Add("WG2497000002");
            results.Add("WG3275000059");
            results.Add("WG2759001728");
            results.Add("WG1907014852");
            results.Add("WG1907014844");
            results.Add("WG2149003258");
            results.Add("WG2493001332");
            results.Add("WG3276000040");
            results.Add("WG2002005567");
            results.Add("1598B0037619");
            results.Add("WG1907014816");
            results.Add("WG1907014817");
            results.Add("WG1907014815");
            results.Add("WG3282000041");
            results.Add("WG3275000047");
            results.Add("WG3275000045");
            results.Add("WG3275000044");
            results.Add("WG3276000034");
            results.Add("WG2002005562");
            results.Add("1598B0037601");
            results.Add("1598B0037602");
            results.Add("WG2759001707");
            results.Add("WG2759001706");
            results.Add("WG2491011963");
            results.Add("WG2491011962");
            results.Add("WG2491011960");
            results.Add("WG1907014794");
            results.Add("WG2002005552");
            results.Add("1598B0037588");
            results.Add("1598B0037585");
            results.Add("1598B0037586");
            results.Add("WG2002005550");
            results.Add("WG2002005549");
            results.Add("1598B0037583");
            results.Add("1598B0037584");
            results.Add("WG1907014789");
            results.Add("WG1907014788");
            results.Add("WG3282000034");
            results.Add("WG3282000033");
            results.Add("WG3282000029");
            results.Add("WG3282000031");
            results.Add("WG3282000030");
            results.Add("WG1872084885");
            results.Add("YG1641000080");
            results.Add("WG2149003219");
            results.Add("WG3275000039");
            results.Add("WG3275000038");
            results.Add("WG2491011727");
            results.Add("WG3282000025");
            results.Add("WG1872084644");
            results.Add("WG3276000028");
            results.Add("WG1907014726");
            results.Add("WG3276000027");
            results.Add("WG3276000026");
            results.Add("WG1907014722");
            results.Add("WG1907014721");
            results.Add("WG1907014720");
            results.Add("WG1907014719");
            results.Add("WG1907014713");
            results.Add("WG1907014711");
            results.Add("WG1872084501");
            results.Add("YG1641000079");
            results.Add("1598B0037396");
            results.Add("1598B0037397");
            results.Add("WG2002005490");
            results.Add("WG1907014679");
            results.Add("YG1641000078");
            results.Add("WG1907014632");
            results.Add("WG1907014631");
            results.Add("WG3275000036");
            results.Add("WG2042003525");
            results.Add("WG2042003524");
            results.Add("WG2042003523");
            results.Add("WG2042003522");
            results.Add("WG2042003521");
            results.Add("WG2042003520");
            results.Add("WG2149003191");
            results.Add("WG2759001649");
            results.Add("WG2759001648");
            results.Add("WG2042003519");
            results.Add("WG2149003189");
            results.Add("WG2149003188");
            results.Add("WG2002005472");
            results.Add("1598B0037351");
            results.Add("WG3275000035");
            results.Add("1598B0037336");
            results.Add("WG2002005465");
            results.Add("WG1872084369");
            results.Add("WG1872084349");
            results.Add("WG2042003508");
            results.Add("WG3275000030");
            results.Add("WG3275000033");
            results.Add("WG3275000031");
            results.Add("WG3275000032");
            results.Add("WG2491011554");
            results.Add("WG2759001639");
            results.Add("WG1907014560");
            results.Add("WG1907014559");
            results.Add("WG2002005453");
            results.Add("WG1872084297");
            results.Add("WG2491011517");
            results.Add("WG1893009194");
            results.Add("WG1907014543");
            results.Add("WG1907014542");
            results.Add("WG2002005444");
            results.Add("WG1907014539");
            results.Add("WG1872084237");
            results.Add("XG2093000693");
            results.Add("WG1907014506");
            results.Add("WG2491011432");
            results.Add("WG2491011430");
            results.Add("WG2491011427");
            results.Add("WG2491011426");
            results.Add("WG2491011425");
            results.Add("WG2491011424");
            results.Add("WG2491011423");
            results.Add("WG2491011421");
            results.Add("WG1872084177");
            results.Add("WG2491011375");
            results.Add("WG2491011369");
            results.Add("WG2491011355");
            results.Add("WG1872084130");
            results.Add("WG1893009163");


            return results;
        }

    }
}
