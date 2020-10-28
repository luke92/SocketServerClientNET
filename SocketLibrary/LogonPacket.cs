using System;

namespace SocketLibrary
{
    [Serializable]
    public class LogonPacket : PacketHeader
    {
        public string RouteCode { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        public LogonPacket(string packetMessage) : base(packetMessage)
        {
            if (packetMessage.Length < 36)
            {
                return;
            }

            RouteCode = packetMessage.Substring(19, 4);
            Password = packetMessage.Substring(23, 4);
            Version = packetMessage.Substring(27, 8);
        }

        public string GetLogonData()
        {
            return RouteCode + Password + Version;
        }

        public override string GetMessage()
        {
            return base.GetMessage() + GetLogonData();
        }
    }
}
