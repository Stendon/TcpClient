using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tcp通信
            UInt16 serverPort = 50000, localPort = (UInt16)new Random().Next(65530);
            TcpClient client = new TcpClient(serverPort, localPort);
            client.Connect();
            
            //发送线程
            Thread sendThread = new Thread(client.Send);

            //接收线程
            Thread recvThread = new Thread(client.Recv);

            sendThread.Start();
            recvThread.Start();

            if (sendThread.IsAlive)
                sendThread.Join();

            if (recvThread.IsAlive)
                recvThread.Join();

            Console.WriteLine("Send and Recv thread already finished!");
        }
    }
}
