using SocketLibrary;
using System;
using System.Net.Sockets;

namespace SocketClient
{

    class Client
    {

        // Main Method 
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        // ExecuteClient() Method 
        static void ExecuteClient()
        {

            try
            {
                Socket sender = SocketHelper.GetSocket();
                
                try
                {
                    sender.Connect(SocketHelper.GetEndPoint());

                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    var login = new LogonPacket("X0200003600000000000BAZ HBQ108FEB02 ");
                    SocketHelper.SendData(sender, login.GetMessage());

                    LogonReplyPacket logonReplyPacket = new LogonReplyPacket(SocketHelper.ReceiveHeader(sender));
                    logonReplyPacket.Data = SocketHelper.ReceiveData(sender, logonReplyPacket);
                    Console.WriteLine("Message from Server -> {0}", logonReplyPacket.GetMessage());
                    
                    while (SocketHelper.SocketConnected(sender))
                    {
                        SendDataPacket sendDataPacket = new SendDataPacket(SocketHelper.ReceiveHeader(sender));
                        sendDataPacket.Data = SocketHelper.ReceiveData(sender, sendDataPacket);
                        Console.WriteLine("Data Packet Header-> {0}", sendDataPacket.GetMessage());
                    }

                    SocketHelper.CloseConnection(sender);
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}
