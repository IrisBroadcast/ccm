using System.Net.Sockets;
using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetDecoderAudioModeResponse : IkusNetStatusResponseBase
    {
        public static IkusNetGetDecoderAudioModeResponse GetResponse(Socket socket)
        {
            var responseBytes = GetResponseBytes(socket, Command.IkusNetDecoderGetAudioMode, 4);
            return new IkusNetGetDecoderAudioModeResponse(responseBytes);
        }

        public IkusNetDspAudioAlgorithm AudioAlgorithm { get; set; }

        public IkusNetGetDecoderAudioModeResponse(byte[] responseBytes)
        {
            AudioAlgorithm = (IkusNetDspAudioAlgorithm)ConvertHelper.DecodeUInt(responseBytes, 0);
        }

    }
}