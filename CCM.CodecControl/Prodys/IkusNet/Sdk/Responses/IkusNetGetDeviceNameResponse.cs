using System.Net.Sockets;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;
using CCM.Core.Extensions;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Responses
{
    public class IkusNetGetDeviceNameResponse : IkusNetStatusResponseBase
    {
        public string DeviceName { get; private set; }

        public IkusNetGetDeviceNameResponse(Socket socket)
        {
            var payload = GetResponseBytes(socket, Command.IkusNetSysGetDeviceName, 256);
            DeviceName = payload.ToNullTerminatedString(0, 256);
        }

    }
}