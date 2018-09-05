using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TcpClient
{
    /// <summary>
    ///  负责TCP客户端通信
    /// </summary>
    class TcpClient
    {
        private Socket      socket;
        private EndPoint    serverEndPoint;
        private EndPoint    localEndPoint;

        public TcpClient(int serverPort, int localPort)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverEndPoint = new IPEndPoint(GetLocalAddress(), serverPort);
            localEndPoint = new IPEndPoint(GetLocalAddress(), localPort);

            //绑定本地端口号
            socket.Bind(localEndPoint);
        }

        /// <summary>
        /// 连接服务端之后才能收发数据
        /// </summary>
        public void Connect()
        {
            try
            {
                socket.Connect(serverEndPoint);
            }
            catch(ApplicationException ae)
            {
                Console.WriteLine("连接服务端失败,详细错误信息：" + ae.Message);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void Send()
        {
            while(true)
            {
                string inputText = Console.ReadLine();
                if (inputText == "end")
                    break;
                try
                { 
                    int sendBytes = socket.Send(Encoding.UTF8.GetBytes(inputText));
                }
                catch(Exception e)
                {
                    Console.WriteLine("发送信息失败,详细错误信息：" + e.Message);
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public void Recv()
        {
            byte []buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int readBytes = socket.Receive(buffer);
                    string recvText = Encoding.UTF8.GetString(buffer, 0, readBytes);
                    Console.WriteLine("Receive data from server: " + recvText);
                }
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionAborted)
                    Console.WriteLine("服务端已经断开连接!");
                Console.WriteLine("接收服务器数据异常，详细信息：" + se.Message);
            }
        }

        /// <summary>
        /// 获取局域网的IPV4地址
        /// </summary>
        /// <returns></returns>
        private IPAddress GetLocalAddress()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] ipAddr = Dns.GetHostAddresses(hostName);
            foreach(IPAddress ip in ipAddr)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            throw new ApplicationException("Can't find local area network ip address!");
        }
    }
}
