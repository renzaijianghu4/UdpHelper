using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpHelper
{
    public class Server
    {
        public delegate void ReceiveHandler(byte[] param);//声明委托  
        /// <summary>
        /// 用于处理接收到的数据。使用方法：ReceiveEvent += func;   格式为 private[隐私类别] [返回类型] func(byte[] data)
        /// </summary>
        public event ReceiveHandler ReceiveEvent;//声明事件
        /// <summary>
        /// 监听端口，整型，取值范围1-65535，建议取值>2000
        /// </summary>
        private int Port;//监听端口
        private UdpClient clt;
        private Thread listenThread;
        private bool ListenFlag = false; //监听标志
        /// <summary>
        /// 当前端口监听状态，true:监听中，false:未监听
        /// </summary>
        public bool IsListenRun
        {
            get { return ListenFlag; }
        }
        public Server(int port)
        {
            Port = port;
        }
        /// <summary>
        /// 启动监听
        /// </summary>
        /// <returns>true为成功，false为失败(建议更换端口号)</returns>
        public bool Start()
        {
            ListenFlag = true;
            try
            {
                clt = new UdpClient(Port);
            }
            catch (Exception)
            {
                return false;
            }
            listenThread = new Thread(new ThreadStart(Receive));
            listenThread.IsBackground = true;
            listenThread.Start();
            return true;
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            ListenFlag = false;
            listenThread.Abort();
            clt.Close();
        }
        private void Receive()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveData = null;
            while (ListenFlag)
            {
                receiveData = clt.Receive(ref iep);
                if (ReceiveEvent != null)
                {
                    ReceiveEvent(receiveData);//将接收到的数据交由外部处理
                    //TODO:需要查看数据来源时需要对iep进行处理
                }
            }
        }
    }
    public class Client
    {
        /// <summary>
        /// 发送UDP数据(字节)
        /// </summary>
        /// <param name="dstIep">目标主机</param>
        /// <param name="data">待发送数据</param>
        public static void Send(IPEndPoint dstIep, byte[] data)
        {
            UdpClient clt = new UdpClient();
            clt.Connect(dstIep);
            clt.Send(data, data.Length);
        }
        /// <summary>
        /// 发送UDP数据(文本)
        /// </summary>
        /// <param name="dstIep">目标主机</param>
        /// <param name="msg">待发送文本信息</param>
        public static void Send(IPEndPoint dstIep, string msg)
        {
            UdpClient clt = new UdpClient();
            clt.Connect(dstIep);
            byte[] data = Encoding.Default.GetBytes(msg);
            clt.Send(data, data.Length);
        }
        /// <summary>
        /// 发送UDP数据(文本)
        /// </summary>
        /// <param name="dstIep">目标主机，格式如"127.0.0.1:12345",冒号为英文</param>
        /// <param name="msg">待发送文本信息</param>
        /// <returns>true:无误，false:端口格式输入有误</returns>
        public static bool Send(string dstIep, string msg)
        {
            IPEndPoint iep;
            try
            {
                iep = new IPEndPoint(IPAddress.Parse(dstIep.Split(':')[0]), int.Parse(dstIep.Split(':')[1]));
            }
            catch (Exception)
            {
                return false;
            }
            UdpClient clt = new UdpClient();
            clt.Connect(iep);
            byte[] data = Encoding.Default.GetBytes(msg);
            clt.Send(data, data.Length);
            return true;
        }
        /// <summary>
        /// 发送UDP数据(数据)
        /// </summary>
        /// <param name="dstIep">目标主机，格式如"127.0.0.1:12345",冒号为英文</param>
        /// <param name="data">待发送数据</param>
        /// <returns>true:无误，false:端口格式输入有误</returns>
        public static bool Send(string dstIep, byte[] data)
        {
            IPEndPoint iep;
            try
            {
                iep = new IPEndPoint(IPAddress.Parse(dstIep.Split(':')[0]), int.Parse(dstIep.Split(':')[1]));
            }
            catch (Exception)
            {
                return false;
            }
            UdpClient clt = new UdpClient();
            clt.Connect(iep);
            clt.Send(data, data.Length);
            return true;
        }
    }

}
