namespace WindowsFormsApp1
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.IpTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.ReadDataButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.AdressTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LengthTextBox = new System.Windows.Forms.TextBox();
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.IsStringReverseCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OutPutTypeComboBox = new System.Windows.Forms.ComboBox();
            this.SignalReadTestbutton = new System.Windows.Forms.Button();
            this.WeightTestbutton = new System.Windows.Forms.Button();
            this.SnAdresstextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SnLengthtextBox = new System.Windows.Forms.TextBox();
            this.WeightAdresstextBox = new System.Windows.Forms.TextBox();
            this.WidthAdresstextBox = new System.Windows.Forms.TextBox();
            this.HeightAdresstextBox = new System.Windows.Forms.TextBox();
            this.LengthAdresstextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(17, 131);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(110, 33);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Location = new System.Drawing.Point(9, 313);
            this.ResultTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ResultTextBox.Multiline = true;
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.Size = new System.Drawing.Size(390, 208);
            this.ResultTextBox.TabIndex = 1;
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(144, 131);
            this.DisconnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(110, 33);
            this.DisconnectButton.TabIndex = 2;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP : ";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(251, 20);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(130, 22);
            this.PortTextBox.TabIndex = 4;
            this.PortTextBox.Text = "8000";
            // 
            // IpTextBox
            // 
            this.IpTextBox.Location = new System.Drawing.Point(62, 20);
            this.IpTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.IpTextBox.Name = "IpTextBox";
            this.IpTextBox.Size = new System.Drawing.Size(122, 22);
            this.IpTextBox.TabIndex = 5;
            this.IpTextBox.Text = "192.168.10.50";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 139);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Status : ";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(325, 139);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(56, 12);
            this.StatusLabel.TabIndex = 8;
            this.StatusLabel.Text = "Disconnect";
            // 
            // ReadDataButton
            // 
            this.ReadDataButton.Enabled = false;
            this.ReadDataButton.Location = new System.Drawing.Point(17, 168);
            this.ReadDataButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReadDataButton.Name = "ReadDataButton";
            this.ReadDataButton.Size = new System.Drawing.Size(110, 33);
            this.ReadDataButton.TabIndex = 9;
            this.ReadDataButton.Text = "批量讀取測試";
            this.ReadDataButton.UseVisualStyleBackColor = true;
            this.ReadDataButton.Click += new System.EventHandler(this.ReadDataButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Adress : ";
            // 
            // AdressTextBox
            // 
            this.AdressTextBox.Location = new System.Drawing.Point(62, 45);
            this.AdressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.AdressTextBox.Name = "AdressTextBox";
            this.AdressTextBox.Size = new System.Drawing.Size(122, 22);
            this.AdressTextBox.TabIndex = 11;
            this.AdressTextBox.Text = "100";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 47);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "Length : ";
            // 
            // LengthTextBox
            // 
            this.LengthTextBox.Location = new System.Drawing.Point(252, 45);
            this.LengthTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.LengthTextBox.Name = "LengthTextBox";
            this.LengthTextBox.Size = new System.Drawing.Size(130, 22);
            this.LengthTextBox.TabIndex = 13;
            this.LengthTextBox.Text = "10";
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "HslCommunication.Core.DataFormat.ABCD",
            "HslCommunication.Core.DataFormat.BADC",
            "HslCommunication.Core.DataFormat.CDAB",
            "HslCommunication.Core.DataFormat.DCBA"});
            this.ModeComboBox.Location = new System.Drawing.Point(63, 72);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(268, 20);
            this.ModeComboBox.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 72);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "Mode : ";
            // 
            // IsStringReverseCheckBox
            // 
            this.IsStringReverseCheckBox.AutoSize = true;
            this.IsStringReverseCheckBox.Location = new System.Drawing.Point(251, 98);
            this.IsStringReverseCheckBox.Name = "IsStringReverseCheckBox";
            this.IsStringReverseCheckBox.Size = new System.Drawing.Size(72, 16);
            this.IsStringReverseCheckBox.TabIndex = 21;
            this.IsStringReverseCheckBox.Text = "字串反轉";
            this.IsStringReverseCheckBox.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 99);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "Out Put Type";
            // 
            // OutPutTypeComboBox
            // 
            this.OutPutTypeComboBox.FormattingEnabled = true;
            this.OutPutTypeComboBox.Items.AddRange(new object[] {
            "String",
            "Float",
            "Int"});
            this.OutPutTypeComboBox.Location = new System.Drawing.Point(87, 96);
            this.OutPutTypeComboBox.Name = "OutPutTypeComboBox";
            this.OutPutTypeComboBox.Size = new System.Drawing.Size(121, 20);
            this.OutPutTypeComboBox.TabIndex = 23;
            // 
            // SignalReadTestbutton
            // 
            this.SignalReadTestbutton.Enabled = false;
            this.SignalReadTestbutton.Location = new System.Drawing.Point(144, 168);
            this.SignalReadTestbutton.Margin = new System.Windows.Forms.Padding(2);
            this.SignalReadTestbutton.Name = "SignalReadTestbutton";
            this.SignalReadTestbutton.Size = new System.Drawing.Size(110, 33);
            this.SignalReadTestbutton.TabIndex = 24;
            this.SignalReadTestbutton.Text = "單數據讀取測試";
            this.SignalReadTestbutton.UseVisualStyleBackColor = true;
            this.SignalReadTestbutton.Click += new System.EventHandler(this.SignalReadTestbutton_Click);
            // 
            // WeightTestbutton
            // 
            this.WeightTestbutton.Enabled = false;
            this.WeightTestbutton.Location = new System.Drawing.Point(271, 168);
            this.WeightTestbutton.Margin = new System.Windows.Forms.Padding(2);
            this.WeightTestbutton.Name = "WeightTestbutton";
            this.WeightTestbutton.Size = new System.Drawing.Size(110, 33);
            this.WeightTestbutton.TabIndex = 25;
            this.WeightTestbutton.Text = "重量資訊讀取測試";
            this.WeightTestbutton.UseVisualStyleBackColor = true;
            this.WeightTestbutton.Click += new System.EventHandler(this.WeightTestbutton_Click);
            // 
            // SnAdresstextBox
            // 
            this.SnAdresstextBox.Location = new System.Drawing.Point(70, 210);
            this.SnAdresstextBox.Name = "SnAdresstextBox";
            this.SnAdresstextBox.Size = new System.Drawing.Size(22, 22);
            this.SnAdresstextBox.TabIndex = 26;
            this.SnAdresstextBox.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 220);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "SnAdress";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(134, 220);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "SnLength";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 248);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "WeightAdress";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 281);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 12);
            this.label11.TabIndex = 33;
            this.label11.Text = "LengthAdress";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(129, 281);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 12);
            this.label12.TabIndex = 34;
            this.label12.Text = "WidthAddress";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(250, 281);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 12);
            this.label13.TabIndex = 35;
            this.label13.Text = "HeightAdress";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // SnLengthtextBox
            // 
            this.SnLengthtextBox.Location = new System.Drawing.Point(189, 210);
            this.SnLengthtextBox.Name = "SnLengthtextBox";
            this.SnLengthtextBox.Size = new System.Drawing.Size(22, 22);
            this.SnLengthtextBox.TabIndex = 36;
            this.SnLengthtextBox.Text = "7";
            // 
            // WeightAdresstextBox
            // 
            this.WeightAdresstextBox.Location = new System.Drawing.Point(97, 245);
            this.WeightAdresstextBox.Name = "WeightAdresstextBox";
            this.WeightAdresstextBox.Size = new System.Drawing.Size(22, 22);
            this.WeightAdresstextBox.TabIndex = 37;
            this.WeightAdresstextBox.Text = "9";
            // 
            // WidthAdresstextBox
            // 
            this.WidthAdresstextBox.Location = new System.Drawing.Point(205, 278);
            this.WidthAdresstextBox.Name = "WidthAdresstextBox";
            this.WidthAdresstextBox.Size = new System.Drawing.Size(22, 22);
            this.WidthAdresstextBox.TabIndex = 38;
            this.WidthAdresstextBox.Text = "13";
            // 
            // HeightAdresstextBox
            // 
            this.HeightAdresstextBox.Location = new System.Drawing.Point(322, 276);
            this.HeightAdresstextBox.Name = "HeightAdresstextBox";
            this.HeightAdresstextBox.Size = new System.Drawing.Size(22, 22);
            this.HeightAdresstextBox.TabIndex = 39;
            this.HeightAdresstextBox.Text = "15";
            // 
            // LengthAdresstextBox
            // 
            this.LengthAdresstextBox.Location = new System.Drawing.Point(97, 273);
            this.LengthAdresstextBox.Name = "LengthAdresstextBox";
            this.LengthAdresstextBox.Size = new System.Drawing.Size(22, 22);
            this.LengthAdresstextBox.TabIndex = 40;
            this.LengthAdresstextBox.Text = "11";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(271, 208);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 41;
            this.button1.Text = "搬移老舊數據";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(251, 237);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 527);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LengthAdresstextBox);
            this.Controls.Add(this.HeightAdresstextBox);
            this.Controls.Add(this.WidthAdresstextBox);
            this.Controls.Add(this.WeightAdresstextBox);
            this.Controls.Add(this.SnLengthtextBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SnAdresstextBox);
            this.Controls.Add(this.WeightTestbutton);
            this.Controls.Add(this.SignalReadTestbutton);
            this.Controls.Add(this.OutPutTypeComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.IsStringReverseCheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ModeComboBox);
            this.Controls.Add(this.LengthTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AdressTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ReadDataButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IpTextBox);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.ConnectButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.TextBox IpTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button ReadDataButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AdressTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LengthTextBox;
        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox IsStringReverseCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox OutPutTypeComboBox;
        private System.Windows.Forms.Button SignalReadTestbutton;
        private System.Windows.Forms.Button WeightTestbutton;
        private System.Windows.Forms.TextBox SnAdresstextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SnLengthtextBox;
        private System.Windows.Forms.TextBox WeightAdresstextBox;
        private System.Windows.Forms.TextBox WidthAdresstextBox;
        private System.Windows.Forms.TextBox HeightAdresstextBox;
        private System.Windows.Forms.TextBox LengthAdresstextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

