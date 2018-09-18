using System;
using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetInputEnabledResponse : IkusNetStatusResponseBase
    {
        public IkusNetGetInputEnabledResponse(Socket socket)
        {
            var response = GetResponseBytes(socket, Command.IkusNetGetInputEnabled, 4);
            Enabled = Convert.ToBoolean(ConvertHelper.DecodeUInt(response, 0));
        }

        public bool Enabled { get; set; }

    }
}