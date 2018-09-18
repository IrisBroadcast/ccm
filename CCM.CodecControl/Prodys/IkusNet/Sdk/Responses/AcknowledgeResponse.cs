using System;
using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class AcknowledgeResponse : ResponseHeader
    {
        public Command ReceivedCommand { get; set; }
        public bool Acknowleged { get; set; }

        public AcknowledgeResponse(Socket socket)
        {
            var buffer = new byte[16];
            socket.Receive(buffer);

            Command = (Command)ConvertHelper.DecodeUInt(buffer, 0);
            Length = (int)ConvertHelper.DecodeUInt(buffer, 4);
            ReceivedCommand = (Command)ConvertHelper.DecodeUInt(buffer, 8);
            Acknowleged = Convert.ToBoolean(ConvertHelper.DecodeUInt(buffer, 12));

        }
        
        public override string ToString()
        {
            return string.Format("{0}, Received Command={1}, Acknowleged={2} ", base.ToString(), ReceivedCommand, Acknowleged);
        }
    }
}