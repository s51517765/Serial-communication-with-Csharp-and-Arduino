using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Collections;

//シリアル通信
//https://www.atsumitakeshi.com/csharp_arduino1.html 

namespace ArduinoSerial
{
    public partial class Form1 : Form
    {
        string[] queue = new string[20];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadExisting();   // ポートから文字列を受信する
                if (!string.IsNullOrEmpty(data))
                {
                    Invoke((MethodInvoker)(() =>    // 受信用スレッドから切り替えてデータを書き込む
                    {
                        data = data.Replace("\0", System.Environment.NewLine);
                        textBoxLog.AppendText(data);
                        Application.DoEvents();
                        Thread.Sleep(1);
                        button1.Enabled = true; // ボタンを押せるようにしておく
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataSend()
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboBox1.SelectedItem.ToString();   // 選択されたCOMをポート名に設定
                serialPort1.Open(); // ポートを開く
            }

            string data = textBoxValue.Text;

            //全角数字を半角数字に変換
            data = data.Replace("０", "0");
            data = data.Replace("１", "1");
            data = data.Replace("２", "2");
            data = data.Replace("３", "3");
            data = data.Replace("４", "4");
            data = data.Replace("５", "5");
            data = data.Replace("６", "6");
            data = data.Replace("７", "7");
            data = data.Replace("８", "8");
            data = data.Replace("９", "9");


            //正規表現で数値を取得
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(data, @"\d+");
            {
                foreach (System.Text.RegularExpressions.Match m in mc)
                {
                    string value = m.ToString();
                    while (value.Length < 3) value = "0" + value.ToString(); //3桁の文字列に変換して送る
                    serialPort1.Write(value);
                    serialPort1.Write("/"); //終端文字？
                    break;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataSend();

            button1.Enabled = false;    // ボタンを押した直後、利用不能にしておく
            Application.DoEvents();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.SelectedItem.ToString();   // 選択されたCOMをポート名に設定
            serialPort1.Open(); // ポートを開く
        }
    }//class
}//namespace
