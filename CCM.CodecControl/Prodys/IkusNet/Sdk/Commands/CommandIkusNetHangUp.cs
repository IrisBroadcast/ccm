using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public sealed class CommandIkusNetHangUp : CommandBase
    {
        public CommandIkusNetHangUp() : base (Command.IkusNetHangUp, 4)
        {
        }

        public IkusNetCodec Codec { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            return ConvertHelper.EncodeUInt((uint)Codec, bytes, offset);
        }
    }
}