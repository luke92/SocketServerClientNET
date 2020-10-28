using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SocketLibrary
{
    public static class SocketHelper
    {
        public static MyMessage Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return new MyMessage { Data = memoryStream.ToArray() };
            }
                       
        }

        public static MyMessage Serialize(string data)
        {
            return new MyMessage { Data = Encoding.ASCII.GetBytes(data) };

        }

        public static object Deserialize(MyMessage message)
        {
            using (var memoryStream = new MemoryStream(message.Data))
                return new BinaryFormatter().Deserialize(memoryStream);
        }

        public static string DeserializeToString(MyMessage message)
        {
            return Encoding.ASCII.GetString(message.Data);
        }

        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        public static Socket GetSocket(string hostnameOrIp = "", int port = 11111)
        {
            if (string.IsNullOrEmpty(hostnameOrIp))
            {
                hostnameOrIp = Dns.GetHostName();
            }
            IPHostEntry ipHost = Dns.GetHostEntry(hostnameOrIp);
            IPAddress ipAddr = ipHost.AddressList[0];

            return new Socket(ipAddr.AddressFamily,
                       SocketType.Stream, ProtocolType.Tcp);
        }

        public static IPEndPoint GetEndPoint(string hostnameOrIp = "", int port = 11111)
        {
            if (string.IsNullOrEmpty(hostnameOrIp))
            {
                hostnameOrIp = Dns.GetHostName();
            }
            IPHostEntry ipHost = Dns.GetHostEntry(hostnameOrIp);
            IPAddress ipAddr = ipHost.AddressList[0];
            return new IPEndPoint(ipAddr, port);
        }

        public static int SendData(Socket sender, string message)
        {
            var messageSend = Serialize(message);

            return sender.Send(messageSend.Data);
        }

        public static Tuple<int,string> ReceiveBytesReceiveAndData(Socket receiver, int bytes)
        {
            var messageReceived = new MyMessage
            {
                Data = new byte[bytes]
            };

            int byteRecv = receiver.Receive(messageReceived.Data);

            string message = DeserializeToString(messageReceived);

            return new Tuple<int, string>(byteRecv, message);
        }

        public static LogonPacket ReceiveLogonPacket (Socket receiver)
        {
            var dataHeaderReceived = SocketHelper.ReceiveBytesReceiveAndData(receiver, 36);
            return new LogonPacket(dataHeaderReceived.Item2);
        }
        
        public static PacketHeader ReceiveHeader(Socket receiver)
        {
            var dataHeaderReceived = SocketHelper.ReceiveBytesReceiveAndData(receiver, 20);
            return new PacketHeader(dataHeaderReceived.Item2);
        }

        public static string ReceiveData(Socket receiver, PacketHeader packetHeader)
        {
            var dataReceived = SocketHelper.ReceiveBytesReceiveAndData(receiver, packetHeader.DataSize);
            return dataReceived.Item2;
        }

        public static void CloseConnection(Socket socket)
        {
            // Close Socket using  
            // the method Close() 
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
