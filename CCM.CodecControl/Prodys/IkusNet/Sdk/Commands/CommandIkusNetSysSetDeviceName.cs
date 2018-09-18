using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetSysSetDeviceName : CommandBase
    {
        public CommandIkusNetSysSetDeviceName() : base(Command.IkusNetSysSetDeviceName, 256)
        {
        }

        public string DeviceName { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            offset = ConvertHelper.EncodeString(DeviceName, bytes, offset, 256);
            return offset;
        }
    }

}