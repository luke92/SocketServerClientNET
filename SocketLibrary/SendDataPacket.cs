namespace SocketLibrary
{
    public class SendDataPacket : PacketData
    {
        public SendDataPacket(string packetMessage) : base(packetMessage) { }

        public SendDataPacket(PacketHeader packetHeader) : base(packetHeader.GetMessage()) { }
    }
}
