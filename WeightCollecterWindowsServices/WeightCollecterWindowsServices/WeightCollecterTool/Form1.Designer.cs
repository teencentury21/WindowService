namespace WeightCollecterTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.StationTextBox = new System.Windows.Forms.TextBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StartAddTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LengthTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.HexStringTextBox = new System.Windows.Forms.TextBox();
            this.SnTextBox = new System.Windows.Forms.TextBox();
            this.WeightTextBox = new System.Windows.Forms.TextBox();
            this.LenTextBox = new System.Windows.Forms.TextBox();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.AddressListBox = new System.Windows.Forms.ListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.IntInputTextBox = new System.Windows.Forms.TextBox();
            this.HexStrAnsLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.SendIntTextBox = new System.Windows.Forms.TextBox();
            this.SendResultLabel = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SendAddressTextBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_TCP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "站點";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 133);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "讀取測試(ModBus)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // StationTextBox
            // 
            this.StationTextBox.Location = new System.Drawing.Point(56, 95);
            this.StationTextBox.Name = "StationTextBox";
            this.StationTextBox.Size = new System.Drawing.Size(100, 22);
            this.StationTextBox.TabIndex = 6;
            this.StationTextBox.Text = "0";
            this.StationTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(56, 55);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 22);
            this.PortTextBox.TabIndex = 7;
            this.PortTextBox.Text = "8000";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(56, 12);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(100, 22);
            this.ipTextBox.TabIndex = 8;
            this.ipTextBox.Text = "192.168.10.50";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Port";
            // 
            // StartAddTextBox
            // 
            this.StartAddTextBox.Location = new System.Drawing.Point(290, 61);
            this.StartAddTextBox.Name = "StartAddTextBox";
            this.StartAddTextBox.Size = new System.Drawing.Size(100, 22);
            this.StartAddTextBox.TabIndex = 12;
            this.StartAddTextBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "StartAddress";
            // 
            // LengthTextBox
            // 
            this.LengthTextBox.Location = new System.Drawing.Point(290, 95);
            this.LengthTextBox.Name = "LengthTextBox";
            this.LengthTextBox.Size = new System.Drawing.Size(100, 22);
            this.LengthTextBox.TabIndex = 14;
            this.LengthTextBox.Text = "20";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(209, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Length";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "Hex String";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "SN";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "重量";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "長";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 354);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "寬";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 386);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "高";
            // 
            // HexStringTextBox
            // 
            this.HexStringTextBox.Location = new System.Drawing.Point(12, 190);
            this.HexStringTextBox.Name = "HexStringTextBox";
            this.HexStringTextBox.Size = new System.Drawing.Size(417, 22);
            this.HexStringTextBox.TabIndex = 22;
            // 
            // SnTextBox
            // 
            this.SnTextBox.Location = new System.Drawing.Point(56, 243);
            this.SnTextBox.Name = "SnTextBox";
            this.SnTextBox.Size = new System.Drawing.Size(100, 22);
            this.SnTextBox.TabIndex = 23;
            // 
            // WeightTextBox
            // 
            this.WeightTextBox.Location = new System.Drawing.Point(56, 282);
            this.WeightTextBox.Name = "WeightTextBox";
            this.WeightTextBox.Size = new System.Drawing.Size(100, 22);
            this.WeightTextBox.TabIndex = 24;
            // 
            // LenTextBox
            // 
            this.LenTextBox.Location = new System.Drawing.Point(56, 319);
            this.LenTextBox.Name = "LenTextBox";
            this.LenTextBox.Size = new System.Drawing.Size(100, 22);
            this.LenTextBox.TabIndex = 25;
            // 
            // WidthTextBox
            // 
            this.WidthTextBox.Location = new System.Drawing.Point(56, 351);
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.Size = new System.Drawing.Size(100, 22);
            this.WidthTextBox.TabIndex = 26;
            // 
            // HeightTextBox
            // 
            this.HeightTextBox.Location = new System.Drawing.Point(56, 383);
            this.HeightTextBox.Name = "HeightTextBox";
            this.HeightTextBox.Size = new System.Drawing.Size(100, 22);
            this.HeightTextBox.TabIndex = 27;
            // 
            // AddressListBox
            // 
            this.AddressListBox.FormattingEnabled = true;
            this.AddressListBox.ItemHeight = 12;
            this.AddressListBox.Location = new System.Drawing.Point(452, 27);
            this.AddressListBox.Name = "AddressListBox";
            this.AddressListBox.Size = new System.Drawing.Size(471, 256);
            this.AddressListBox.TabIndex = 28;
            this.AddressListBox.SelectedIndexChanged += new System.EventHandler(this.AddressListBox_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(450, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "Address List";
            // 
            // IntInputTextBox
            // 
            this.IntInputTextBox.Location = new System.Drawing.Point(551, 289);
            this.IntInputTextBox.Name = "IntInputTextBox";
            this.IntInputTextBox.Size = new System.Drawing.Size(100, 22);
            this.IntInputTextBox.TabIndex = 31;
            this.IntInputTextBox.Text = "0";
            // 
            // HexStrAnsLabel
            // 
            this.HexStrAnsLabel.AutoSize = true;
            this.HexStrAnsLabel.Location = new System.Drawing.Point(760, 292);
            this.HexStrAnsLabel.Name = "HexStrAnsLabel";
            this.HexStrAnsLabel.Size = new System.Drawing.Size(81, 12);
            this.HexStrAnsLabel.TabIndex = 32;
            this.HexStrAnsLabel.Text = "HexStrAnsLabel";
            this.HexStrAnsLabel.Click += new System.EventHandler(this.label14_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 289);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(663, 351);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 35;
            this.button4.Text = "發送測試";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(450, 322);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 37;
            this.label14.Text = "發送數據 (Int)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(450, 292);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 12);
            this.label15.TabIndex = 39;
            this.label15.Text = "Int To Hex String";
            // 
            // SendIntTextBox
            // 
            this.SendIntTextBox.Location = new System.Drawing.Point(551, 319);
            this.SendIntTextBox.Name = "SendIntTextBox";
            this.SendIntTextBox.Size = new System.Drawing.Size(100, 22);
            this.SendIntTextBox.TabIndex = 40;
            this.SendIntTextBox.Text = "2";
            // 
            // SendResultLabel
            // 
            this.SendResultLabel.AutoSize = true;
            this.SendResultLabel.Location = new System.Drawing.Point(450, 383);
            this.SendResultLabel.Name = "SendResultLabel";
            this.SendResultLabel.Size = new System.Drawing.Size(53, 12);
            this.SendResultLabel.TabIndex = 41;
            this.SendResultLabel.Text = "發送結果";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(450, 354);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 12);
            this.label16.TabIndex = 42;
            this.label16.Text = "發送 Address";
            // 
            // SendAddressTextBox
            // 
            this.SendAddressTextBox.Location = new System.Drawing.Point(551, 347);
            this.SendAddressTextBox.Name = "SendAddressTextBox";
            this.SendAddressTextBox.Size = new System.Drawing.Size(100, 22);
            this.SendAddressTextBox.TabIndex = 43;
            this.SendAddressTextBox.Text = "20";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(239, 133);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 23);
            this.button2.TabIndex = 44;
            this.button2.Text = "Hex String 內容解析";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // btn_TCP
            // 
            this.btn_TCP.Location = new System.Drawing.Point(133, 133);
            this.btn_TCP.Name = "btn_TCP";
            this.btn_TCP.Size = new System.Drawing.Size(100, 23);
            this.btn_TCP.TabIndex = 45;
            this.btn_TCP.Text = "讀取測試(TCP)";
            this.btn_TCP.UseVisualStyleBackColor = true;
            this.btn_TCP.Click += new System.EventHandler(this.btn_TCP_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 415);
            this.Controls.Add(this.btn_TCP);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SendAddressTextBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.SendResultLabel);
            this.Controls.Add(this.SendIntTextBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.HexStrAnsLabel);
            this.Controls.Add(this.IntInputTextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.AddressListBox);
            this.Controls.Add(this.HeightTextBox);
            this.Controls.Add(this.WidthTextBox);
            this.Controls.Add(this.LenTextBox);
            this.Controls.Add(this.WeightTextBox);
            this.Controls.Add(this.SnTextBox);
            this.Controls.Add(this.HexStringTextBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LengthTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.StartAddTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.StationTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox StationTextBox;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox StartAddTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox LengthTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox HexStringTextBox;
        private System.Windows.Forms.TextBox SnTextBox;
        private System.Windows.Forms.TextBox WeightTextBox;
        private System.Windows.Forms.TextBox LenTextBox;
        private System.Windows.Forms.TextBox WidthTextBox;
        private System.Windows.Forms.TextBox HeightTextBox;
        private System.Windows.Forms.ListBox AddressListBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox IntInputTextBox;
        private System.Windows.Forms.Label HexStrAnsLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox SendIntTextBox;
        private System.Windows.Forms.Label SendResultLabel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox SendAddressTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_TCP;
    }
}

