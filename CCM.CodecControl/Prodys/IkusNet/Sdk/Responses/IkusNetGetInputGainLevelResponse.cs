using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetInputGainLevelResponse : IkusNetStatusResponseBase
    {
        public int GainLeveldB { get; set; }

        public IkusNetGetInputGainLevelResponse(Socket socket)
        {
            var responseBytes = GetResponseBytes(socket, Command.IkusNetGetInputGainLevel, 4);
            GainLeveldB = (int)ConvertHelper.DecodeUInt(responseBytes, 0);
        }

        
    }

   

}