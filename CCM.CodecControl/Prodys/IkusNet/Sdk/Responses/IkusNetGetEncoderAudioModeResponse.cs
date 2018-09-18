using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetEncoderAudioModeResponse : IkusNetStatusResponseBase
    {
        public static IkusNetGetEncoderAudioModeResponse GetResponse(Socket socket)
        {
            var responseBytes = GetResponseBytes(socket, Command.IkusNetEncoderGetAudioMode, 4);
            return new IkusNetGetEncoderAudioModeResponse(responseBytes);
        }

        public IkusNetDspAudioAlgorithm AudioAlgorithm { get; set; }

        public IkusNetGetEncoderAudioModeResponse(byte[] responseBytes)
        {
            AudioAlgorithm = (IkusNetDspAudioAlgorithm)ConvertHelper.DecodeUInt(responseBytes, 0);
        }
      
    }
}