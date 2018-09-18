using System.Net.Sockets;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;
using CCM.Core.Extensions;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetLoadedPresetNameResponse : IkusNetStatusResponseBase
    {
        public string PresetName { get; set; }

        public IkusNetGetLoadedPresetNameResponse(Socket socket)
        {
            var responseBytes = GetResponseBytes(socket, Command.IkusNetGetLoadedPresetName, 256);
            PresetName = responseBytes.ToNullTerminatedString(0, 256);
        }
        
        
    }
}