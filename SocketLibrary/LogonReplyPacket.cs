namespace SocketLibrary
{
    public class LogonReplyPacket : PacketData
    {
        public LogonReplyPacket(string packetMessage) : base(packetMessage) { }

        public LogonReplyPacket(PacketHeader packetHeader) : base(packetHeader.GetMessage()) {}
    }
}
