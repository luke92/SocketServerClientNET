namespace SocketLibrary
{
    public class PacketData : PacketHeader
    {
        public string Data { get; set; }
        public PacketData(string packetMessage) : base(packetMessage)
        {
            if (packetMessage.Length < 20)
            {
                return;
            }

            if (packetMessage.Length > 20)
                Data = packetMessage.Substring(20);
        }

        public override string GetMessage()
        {
            return base.GetMessage() + Data;
        }
    }
}
