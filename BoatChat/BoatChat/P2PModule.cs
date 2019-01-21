using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
namespace BoatChat
{
    public class P2PModule
    {
        IPAddress ListenerIP;
        EndPoint ListenerEP, PeerEP;
        public Socket ListenerSocket, ReceiveSocket, PeerSocket;
        int port = 52176;
        byte[] receiveByte = new byte[1024 * 1024];
        public byte[] receiveByteAll = new byte[1024 * 1024 * 1024];
        public int lenReceiveAll; //接收的总的字节数
        public IPEndPoint remoteEP;
        public IPAddress remoteIP;
        public bool newMessage = false;

        public P2PModule(string MyIP)
        {
            ListenerIP = IPAddress.Parse(MyIP);
            ListenerEP = new IPEndPoint(ListenerIP, port);
        }

        /*
           协议说明：
           共14位，例：20160114980000
           其中，前10位为学号，第11位和第12位为类别，第13位和第14位为扩展位
           
           侦听线程的软停止条件为 收到一个字节的0
        */
        public void P2PSendMess(IPAddress DestIP, byte[] sendByte)
        {
            PeerEP = new IPEndPoint(DestIP, port);
            PeerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //建立连接
            try
            {
                PeerSocket.Connect(PeerEP);
            }
            catch(SocketException se)
            {
                MessageBox.Show(se.Message);
                return;
            }
            PeerSocket.Send(sendByte);
            PeerSocket.Close();
        }


        public void P2PListen()
        {
            int backlog = 10;
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            // bind 侦听 socket
            {
                ListenerSocket.Bind(ListenerEP);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                return;
            }
            //开始侦听
            ListenerSocket.Listen(backlog);
            while(true)
            {
                try
                {
                    ReceiveSocket = ListenerSocket.Accept();
                }
                catch(SocketException se)
                {
                    MessageBox.Show(se.Message);
                }
                //记录IP和EP
                remoteEP = (IPEndPoint)ReceiveSocket.RemoteEndPoint;
                remoteIP = remoteEP.Address;

                //开始接收消息
                int lentmp = ReceiveSocket.Receive(receiveByte);
                lenReceiveAll = 0;
                //把接收到的复制到receiveByteAll中
                Array.Copy(receiveByte, 0, receiveByteAll, lenReceiveAll, lentmp);
                lenReceiveAll += lentmp;
                
                //循环，把后续所有的都收到并存在receiveByteAll中
                while(lentmp > 0)
                {
                    lentmp = ReceiveSocket.Receive(receiveByte);
                    Array.Copy(receiveByte, 0, receiveByteAll, lenReceiveAll, lentmp);
                    lenReceiveAll += lentmp;
                }
                
                //软停止，收到一字节的0
                if(lenReceiveAll == 1)
                {
                    if (receiveByteAll[0] == 0)
                        break;

                }
                ReceiveSocket.Close();
                newMessage = true;

            }
            ListenerSocket.Close();
        }



    }
}
