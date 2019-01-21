using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
namespace BoatChat
{
    public class ServerConnct
    {
        IPAddress serverIP;
        IPEndPoint serverEndPoint;
        Socket serverSocket;
        
        public ServerConnct()
        {
            serverIP = IPAddress.Parse("166.111.140.14");
            serverEndPoint = new IPEndPoint(serverIP, 8000);
        }

        public string ServerQuery(string strQuery)
        {
            string strReceive = null;
            byte[] byteQuery = Encoding.UTF8.GetBytes(strQuery);
            byte[] byteReceive = new byte[1024 * 2048];
            int lenReceive;
            serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            //建立连接
            try
            {
                serverSocket.Connect(serverEndPoint);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
            serverSocket.Send(byteQuery);
            lenReceive = serverSocket.Receive(byteReceive);
            strReceive = Encoding.UTF8.GetString(byteReceive, 0, lenReceive);
            serverSocket.Close();
            return strReceive;
        }
         

    }
}
