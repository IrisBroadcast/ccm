using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetGetGpi : CommandBase
    {
        public CommandIkusNetGetGpi() : base(Command.IkusNetGetGpi, 4)
        {
        }

        public int Gpio { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            return ConvertHelper.EncodeUInt((uint)Gpio, bytes, offset);
        }
    }
}