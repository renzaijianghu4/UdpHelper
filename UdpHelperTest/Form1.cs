using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UdpHelper;

namespace UdpHelperTest
{
    public partial class Form1 : Form
    {
        Server udpServer;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (btnListen.Text == "启动监听")
            {
                udpServer = new Server(int.Parse(txtPort.Text));
                if (udpServer.Start())
                {
                    udpServer.ReceiveEvent += DisReceiveMsg;//处理接收到的数据函数
                    btnListen.Text = "停止监听";
                    txtPort.Enabled = false;
                }
                else MessageBox.Show("端口错误，请更换");
            }
            else
            {
                udpServer.Stop();
                btnListen.Text = "启动监听";
                txtPort.Enabled = true;
            }
        }
       /// <summary>
       /// 将UDP监听端口收到的数据转化为文本显示
       /// </summary>
       /// <param name="data">接收到的udp数据字节组</param>
        private void DisReceiveMsg(byte[] data)
        {
            txtRecvMsg.Text += DateTime.Now.ToString("HH:mm:ss") + " Received:" + Environment.NewLine;
            txtRecvMsg.Text += Encoding.Default.GetString(data) + Environment.NewLine;
        }





        private void btnSend_Click(object sender, EventArgs e)
        {
            Client.Send(txtRemoteHost.Text.Replace(" ", "").ToString() + ":" + txtRemotePort.Text, txtMsgSend.Text);    //客户端定义了4种类型的发送方式  
        }

    }
}
