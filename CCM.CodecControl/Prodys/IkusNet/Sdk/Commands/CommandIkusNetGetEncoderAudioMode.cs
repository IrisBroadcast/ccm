using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetGetEncoderAudioMode : CommandBase
    {
        public CommandIkusNetGetEncoderAudioMode() : base(Command.IkusNetEncoderGetAudioMode, 4) {}

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            return ConvertHelper.EncodeUInt((uint)IkusNetCodec.Program, bytes, offset);
        }
    }

}