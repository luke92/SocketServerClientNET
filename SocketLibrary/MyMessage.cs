using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    [Serializable]
    public class MyMessage
    {
        public byte[] Data { get; set; }
    }
}
