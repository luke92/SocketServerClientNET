using SocketLibrary;
using System; 
using System.Net.Sockets; 

namespace SocketServer
{

    class Server
    {

        // Main Method 
        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
            Socket listener = SocketHelper.GetSocket();
            
            try
            {
                listener.Bind(SocketHelper.GetEndPoint());

                listener.Listen(10);

                while (true)
                {

                    Console.WriteLine("Waiting connection ... ");
                    Console.WriteLine();

                    Socket clientSocket = listener.Accept();

                    LogonPacket logonPacket = SocketHelper.ReceiveLogonPacket(clientSocket);
                    Console.WriteLine("Text received -> {0} ", logonPacket.GetMessage());
                    Console.WriteLine();

                    var logonReply = new LogonReplyPacket("X0200005900000000000Logon Complete. 10 msgs queued for BAZ ");
                    SocketHelper.SendData(clientSocket, logonReply.GetMessage());
                                        
                    for(int i = 0; i < 3; i++)
                    {
                        var sendDataPacket = new SendDataPacket("S0200007000000055000The quick brown fox jumped over the lazy dogs tail");
                        SocketHelper.SendData(clientSocket, sendDataPacket.GetMessage());                        
                    }

                    SocketHelper.CloseConnection(clientSocket);

                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}