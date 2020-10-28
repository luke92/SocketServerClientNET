using System;

namespace SocketLibrary
{
    [Serializable]
    public class PacketHeader
    {
        public PacketHeaderType Type { get; private set; }
        public string TypeString { get; }
        public int HeaderSize { get; set; }
        public string HeaderSizeString { get; private set; }
        public int MessageSize { get; set; }
        public string MessageSizeString { get; private set; }
        public int SequenceNumber { get; set; }
        public string SequenceNumberString { get; private set; }
        public PacketHeaderError Error { get; set; }
        public string ErrorString { get; private set; }

        public int DataSize
        {
            get
            {
                return MessageSize - HeaderSize;
            }
             
        }

        public string GetHeaderMessage()
        {
            return TypeString + HeaderSizeString + MessageSizeString + SequenceNumberString + ErrorString;
        }

        public virtual string GetMessage()
        {
            return GetHeaderMessage();
        }

        public PacketHeader(string dataHeader)
        {
            Type = PacketHeaderType.None;
            Error = PacketHeaderError.OtherError;

            if (dataHeader.Length < 20)
            {
                return;
            }

            var typeString = dataHeader[0].ToString().ToUpper();
            TypeString = typeString;

            if (typeString.Equals("L"))
                Type = PacketHeaderType.LogonContingent;

            if (typeString.Equals("X"))
                Type = PacketHeaderType.LogonDemand;

            if (typeString.Equals("M"))
                Type = PacketHeaderType.OperatorMessage;

            if (typeString.Equals("R"))
                Type = PacketHeaderType.DataReply;

            if (typeString.Equals("S"))
                Type = PacketHeaderType.LogonContingent;


            var headerSizeString = dataHeader.Substring(1, 3);
            HeaderSizeString = headerSizeString;
            int.TryParse(headerSizeString, out var headerSize);
            HeaderSize = headerSize;

            var messageSizeString = dataHeader.Substring(4, 5);
            MessageSizeString = messageSizeString;
            int.TryParse(messageSizeString, out var messageSize);
            MessageSize = messageSize;

            var sequenceString = dataHeader.Substring(9, 8);
            SequenceNumberString = sequenceString;
            int.TryParse(sequenceString, out var sequence);
            SequenceNumber = sequence;

            var CodeError = dataHeader.Substring(17, 3);
            ErrorString = CodeError;
            if (CodeError.Equals("000"))
            {
                Error = PacketHeaderError.None;
            }
            if (CodeError.Equals("020"))
            {
                Error = PacketHeaderError.WrongVersionNumberInLogonPacket;
            }
            if (CodeError.Equals("021"))
            {
                Error = PacketHeaderError.TransactionErrorDuringLogon;
            }
            if (CodeError.Equals("022"))
            {
                Error = PacketHeaderError.DeviceInInvalidState;
            }

        }

    }

    public enum PacketHeaderType
    {
        /// <summary>
        /// Logon if messages are queued 
        /// </summary>
        LogonContingent,
        /// <summary>
        /// Logon reguardless of message queue
        /// </summary>
        LogonDemand,
        /// <summary>
        /// Msg from Himalaya to remote node for operator or log
        /// </summary>
        OperatorMessage,
        /// <summary>
        /// Reply to Data Send 
        /// </summary>
        DataReply,
        /// <summary>
        /// Send Data for delivery 
        /// </summary>
        SendData,
        None
    }

    public enum PacketHeaderError
    {
        /// <summary>
        /// Possible Error Numbers from Himalaya "020"
        /// </summary>
        WrongVersionNumberInLogonPacket,
        /// <summary>
        /// Possible Error Numbers from Himalaya "021"
        /// </summary>
        TransactionErrorDuringLogon,
        /// <summary>
        /// Possible Error Numbers from Himalaya "022"
        /// </summary>
        DeviceInInvalidState,
        /// <summary>
        /// Possible Error Numbers from HP-UX (What ever you define)
        /// </summary>
        OtherError,
        None
    }
}
