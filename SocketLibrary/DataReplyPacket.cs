using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    [Serializable]
    public class DataReplyPacket : PacketHeader
    {

        public DataReplyPacket(string packetHeader) : base(packetHeader)
        {

        }
    }
}
