using CCM.CodecControl.Helpers;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Commands.Base;
using CCM.CodecControl.Prodys.IkusNet.Sdk.Enums;

namespace CCM.CodecControl.Prodys.IkusNet.Sdk.Commands
{
    public class CommandIkusNetGetLoadedPresetName : CommandBase
    {
        public CommandIkusNetGetLoadedPresetName() : base(Command.IkusNetGetLoadedPresetName, 256)
        {
        }

        public string LastLoadedPresetName { get; set; }

        protected override int EncodePayload(byte[] bytes, int offset)
        {
            return ConvertHelper.EncodeString(LastLoadedPresetName, bytes, offset, 256);
        }

    }

}